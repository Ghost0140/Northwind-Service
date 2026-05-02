using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NorthwindWebMvc.Filters;
using NorthwindWebMvc.Models;
using System.Text;

namespace NorthwindWebMvc.Controllers
{
    [SessionRoleAuthorize(RequiredRole = "Admin")]
    public class UsuarioController : Controller
    {
        private readonly IConfiguration _configuration;

        public UsuarioController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private bool EsAdmin()
        {
            return HttpContext.Session.GetString("rol") == "Admin";
        }

        private async Task<int?> ObtenerIdAdminProtegido()
        {
            using (var client = new HttpClient())
            {
                string apiBase = _configuration["ApiSettings:BaseUrl"];
                client.BaseAddress = new Uri(apiBase);

                HttpResponseMessage response = await client.GetAsync("api/Usuario/listar");
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

                string apiResponse = await response.Content.ReadAsStringAsync();
                var lista = JsonConvert.DeserializeObject<List<Usuario>>(apiResponse) ?? new List<Usuario>();

                return lista
                    .Where(u => string.Equals(u.Rol, "Admin", StringComparison.OrdinalIgnoreCase))
                    .OrderBy(u => u.IdUsuario)
                    .Select(u => (int?)u.IdUsuario)
                    .FirstOrDefault();
            }
        }

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("usuario") == null)
                return RedirectToAction("Login", "Account");

            if (!EsAdmin())
                return RedirectToAction("Index", "Home");

            List<Usuario> lista = new List<Usuario>();

            using (var client = new HttpClient())
            {
                string apiBase = _configuration["ApiSettings:BaseUrl"];
                client.BaseAddress = new Uri(apiBase);

                HttpResponseMessage response = await client.GetAsync("api/Usuario/listar");

                if (response.IsSuccessStatusCode)
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    lista = JsonConvert.DeserializeObject<List<Usuario>>(apiResponse);
                }
            }

            ViewBag.AdminProtegidoId = await ObtenerIdAdminProtegido();
            return View(lista);
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("usuario") == null)
                return RedirectToAction("Login", "Account");

            if (!EsAdmin())
                return RedirectToAction("Index", "Home");

            return View(new Usuario { Estado = true });
        }

        [HttpPost]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            if (HttpContext.Session.GetString("usuario") == null)
                return RedirectToAction("Login", "Account");

            if (!EsAdmin())
                return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
                return View(usuario);

            using (var client = new HttpClient())
            {
                string apiBase = _configuration["ApiSettings:BaseUrl"];
                client.BaseAddress = new Uri(apiBase);

                var json = JsonConvert.SerializeObject(usuario);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync("api/Usuario/registrar", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["mensaje"] = await response.Content.ReadAsStringAsync();
                    return RedirectToAction("Index");
                }
            }

            ViewBag.Error = "No se pudo registrar el usuario";
            return View(usuario);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (HttpContext.Session.GetString("rol") != "Admin")
                return RedirectToAction("Index", "Home");
            
            // Obtener usuario a desactivar
            Usuario usuario = new Usuario();

            using (var client = new HttpClient())
            {
                string apiBase = _configuration["ApiSettings:BaseUrl"];
                client.BaseAddress = new Uri(apiBase);

                var response = await client.GetAsync($"api/Usuario/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    usuario = JsonConvert.DeserializeObject<Usuario>(json);

                    // Proteger siempre al primer usuario Admin creado.
                    int? adminProtegidoId = await ObtenerIdAdminProtegido();
                    if (adminProtegidoId.HasValue && usuario.IdUsuario == adminProtegidoId.Value)
                    {
                        TempData["error"] = "El primer usuario Admin creado no puede desactivarse";
                        return RedirectToAction("Index");
                    }

                    // Cambiar estado a inactivo
                    usuario.Estado = false;

                    var jsonUpdate = JsonConvert.SerializeObject(usuario);
                    var content = new StringContent(jsonUpdate, Encoding.UTF8, "application/json");

                    await client.PutAsync("api/Usuario/actualizar", content);

                    TempData["mensaje"] = "Usuario desactivado correctamente";
                }
            }

            return RedirectToAction("Index");
        }

        // GET EDIT
        public async Task<IActionResult> Edit(int id)
        {
            if (HttpContext.Session.GetString("rol") != "Admin")
                return RedirectToAction("Index", "Home");

            Usuario usuario = new Usuario();

            using (var client = new HttpClient())
            {
                string apiBase = _configuration["ApiSettings:BaseUrl"];
                client.BaseAddress = new Uri(apiBase);

                var response = await client.GetAsync($"api/Usuario/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    usuario = JsonConvert.DeserializeObject<Usuario>(json);
                }
            }

            return View(usuario);
        }

        // POST EDIT
        [HttpPost]
        public async Task<IActionResult> Edit(Usuario usuario)
        {
            if (HttpContext.Session.GetString("rol") != "Admin")
                return RedirectToAction("Index", "Home");

            if (!ModelState.IsValid)
            {
                return View(usuario);
            }

            try
            {
                using (var client = new HttpClient())
                {
                    string apiBase = _configuration["ApiSettings:BaseUrl"];
                    client.BaseAddress = new Uri(apiBase);

                    var json = JsonConvert.SerializeObject(usuario);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync("api/Usuario/actualizar", content);

                    if (response.IsSuccessStatusCode)
                    {
                        TempData["mensaje"] = "Usuario actualizado correctamente";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        ViewBag.Error = $"Error al actualizar: {errorContent}";
                        return View(usuario);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error: {ex.Message}";
                return View(usuario);
            }
        }
    }
}