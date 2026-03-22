using NorthwindWebApi.Models;

namespace NorthwindWebApi.Repositorio.Interfaces
{
    public interface IProduct
    {
        IEnumerable<Product> getProductos(string producto = "");
        Product getProducto(int id);
        string insertProducto(Product reg);
        string updateProducto(Product reg);
        string deleteProducto(int id);
    }
}
