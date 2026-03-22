using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NorthwindWebMvc.Models;
using System.Diagnostics;

namespace NorthwindWebMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            Home model = new Home();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7226/api/Home/");
                HttpResponseMessage response = await client.GetAsync("getTotales");
                string apiResponse = await response.Content.ReadAsStringAsync();
                model = JsonConvert.DeserializeObject<Home>(apiResponse);
            }
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
