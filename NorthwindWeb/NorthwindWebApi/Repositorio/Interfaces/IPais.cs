using NorthwindWebApi.Models;

namespace NorthwindWebApi.Repositorio.Interfaces
{
    public interface IPais
    {
        IEnumerable<Pais> getPaises();
        Pais getPais(int id);
    }
}
