using NorthwindWebApi.Models;

namespace NorthwindWebApi.Repositorio.Interfaces
{
    public interface IShipper
    {
        IEnumerable<Shipper> getTransportistas(string? nombre);
        Shipper getTransportista(int id);
        string insertTransportista(Shipper reg);
        string updateTransportista(Shipper reg);
    }
}
