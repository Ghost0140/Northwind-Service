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
        public async Task<IActionResult> FindBySupplier(string CompanyName = "", int page = 1, int pageSize = 10)
        {
            var canal = GrpcChannel.ForAddress("https://localhost:7075");
            _client = new Products.ProductsClient(canal);

            List<Product> temporal = new List<Product>();

            if (string.IsNullOrWhiteSpace(CompanyName))
            {
                var request = new Empty();
                var mensaje = await _client.GetAllAsync(request);
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
            }
            else
            {
                var request = new SupplierFilter { SupplierName = CompanyName };
                var mensaje = await _client.GetBySupplierAsync(request);
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
            }

            ViewBag.CompanyName = CompanyName;

            var paged = new PagedList<Product>
            {
                Items = temporal.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = temporal.Count
            };

            return View(paged);
        }

        [HttpPost]
        public IActionResult FindBySupplier(string CompanyName)
        {
            // Redirect to GET so the filter is reflected in the query string and pagination links
            return RedirectToAction("FindBySupplier", new { CompanyName = CompanyName });
        }
        public async Task<IActionResult> FindByCategory(string CategoryName = "", int page = 1, int pageSize = 10)
        {
            var canal = GrpcChannel.ForAddress("https://localhost:7075");
            _client = new Products.ProductsClient(canal);

            List<Product> temporal = new List<Product>();

            if (string.IsNullOrWhiteSpace(CategoryName))
            {
                var request = new Empty();
                var mensaje = await _client.GetAllAsync(request);
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
            }
            else
            {
                var request = new CategoryFilter { CategoryName = CategoryName };
                var mensaje = await _client.GetByCategoryAsync(request);
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
            }

            ViewBag.CategoryName = CategoryName;

            var paged = new PagedList<Product>
            {
                Items = temporal.Skip((page - 1) * pageSize).Take(pageSize).ToList(),
                PageNumber = page,
                PageSize = pageSize,
                TotalItems = temporal.Count
            };

            return View(paged);
        }

        [HttpPost]
        public IActionResult FindByCategory(string CategoryName)
        {
            return RedirectToAction("FindByCategory", new { CategoryName = CategoryName });
        }
    }
}
