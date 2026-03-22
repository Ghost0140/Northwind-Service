using Microsoft.AspNetCore.Mvc;
using NorthwindWebApi.Models;
using NorthwindWebApi.Repositorio.Interfaces;

namespace NorthwindWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplier _proveedorRepo;
        public SupplierController(ISupplier proveedorRepo)
        {
            _proveedorRepo = proveedorRepo;
        }

        [HttpGet("getProveedores")]
        public async Task<ActionResult<IEnumerable<Supplier>>> getProveedores(string? nombre)
        {
            var lista = await Task.Run(() => _proveedorRepo.getProveedores(nombre));
            return Ok(lista);
        }

        [HttpGet("getProveedor/{id}")]
        public async Task<ActionResult<Supplier>> getProveedor(int id)
        {
            var proveedor = await Task.Run(() => _proveedorRepo.getProveedor(id));
            if (proveedor == null) return NotFound();
            return Ok(proveedor);
        }

        [HttpPost("insertProveedor")]
        public async Task<ActionResult<string>> insertarProveedor(Supplier reg)
        {
            var mensaje = await Task.Run(() => _proveedorRepo.insertProveedor(reg));
            return Ok(mensaje);
        }

        [HttpPut("updateProveedor")]
        public async Task<ActionResult<string>> actualizarProveedor(Supplier reg)
        {
            var mensaje = await Task.Run(() => _proveedorRepo.updateProveedor(reg));
            return Ok(mensaje);
        }
    }
}
