using Microsoft.AspNetCore.Mvc;
using NorthwindWebApi.Models;
using NorthwindWebApi.Repositorio.Interfaces;

namespace NorthwindWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHome _homeRepo;
        public HomeController(IHome homeRepo)
        {
            _homeRepo = homeRepo;
        }

        [HttpGet("getTotales")]
        public async Task<ActionResult<Home>> getTotales()
        {
            var datos = await Task.Run(() => _homeRepo.getTotales());
            return Ok(datos);
        }
    }
}
