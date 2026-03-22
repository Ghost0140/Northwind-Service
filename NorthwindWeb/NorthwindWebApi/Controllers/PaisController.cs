using Microsoft.AspNetCore.Mvc;
using NorthwindWebApi.Models;
using NorthwindWebApi.Repositorio.Interfaces;

namespace NorthwindWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaisController : ControllerBase
    {
        private readonly IPais _paisRepo;
        public PaisController(IPais paisRepo)
        {
            _paisRepo = paisRepo;
        }

        [HttpGet("getPaises")]
        public async Task<ActionResult<IEnumerable<Pais>>> getPaises()
        {
            var lista = await Task.Run(() => _paisRepo.getPaises());
            return Ok(lista);
        }

        [HttpGet("getPais/{id}")]
        public async Task<ActionResult<Supplier>> getPais(int id)
        {
            var pais = await Task.Run(() => _paisRepo.getPais(id));
            if (pais == null) return NotFound();
            return Ok(pais);
        }
    }
}
