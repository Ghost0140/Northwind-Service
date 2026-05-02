using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NorthwindWebMvc.Filters;
using NorthwindWebMvc.Models;
using System.Text;

namespace NorthwindWebMvc.Controllers
{
    [SessionRoleAuthorize(RequiredRole = "Admin")]
    public class ShipperController : Controller
    {
        public async Task<IActionResult> Index(string? nombre, int page = 1, int pageSize = 10)
        {
            List<Shipper> temporal = new List<Shipper>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7226/api/Shipper/");
                string url = string.IsNullOrWhiteSpace(nombre) ? "getTransportistas" : $"getTransportistas?nombre={nombre}";
                HttpResponseMessage response = await client.GetAsync(url);
                string apiResponse = await response.Content.ReadAsStringAsync();
                temporal = JsonConvert.DeserializeObject<List<Shipper>>(apiResponse).ToList();
            }
            ViewBag.Nombre = nombre;
            var paged = new PagedList<Shipper>
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
            return View(await Task.Run(() => new Shipper()));
        }

        [HttpPost]
        public async Task<IActionResult> Create(Shipper reg)
        {
            if (!ModelState.IsValid)
            {
                return View(await Task.Run(() => reg));
            }
            string mensaje = "";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7226/api/Shipper/");
                StringContent content = new StringContent(JsonConvert.SerializeObject(reg), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync("insertTransportista", content);
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
            Shipper reg = new Shipper();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7226/api/Shipper/");
                HttpResponseMessage response = await client.GetAsync("getTransportista/" + id);
                string apiResponse = await response.Content.ReadAsStringAsync();
                reg = JsonConvert.DeserializeObject<Shipper>(apiResponse);
            }
            return View(await Task.Run(() => reg));
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Shipper reg)
        {
            if (!ModelState.IsValid)
            {
                return View(await Task.Run(() => reg));
            }
            string mensaje = "";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7226/api/Shipper/");
                StringContent content = new StringContent(JsonConvert.SerializeObject(reg), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PutAsync("updateTransportista", content);
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
            Shipper reg = new Shipper();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7226/api/Shipper/");
                HttpResponseMessage response = await client.GetAsync("getTransportista/" + id);
                string apiResponse = await response.Content.ReadAsStringAsync();
                reg = JsonConvert.DeserializeObject<Shipper>(apiResponse);
            }
            return View(await Task.Run(() => reg));
        }
    }
}
