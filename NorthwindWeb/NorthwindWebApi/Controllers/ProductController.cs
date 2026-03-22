using Microsoft.AspNetCore.Mvc;
using NorthwindWebApi.Models;
using NorthwindWebApi.Repositorio.Interfaces;

namespace NorthwindWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProduct _productRepo;
        public ProductController(IProduct productoRepo)
        {
            _productRepo = productoRepo;
        }

        [HttpGet("getProductos")]
        public async Task<ActionResult<IEnumerable<Product>>> getProductos(string producto = "")
        {
            var lista = await Task.Run(() => _productRepo.getProductos(producto));
            return Ok(lista);
        }

        [HttpGet("getProducto/{id}")]
        public async Task<ActionResult<Product>> getProducto(int id)
        {
            var producto = await Task.Run(() => _productRepo.getProducto(id));
            if (producto == null) return NotFound();
            return Ok(producto);
        }

        [HttpPost("insertProducto")]
        public async Task<ActionResult<string>> insertarProducto(Product reg)
        {
            var mensaje = await Task.Run(() => _productRepo.insertProducto(reg));
            return Ok(mensaje);
        }

        [HttpPut("updateProducto")]
        public async Task<ActionResult<string>> actualizarProducto(Product reg)
        {
            var mensaje = await Task.Run(() => _productRepo.updateProducto(reg));
            return Ok(mensaje);
        }

        [HttpDelete("deleteProducto/{id}")]
        public async Task<ActionResult<string>> eliminarProducto(int id)
        {
            var producto = await Task.Run(() => _productRepo.getProducto(id));
            if (producto == null) return NotFound();
            var mensaje = await Task.Run(() => _productRepo.deleteProducto(id));
            return Ok(mensaje);
        }
    }
}
