using Microsoft.AspNetCore.Mvc;
using NorthwindWebApi.Models;
using NorthwindWebApi.Repositorio.Interfaces;

namespace NorthwindWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioDAO _usuarioDAO;

        public UsuarioController(IUsuarioDAO usuarioDAO)
        {
            _usuarioDAO = usuarioDAO;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var response = _usuarioDAO.ValidarLogin(request);
            return Ok(response);
        }

        [HttpGet("listar")]
        public IActionResult Listar()
        {
            var lista = _usuarioDAO.ListarUsuarios();
            return Ok(lista);
        }

        [HttpPost("registrar")]
        public IActionResult Registrar([FromBody] Usuario usuario)
        {
            var mensaje = _usuarioDAO.RegistrarUsuario(usuario);
            return Ok(mensaje);
        }

        [HttpDelete("eliminar/{id}")]
        public IActionResult Eliminar(int id)
        {
            var mensaje = _usuarioDAO.EliminarUsuario(id);
            return Ok(mensaje);
        }

        [HttpGet("{id}")]
        public IActionResult Obtener(int id)
        {
            var usuario = _usuarioDAO.ObtenerUsuario(id);
            return Ok(usuario);
        }

        [HttpPut("actualizar")]
        public IActionResult Actualizar([FromBody] Usuario usuario)
        {
            var mensaje = _usuarioDAO.ActualizarUsuario(usuario);
            return Ok(mensaje);
        }
    }
}