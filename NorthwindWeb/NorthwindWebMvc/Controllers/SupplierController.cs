using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using NorthwindWebMvc.Models;
using System.Text;

namespace NorthwindWebMvc.Controllers
{
    public class SupplierController : Controller
    {
        private async Task<List<Cargo>> CargarCargos()
        {
            List<Cargo> listaCargos = new List<Cargo>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7226/api/Cargo/");
                HttpResponseMessage response = await client.GetAsync("getCargos");
                string apiResponse = await response.Content.ReadAsStringAsync();
                listaCargos = JsonConvert.DeserializeObject<List<Cargo>>(apiResponse).ToList();
            }
            return listaCargos;
        }
        private async Task<List<Pais>> CargarPaises()
        {
            List<Pais> listaPaises = new List<Pais>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7226/api/Pais/");
                HttpResponseMessage response = await client.GetAsync("getPaises");
                string apiResponse = await response.Content.ReadAsStringAsync();
                listaPaises = JsonConvert.DeserializeObject<List<Pais>>(apiResponse).ToList();
            }
            return listaPaises;
        }
        public async Task<IActionResult> Index(string? nombre, int page = 1, int pageSize = 10)
        {
            List<Supplier> temporal = new List<Supplier>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7226/api/Supplier/");
                string url = string.IsNullOrWhiteSpace(nombre) ? "getProveedores" : $"getProveedores?nombre={nombre}";
                HttpResponseMessage response = await client.GetAsync(url);
                string apiResponse = await response.Content.ReadAsStringAsync();
                temporal = JsonConvert.DeserializeObject<List<Supplier>>(apiResponse).ToList();
            }
            ViewBag.Nombre = nombre;
            var paged = new PagedList<Supplier>
            {
                Items = temporal.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = temporal.Count
            };
            return View(paged);
        }
        public async Task<IActionResult> Create()
        {
            List<Cargo> listaCargos = await CargarCargos();
            List<Pais> listaPaises = await CargarPaises();
            ViewBag.Cargos = new SelectList(listaCargos, "CargoID", "NombreCargo");
            ViewBag.Paises = new SelectList(listaPaises, "PaisID", "NombrePais");
            return View(await Task.Run(() => new Supplier()));
        }

        [HttpPost]
        public async Task<IActionResult> Create(Supplier reg)
        {
            if (!ModelState.IsValid)
            {
                List<Cargo> listaCargos = await CargarCargos();
                List<Pais> listaPaises = await CargarPaises();
                ViewBag.Cargos = new SelectList(listaCargos, "CargoID", "NombreCargo");
                ViewBag.Paises = new SelectList(listaPaises, "PaisID", "NombrePais");
                return View(await Task.Run(() => reg));
            }
            string mensaje = "";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7226/api/Supplier/");
                StringContent content = new StringContent(JsonConvert.SerializeObject(reg), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("insertProveedor", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                mensaje = apiResponse;
            }
            TempData["mensaje"] = mensaje;
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                return RedirectToAction("Index");
            }
            Supplier reg = new Supplier();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7226/api/Supplier/");
                HttpResponseMessage response = await client.GetAsync("getProveedor/" + id);
                string apiResponse = await response.Content.ReadAsStringAsync();
                reg = JsonConvert.DeserializeObject<Supplier>(apiResponse);
                List<Cargo> listaCargos = await CargarCargos();
                List<Pais> listaPaises = await CargarPaises();
                ViewBag.Cargos = new SelectList(listaCargos, "CargoID", "NombreCargo");
                ViewBag.Paises = new SelectList(listaPaises, "PaisID", "NombrePais");
            }
            return View(await Task.Run(() => reg));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Supplier reg)
        {
            if (!ModelState.IsValid)
            {
                List<Cargo> listaCargos = await CargarCargos();
                List<Pais> listaPaises = await CargarPaises();
                ViewBag.Cargos = new SelectList(listaCargos, "CargoID", "NombreCargo");
                ViewBag.Paises = new SelectList(listaPaises, "PaisID", "NombrePais");
                return View(await Task.Run(() => reg));
            }
            string mensaje = "";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7226/api/Supplier/");
                StringContent content = new StringContent(JsonConvert.SerializeObject(reg), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync("updateProveedor", content);
                string apiResponse = await response.Content.ReadAsStringAsync();
                mensaje = apiResponse;
            }
            TempData["mensaje"] = mensaje;
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                return RedirectToAction("Index");
            }
            Supplier reg = new Supplier();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7226/api/Supplier/");
                HttpResponseMessage response = await client.GetAsync("getProveedor/" + id);
                string apiResponse = await response.Content.ReadAsStringAsync();
                reg = JsonConvert.DeserializeObject<Supplier>(apiResponse);
            }
            return View(await Task.Run(() => reg));
        }
    }
}
