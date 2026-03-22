using Microsoft.AspNetCore.Mvc;
using NorthwindWebApi.Models;
using NorthwindWebApi.Repositorio.Interfaces;

namespace NorthwindWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShipperController : ControllerBase
    {
        private readonly IShipper _transportistaRepo;
        public ShipperController(IShipper transportistaRepo)
        {
            _transportistaRepo = transportistaRepo;
        }
        [HttpGet("getTransportistas")]
        public async Task<ActionResult<IEnumerable<Shipper>>> getTransportistas(string? nombre)
        {
            var lista = await Task.Run(() => _transportistaRepo.getTransportistas(nombre));
            return Ok(lista);
        }

        [HttpGet("getTransportista/{id}")]
        public async Task<ActionResult<Shipper>> getTransportista(int id)
        {
            var transportista = await Task.Run(() => _transportistaRepo.getTransportista(id));
            if (transportista == null) return NotFound();
            return Ok(transportista);
        }

        [HttpPost("insertTransportista")]
        public async Task<ActionResult<string>> insertarTransportista(Shipper reg)
        {
            var mensaje = await Task.Run(() => _transportistaRepo.insertTransportista(reg));
            return Ok(mensaje);
        }

        [HttpPut("updateTransportista")]
        public async Task<ActionResult<string>> actualizarTransportista(Shipper reg)
        {
            var mensaje = await Task.Run(() => _transportistaRepo.updateTransportista(reg));
            return Ok(mensaje);
        }
    }
}
