using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NorthwindWebMvc.Filters;
using NorthwindWebMvc.Models;
using NorthwindWebMvc.Models.StoreViewModels;
using System.Text;

namespace NorthwindWebMvc.Controllers
{
    [SessionRoleAuthorize(BlockedRole = "Admin")]
    public class StoreController : Controller
    {
        private const string CartSessionKey = "ShoppingCart";
        private const string OrdersSessionKey = "UserOrders";
        private const string ApiBaseUrl = "https://localhost:7226/api/";

        private async Task<List<Category>> GetCategories()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiBaseUrl);
                HttpResponseMessage response = await client.GetAsync("Category/getCategorias");
                string apiResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<Category>>(apiResponse) ?? new List<Category>();
            }
        }

        private async Task<List<Product>> GetProducts(int? categoryID = null, string? searchTerm = null)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiBaseUrl);
                string endpoint = "Product/getProductos";
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    endpoint += $"?producto={Uri.EscapeDataString(searchTerm)}";
                }
                HttpResponseMessage response = await client.GetAsync(endpoint);
                string apiResponse = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<List<Product>>(apiResponse) ?? new List<Product>();

                if (categoryID.HasValue && categoryID > 0)
                {
                    products = products.Where(p => p.CategoryID == categoryID).ToList();
                }

                return products;
            }
        }

        private async Task<Product> GetProductDetail(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiBaseUrl);
                HttpResponseMessage response = await client.GetAsync($"Product/getProducto/{id}");
                string apiResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Product>(apiResponse);
            }
        }

        private CartViewModel GetCart()
        {
            string? cartJson = HttpContext.Session.GetString(CartSessionKey);
            if (string.IsNullOrEmpty(cartJson))
            {
                return new CartViewModel();
            }
            return JsonConvert.DeserializeObject<CartViewModel>(cartJson) ?? new CartViewModel();
        }

        private void SaveCart(CartViewModel cart)
        {
            string cartJson = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString(CartSessionKey, cartJson);
        }

        private List<OrderViewModel> GetOrders()
        {
            string? ordersJson = HttpContext.Session.GetString(OrdersSessionKey);
            if (string.IsNullOrEmpty(ordersJson))
            {
                return new List<OrderViewModel>();
            }
            return JsonConvert.DeserializeObject<List<OrderViewModel>>(ordersJson) ?? new List<OrderViewModel>();
        }

        private void SaveOrders(List<OrderViewModel> orders)
        {
            string ordersJson = JsonConvert.SerializeObject(orders);
            HttpContext.Session.SetString(OrdersSessionKey, ordersJson);
        }

        public async Task<IActionResult> Catalog(int? categoryID = null, string? search = null, int page = 1, int pageSize = 9)
        {
            var categories = await GetCategories();
            var products = await GetProducts(categoryID, search);

            if (pageSize <= 0) pageSize = 9;
            if (page <= 0) page = 1;

            var totalItems = products.Count;
            var pagedProducts = products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var viewModel = new CatalogViewModel
            {
                Products = pagedProducts,
                Categories = categories,
                SelectedCategoryID = categoryID,
                SearchTerm = search,
                CurrentPage = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            return View(viewModel);
        }

        public async Task<IActionResult> ProductDetail(int id)
        {
            if (id <= 0)
                return RedirectToAction("Catalog");

            var product = await GetProductDetail(id);
            if (product == null)
                return RedirectToAction("Catalog");

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productID, int quantity = 1)
        {
            if (quantity <= 0)
                quantity = 1;

            var product = await GetProductDetail(productID);
            if (product == null)
                return RedirectToAction("Catalog");

            var cart = GetCart();
            var existingItem = cart.Items.FirstOrDefault(x => x.ProductID == productID);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    ProductID = productID,
                    ProductName = product.ProductName,
                    UnitPrice = product.UnitPrice ?? 0,
                    Quantity = quantity
                });
            }

            SaveCart(cart);
            TempData["mensaje"] = "Producto agregado al carrito";
            return RedirectToAction("ProductDetail", new { id = productID });
        }

        public IActionResult Cart()
        {
            var cart = GetCart();
            return View(cart);
        }

        [HttpPost]
        public IActionResult UpdateCart(List<CartItem> items)
        {
            var cart = new CartViewModel { Items = items ?? new List<CartItem>() };
            SaveCart(cart);
            TempData["mensaje"] = "Carrito actualizado";
            return RedirectToAction("Cart");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productID)
        {
            var cart = GetCart();
            cart.Items.RemoveAll(x => x.ProductID == productID);
            SaveCart(cart);
            TempData["mensaje"] = "Producto removido del carrito";
            return RedirectToAction("Cart");
        }

        public IActionResult Checkout()
        {
            var cart = GetCart();
            if (cart.Items.Count == 0)
                return RedirectToAction("Catalog");

            var viewModel = new CheckoutViewModel
            {
                Items = cart.Items
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult ProcessCheckout(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var cart = GetCart();
                model.Items = cart.Items;
                return View("Checkout", model);
            }

            var cart2 = GetCart();
            if (cart2.Items.Count == 0)
                return RedirectToAction("Catalog");

            var orders = GetOrders();
            var newOrder = new OrderViewModel
            {
                OrderID = orders.Count > 0 ? orders.Max(o => o.OrderID) + 1 : 1,
                OrderDate = DateTime.Now,
                Items = cart2.Items.ToList(),
                Total = cart2.Total
            };

            orders.Add(newOrder);
            SaveOrders(orders);
            SaveCart(new CartViewModel());

            TempData["ordenID"] = newOrder.OrderID;
            return RedirectToAction("OrderConfirmation", new { orderID = newOrder.OrderID });
        }

        public IActionResult OrderConfirmation(int orderID)
        {
            var orders = GetOrders();
            var order = orders.FirstOrDefault(o => o.OrderID == orderID);

            if (order == null)
                return RedirectToAction("Catalog");

            return View(order);
        }

        public IActionResult MyOrders()
        {
            var orders = GetOrders();
            return View(orders.OrderByDescending(o => o.OrderDate).ToList());
        }

        public IActionResult OrderDetails(int orderID)
        {
            var orders = GetOrders();
            var order = orders.FirstOrDefault(o => o.OrderID == orderID);

            if (order == null)
                return RedirectToAction("MyOrders");

            return View(order);
        }

        [HttpPost]
        public IActionResult ClearCart()
        {
            SaveCart(new CartViewModel());
            TempData["mensaje"] = "Carrito vaciado";
            return RedirectToAction("Catalog");
        }
    }
}
