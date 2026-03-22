using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc;
using NorthwindgRPC;
using NorthwindWebMvc.Models;

namespace NorthwindWebMvc.Controllers
{
    public class ProductGrpcController : Controller
    {
        private Products.ProductsClient _client;

        public async Task<IActionResult> Lista()
        {
            var canal = GrpcChannel.ForAddress("https://localhost:7075");
            _client = new Products.ProductsClient(canal);
            var request = new Empty();
            var mensaje = await _client.GetAllAsync(request);

            List<Product> temporal = new List<Product>();
            foreach (var reg in mensaje.Items)
            {
                temporal.Add(new Product()
                {
                    ProductID = reg.ProductID,
                    ProductName = reg.ProductName,
                    SupplierName = reg.SupplierName,
                    CategoryName = reg.CategoryName,
                    QuantityPerUnit = reg.QuantityPerUnit,
                    UnitPrice = (decimal)reg.UnitPrice,
                    UnitsInStock = (short)reg.UnitsInStock,
                    UnitsOnOrder = (short)reg.UnitsOnOrder,
                    ReorderLevel = (short)reg.ReorderLevel
                });
            }
            return View(temporal);
        }
        public async Task<IActionResult> FindBySupplier()
        {
            var canal = GrpcChannel.ForAddress("https://localhost:7075");
            _client = new Products.ProductsClient(canal);
            var request = new Empty();
            var mensaje = await _client.GetAllAsync(request);

            List<Product> temporal = new List<Product>();
            foreach (var reg in mensaje.Items)
            {
                temporal.Add(new Product()
                {
                    ProductID = reg.ProductID,
                    ProductName = reg.ProductName,
                    SupplierName = reg.SupplierName,
                    CategoryName = reg.CategoryName,
                    QuantityPerUnit = reg.QuantityPerUnit,
                    UnitPrice = (decimal)reg.UnitPrice,
                    UnitsInStock = (short)reg.UnitsInStock,
                    UnitsOnOrder = (short)reg.UnitsOnOrder,
                    ReorderLevel = (short)reg.ReorderLevel
                });
            }
            return View(temporal);
        }

        [HttpPost]
        public async Task<IActionResult> FindBySupplier(string CompanyName)
        {
            var canal = GrpcChannel.ForAddress("https://localhost:7075");
            _client = new Products.ProductsClient(canal);
            var request = new SupplierFilter();
            request.SupplierName = CompanyName;
            var mensaje = await _client.GetBySupplierAsync(request);

            List<Product> temporal = new List<Product>();
            foreach (var reg in mensaje.Items)
            {
                temporal.Add(new Product()
                {
                    ProductID = reg.ProductID,
                    ProductName = reg.ProductName,
                    SupplierName = reg.SupplierName,
                    CategoryName = reg.CategoryName,
                    QuantityPerUnit = reg.QuantityPerUnit,
                    UnitPrice = (decimal)reg.UnitPrice,
                    UnitsInStock = (short)reg.UnitsInStock,
                    UnitsOnOrder = (short)reg.UnitsOnOrder,
                    ReorderLevel = (short)reg.ReorderLevel
                });
            }
            ViewBag.CompanyName = CompanyName;
            return View(temporal);

        }
        public async Task<IActionResult> FindByCategory()
        {
            var canal = GrpcChannel.ForAddress("https://localhost:7075");
            _client = new Products.ProductsClient(canal);
            var request = new Empty();
            var mensaje = await _client.GetAllAsync(request);

            List<Product> temporal = new List<Product>();
            foreach (var reg in mensaje.Items)
            {
                temporal.Add(new Product()
                {
                    ProductID = reg.ProductID,
                    ProductName = reg.ProductName,
                    SupplierName = reg.SupplierName,
                    CategoryName = reg.CategoryName,
                    QuantityPerUnit = reg.QuantityPerUnit,
                    UnitPrice = (decimal)reg.UnitPrice,
                    UnitsInStock = (short)reg.UnitsInStock,
                    UnitsOnOrder = (short)reg.UnitsOnOrder,
                    ReorderLevel = (short)reg.ReorderLevel
                });
            }
            return View(temporal);
        }

        [HttpPost]
        public async Task<IActionResult> FindByCategory(string CategoryName)
        {
            var canal = GrpcChannel.ForAddress("https://localhost:7075");
            _client = new Products.ProductsClient(canal);
            var request = new CategoryFilter();
            request.CategoryName = CategoryName;
            var mensaje = await _client.GetByCategoryAsync(request);

            List<Product> temporal = new List<Product>();
            foreach (var reg in mensaje.Items)
            {
                temporal.Add(new Product()
                {
                    ProductID = reg.ProductID,
                    ProductName = reg.ProductName,
                    SupplierName = reg.SupplierName,
                    CategoryName = reg.CategoryName,
                    QuantityPerUnit = reg.QuantityPerUnit,
                    UnitPrice = (decimal)reg.UnitPrice,
                    UnitsInStock = (short)reg.UnitsInStock,
                    UnitsOnOrder = (short)reg.UnitsOnOrder,
                    ReorderLevel = (short)reg.ReorderLevel
                });
            }
            ViewBag.CategoryName = CategoryName;
            return View(temporal);
        }
    }
}
