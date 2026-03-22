using NorthwindWebApi.Models;

namespace NorthwindWebApi.Repositorio.Interfaces
{
    public interface ISupplier
    {
        IEnumerable<Supplier> getProveedores(string? nombre);
        Supplier getProveedor(int id);
        string insertProveedor(Supplier reg);
        string updateProveedor(Supplier reg);
    }
}
