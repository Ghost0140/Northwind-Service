using NorthwindWebApi.Models;

namespace NorthwindWebApi.Repositorio.Interfaces
{
    public interface ICustomer
    {
        IEnumerable<Customer> getClientes(string? nombre);
        Customer getCliente(string id);
        string insertCliente(Customer reg);
        string updateCliente(Customer reg);
    }
}
