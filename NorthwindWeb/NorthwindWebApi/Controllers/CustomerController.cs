using Microsoft.AspNetCore.Mvc;
using NorthwindWebApi.Models;
using NorthwindWebApi.Repositorio.Interfaces;

namespace NorthwindWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomer _clienteRepo;
        public CustomerController(ICustomer customerRepo)
        {
            _clienteRepo = customerRepo;
        }

        [HttpGet("getClientes")]
        public async Task<ActionResult<IEnumerable<Customer>>> getClientes(string? nombre)
        {
            var lista = await Task.Run(() => _clienteRepo.getClientes(nombre));
            return Ok(lista);
        }

        [HttpGet("getCliente/{id}")]
        public async Task<ActionResult<Customer>> getCliente(string id)
        {
            var customer = await Task.Run(() => _clienteRepo.getCliente(id));
            if (customer == null) return NotFound();
            return Ok(customer);
        }

        [HttpPost("insertCliente")]
        public async Task<ActionResult<string>> insertarCliente(Customer reg)
        {
            var mensaje = await Task.Run(() => _clienteRepo.insertCliente(reg));
            return Ok(mensaje);
        }

        [HttpPut("updateCliente")]
        public async Task<ActionResult<string>> updateCliente(Customer reg)
        {
            var mensaje = await Task.Run(() => _clienteRepo.updateCliente(reg));
            return Ok(mensaje);
        }
    }
}
