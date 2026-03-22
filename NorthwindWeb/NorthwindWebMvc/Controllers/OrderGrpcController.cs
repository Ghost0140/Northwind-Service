using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using NorthwindgRPC;
using NorthwindWebMvc.Models;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace NorthwindWebMvc.Controllers
{
    public class OrderGrpcController : Controller
    {
        private Orders.OrdersClient _client;



        public async Task<IActionResult> FindByCustomer()
        {
            var canal = GrpcChannel.ForAddress("https://localhost:7075");
            _client = new Orders.OrdersClient(canal);

            // CustomerID vacío → listar todos
            var request = new CustomerFilter
            {
                CustomerID = ""
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

            return View(temporal);
        }

        [HttpPost]
        public async Task<IActionResult> FindByCustomer(string customerID)
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
                temporal.Add(new Order()
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
            return View(temporal);
        }






        public async Task<IActionResult> BetweenDates()
        {
            var canal = GrpcChannel.ForAddress("https://localhost:7075");
            _client = new Orders.OrdersClient(canal);

            var request = new DateFilter
            {
                FechaInicio = "01-01-1996",
                FechaFin = DateTime.Today.ToString("dd-MM-yyyy")
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

            return View(lista);
        }



        [HttpPost]
        public async Task<IActionResult> BetweenDates(DateTime? fechaInicio, DateTime? fechaFin)
        {
            if (!fechaInicio.HasValue || !fechaFin.HasValue)
            {
                ViewBag.Error = "Digite una fecha de inicio y fin.";
                return View(new List<Order>());
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

            return View(lista);
        }





    }
}
