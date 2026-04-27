using Grpc.Core;
using Microsoft.Data.SqlClient;
using System.Data;

namespace NorthwindgRPC
{
    public class ProductoService : Products.ProductsBase
    {
        private readonly ILogger<ProductoService> _logger;

        public ProductoService(ILogger<ProductoService> logger)
        {
            _logger = logger;
        }

        string cadena = "server=.;database=Northwind; trusted_Connection=true;" +
            "MultipleActiveResultSets=true; TrustServerCertificate=false; Encrypt=false";

        List<Producto> Lista()
        {
            List<Producto> temporal = new List<Producto>();
            using (SqlConnection con = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("USP_Productos_ListarTodo", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    temporal.Add(new Producto()
                    {
                        ProductID = dr.GetInt32(0),
                        ProductName = dr.GetString(1),
                        SupplierID = dr.IsDBNull(2) ? 0 : dr.GetInt32(2),
                        SupplierName = dr.IsDBNull(3) ? "" : dr.GetString(3),
                        CategoryID = dr.IsDBNull(4) ? 0 : dr.GetInt32(4),
                        CategoryName = dr.IsDBNull(5) ? "" : dr.GetString(5),
                        QuantityPerUnit = dr.IsDBNull(6) ? "" : dr.GetString(6),
                        UnitPrice = dr.IsDBNull(7) ? 0 : Convert.ToDouble(dr.GetDecimal(7)),
                        UnitsInStock = dr.IsDBNull(8) ? 0 : dr.GetInt16(8),
                        UnitsOnOrder = dr.IsDBNull(9) ? 0 : dr.GetInt16(9),
                        ReorderLevel = dr.IsDBNull(10) ? 0 : dr.GetInt16(10)
                    });
                }
            }
            return temporal;
        }
        public override Task<Productos> GetAll(Empty request, ServerCallContext context)
        {
            Productos response = new Productos();
            response.Items.AddRange(Lista());
            return Task.FromResult(response);
        }
        public override Task<Productos> GetBySupplier(SupplierFilter request, ServerCallContext context)
        {
            Productos response = new Productos();
            var filtrados = Lista().Where(p => p.SupplierName.Contains(request.SupplierName, StringComparison.OrdinalIgnoreCase));
            response.Items.AddRange(filtrados);
            return Task.FromResult(response);
        }
        public override Task<Productos> GetByCategory(CategoryFilter request, ServerCallContext context)
        {
            Productos response = new Productos();
            var filtrados = Lista().Where(c => c.CategoryName.Contains(request.CategoryName, StringComparison.OrdinalIgnoreCase));
            response.Items.AddRange(filtrados);
            return Task.FromResult(response);
        }
    }
}
