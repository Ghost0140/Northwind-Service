using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using NorthwindgRPC;
using NorthwindWebMvc.Filters;
using NorthwindWebMvc.Models;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace NorthwindWebMvc.Controllers
{
    [SessionRoleAuthorize(RequiredRole = "Admin")]
    public class OrderGrpcController : Controller
    {
        private Orders.OrdersClient _client;



        public async Task<IActionResult> FindByCustomer(string customerID = "", int page = 1, int pageSize = 10)
        {
            var canal = GrpcChannel.ForAddress("https://localhost:7075");
            _client = new Orders.OrdersClient(canal);

            var request = new CustomerFilter
            {
                CustomerID = customerID
            };

            OrdenResponse mensaje = await _client.GetByCustomerAsync(request);

            List<Order> temporal = new List<Order>();

            foreach (Orden reg in mensaje.Items)
            {
                temporal.Add(new Order
                {
                    OrderID = reg.OrderID,
                    CustomerID = reg.CustomerID,
                    CompanyName = reg.CompanyName,
                    OrderDate = reg.OrderDate,
                    RequiredDate = reg.RequiredDate,
                    ShippedDate = reg.ShippedDate,
                    ShipName = reg.ShipName,
                    ShipCity = reg.ShipCity,
                    ShipCountry = reg.ShipCountry
                });
            }

            ViewBag.CustomerID = customerID;

            var paged = new PagedList<Order>
            {
                Items = temporal.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = temporal.Count
            };

            return View(paged);
        }

        [HttpPost]
        public IActionResult FindByCustomer(string customerID)
        {
            return RedirectToAction("FindByCustomer", new { customerID = customerID });
        }






        public async Task<IActionResult> BetweenDates(string? fechaInicio, string? fechaFin, int page = 1, int pageSize = 10)
        {
            var canal = GrpcChannel.ForAddress("https://localhost:7075");
            _client = new Orders.OrdersClient(canal);

            DateTime startDate;
            DateTime endDate;

            // If no dates provided, query full range but keep inputs empty
            if (string.IsNullOrWhiteSpace(fechaInicio) || string.IsNullOrWhiteSpace(fechaFin))
            {
                // If there was an error from a previous POST, expose it
                if (TempData["Error"] != null)
                {
                    ViewBag.Error = TempData["Error"].ToString();
                }

                startDate = new DateTime(1996, 1, 1);
                endDate = DateTime.Today;
                ViewBag.FechaInicio = null; // keep inputs empty
                ViewBag.FechaFin = null;
            }
            else
            {
                // Expecting yyyy-MM-dd from input type=date
                if (!DateTime.TryParse(fechaInicio, out startDate) || !DateTime.TryParse(fechaFin, out endDate))
                {
                    ViewBag.Error = "Formato de fecha inválido.";
                    ViewBag.FechaInicio = fechaInicio;
                    ViewBag.FechaFin = fechaFin;
                    return View(new PagedList<Order> { Items = new List<Order>(), PageNumber = 1, PageSize = pageSize, TotalItems = 0 });
                }

                if (startDate > endDate)
                {
                    ViewBag.Error = "La fecha de inicio no puede ser mayor a la fecha fin.";
                    ViewBag.FechaInicio = fechaInicio;
                    ViewBag.FechaFin = fechaFin;
                    return View(new PagedList<Order> { Items = new List<Order>(), PageNumber = 1, PageSize = pageSize, TotalItems = 0 });
                }

                ViewBag.FechaInicio = startDate.ToString("yyyy-MM-dd");
                ViewBag.FechaFin = endDate.ToString("yyyy-MM-dd");
            }

            var request = new DateFilter
            {
                FechaInicio = startDate.ToString("dd-MM-yyyy"),
                FechaFin = endDate.ToString("dd-MM-yyyy")
            };

            var response = await _client.GetBetweenDatesAsync(request);

            List<Order> lista = response.Items.Select(reg => new Order
            {
                OrderID = reg.OrderID,
                CustomerID = reg.CustomerID,
                CompanyName = reg.CompanyName,
                OrderDate = reg.OrderDate,
                RequiredDate = reg.RequiredDate,
                ShippedDate = reg.ShippedDate,
                ShipName = reg.ShipName,
                ShipCity = reg.ShipCity,
                ShipCountry = reg.ShipCountry
            }).ToList();

            var paged = new PagedList<Order>
            {
                Items = lista.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = lista.Count
            };

            return View(paged);
        }



        [HttpPost]
        public async Task<IActionResult> BetweenDates(DateTime? fechaInicio, DateTime? fechaFin, int page = 1, int pageSize = 10)
        {
            if (!fechaInicio.HasValue || !fechaFin.HasValue)
            {
                ViewBag.Error = "Digite una fecha de inicio y fin.";
                return View(new PagedList<Order> { Items = new List<Order>(), PageNumber = 1, PageSize = pageSize, TotalItems = 0 });
            }

            if (fechaInicio > fechaFin)
            {
                ViewBag.Error = "La fecha de inicio no puede ser mayor a la fecha fin.";
                return View(new List<Order>());
            }

            var canal = GrpcChannel.ForAddress("https://localhost:7075");
            _client = new Orders.OrdersClient(canal);

            var request = new DateFilter
            {
                FechaInicio = fechaInicio.Value.ToString("dd-MM-yyyy"),
                FechaFin = fechaFin.Value.ToString("dd-MM-yyyy")
            };

            var response = await _client.GetBetweenDatesAsync(request);

            List<Order> lista = response.Items.Select(reg => new Order
            {
                OrderID = reg.OrderID,
                CustomerID = reg.CustomerID,
                CompanyName = reg.CompanyName,
                OrderDate = reg.OrderDate,
                RequiredDate = reg.RequiredDate,
                ShippedDate = reg.ShippedDate,
                ShipName = reg.ShipName,
                ShipCity = reg.ShipCity,
                ShipCountry = reg.ShipCountry
            }).ToList();

            ViewBag.FechaInicio = request.FechaInicio;
            ViewBag.FechaFin = request.FechaFin;

            var paged = new PagedList<Order>
            {
                Items = lista.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = lista.Count
            };

            return View(paged);
        }





    }
}
