using NorthwindWebApi.Models;

namespace NorthwindWebApi.Repositorio.Interfaces
{
    public interface ICargo
    {
        IEnumerable<Cargo> getCargos();
        Cargo getCargo(int id);
    }
}
