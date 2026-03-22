using Microsoft.AspNetCore.Mvc;
using NorthwindWebApi.Models;
using NorthwindWebApi.Repositorio.Interfaces;

namespace NorthwindWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategory _categoryRepo;
        public CategoryController(ICategory categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        [HttpGet("getCategorias")]
        public async Task<ActionResult<IEnumerable<Supplier>>> getCategorias()
        {
            var lista = await Task.Run(() => _categoryRepo.getCategorias());
            return Ok(lista);
        }

        [HttpGet("getCategoria/{id}")]
        public async Task<ActionResult<Supplier>> getCategoria(int id)
        {
            var categoria = await Task.Run(() => _categoryRepo.getCategoria(id));
            if (categoria == null) return NotFound();
            return Ok(categoria);
        }
    }
}
