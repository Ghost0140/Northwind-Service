using Microsoft.AspNetCore.Mvc;
using NorthwindWebApi.Models;
using NorthwindWebApi.Repositorio.Interfaces;

namespace NorthwindWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CargoController : ControllerBase
    {
        private readonly ICargo _cargoRepo;
        public CargoController(ICargo cargoRepo)
        {
            _cargoRepo = cargoRepo;
        }

        [HttpGet("getCargos")]
        public async Task<ActionResult<IEnumerable<Cargo>>> getCargos()
        {
            var lista = await Task.Run(() => _cargoRepo.getCargos());
            return Ok(lista);
        }

        [HttpGet("getCargo/{id}")]
        public async Task<ActionResult<Cargo>> getCargo(int id)
        {
            var cargo = await Task.Run(() => _cargoRepo.getCargo(id));
            if (cargo == null) return NotFound();
            return Ok(cargo);
        }
    }
}
