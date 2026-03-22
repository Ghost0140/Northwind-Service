using NorthwindWebApi.Models;

namespace NorthwindWebApi.Repositorio.Interfaces
{
    public interface ICategory
    {
        IEnumerable<Category> getCategorias();
        Category getCategoria(int id);
    }
}
