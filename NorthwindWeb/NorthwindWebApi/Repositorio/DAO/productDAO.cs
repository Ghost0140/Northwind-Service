using Microsoft.Data.SqlClient;
using NorthwindWebApi.Models;
using NorthwindWebApi.Repositorio.Interfaces;
using System.Data;

namespace NorthwindWebApi.Repositorio.DAO
{
    public class productDAO : IProduct
    {
        private readonly string cadena;
        public productDAO()
        {
            cadena = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("sql");
        }
        public Product getProducto(int id)
        {
            Product producto = null;
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("USP_Productos_ListarPorId", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductID", id);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    producto = new Product()
                    {
                        ProductID = dr.GetInt32(0),
                        ProductName = dr.GetString(1),
                        SupplierID = dr.IsDBNull(2) ? null : dr.GetInt32(2),
                        SupplierName = dr.IsDBNull(3) ? null : dr.GetString(3),
                        CategoryID = dr.IsDBNull(4) ? null : dr.GetInt32(4),
                        CategoryName = dr.IsDBNull(5) ? null : dr.GetString(5),
                        QuantityPerUnit = dr.IsDBNull(6) ? null : dr.GetString(6),
                        UnitPrice = dr.IsDBNull(7) ? null : dr.GetDecimal(7),
                        UnitsInStock = dr.IsDBNull(8) ? null : dr.GetInt16(8),
                        UnitsOnOrder = dr.IsDBNull(9) ? null : dr.GetInt16(9),
                        ReorderLevel = dr.IsDBNull(10) ? null : dr.GetInt16(10)
                    };
                }
                dr.Close();
            }
            return producto;
        }

        public IEnumerable<Product> getProductos(string producto = "")
        {
            List<Product> temporal = new List<Product>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("USP_Productos_ListarTodo", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NombreProducto", string.IsNullOrEmpty(producto) ? DBNull.Value : (object)producto);

                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    temporal.Add(new Product()
                    {
                        ProductID = dr.GetInt32(0),
                        ProductName = dr.GetString(1),
                        SupplierID = dr.IsDBNull(2) ? null : dr.GetInt32(2),
                        SupplierName = dr.IsDBNull(3) ? null : dr.GetString(3),
                        CategoryID = dr.IsDBNull(4) ? null : dr.GetInt32(4),
                        CategoryName = dr.IsDBNull(5) ? null : dr.GetString(5),
                        QuantityPerUnit = dr.IsDBNull(6) ? null : dr.GetString(6),
                        UnitPrice = dr.IsDBNull(7) ? null : dr.GetDecimal(7),
                        UnitsInStock = dr.IsDBNull(8) ? null : dr.GetInt16(8),
                        UnitsOnOrder = dr.IsDBNull(9) ? null : dr.GetInt16(9),
                        ReorderLevel = dr.IsDBNull(10) ? null : dr.GetInt16(10)
                    });
                }
                dr.Close();
            }
            return temporal;
        }

        public string insertProducto(Product reg)
        {
            string mensaje = string.Empty;
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("USP_Productos_Insertar", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProductName", reg.ProductName);
                    cmd.Parameters.AddWithValue("@SupplierID", reg.SupplierID);
                    cmd.Parameters.AddWithValue("@CategoryID", reg.CategoryID);
                    cmd.Parameters.AddWithValue("@QuantityPerUnit", reg.QuantityPerUnit);
                    cmd.Parameters.AddWithValue("@UnitPrice", reg.UnitPrice);
                    cmd.Parameters.AddWithValue("@UnitsInStock", reg.UnitsInStock);
                    cmd.Parameters.AddWithValue("@UnitsOnOrder", reg.UnitsOnOrder);
                    cmd.Parameters.AddWithValue("@ReorderLevel", reg.ReorderLevel);
                    cn.Open();
                    int i = cmd.ExecuteNonQuery();
                    mensaje = $"Se ha agregado {i} producto";
                }
                catch (SqlException ex)
                {
                    mensaje = ex.Message;
                }
                finally { cn.Close(); }
                return mensaje;
            }
        }

        public string updateProducto(Product reg)
        {
            string mensaje = string.Empty;
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("USP_Productos_Actualizar", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProductID", reg.ProductID);
                    cmd.Parameters.AddWithValue("@ProductName", reg.ProductName);
                    cmd.Parameters.AddWithValue("@SupplierID", reg.SupplierID);
                    cmd.Parameters.AddWithValue("@CategoryID", reg.CategoryID);
                    cmd.Parameters.AddWithValue("@QuantityPerUnit", reg.QuantityPerUnit);
                    cmd.Parameters.AddWithValue("@UnitPrice", reg.UnitPrice);
                    cmd.Parameters.AddWithValue("@UnitsInStock", reg.UnitsInStock);
                    cmd.Parameters.AddWithValue("@UnitsOnOrder", reg.UnitsOnOrder);
                    cmd.Parameters.AddWithValue("@ReorderLevel", reg.ReorderLevel);
                    cn.Open();
                    int i = cmd.ExecuteNonQuery();
                    mensaje = $"Se ha actualizado {i} producto";
                }
                catch (SqlException ex)
                {
                    mensaje = ex.Message;
                }
                finally { cn.Close(); }
                return mensaje;
            }
        }
        public string deleteProducto(int id)
        {
            string mensaje = string.Empty;
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("USP_Productos_Eliminar", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProductID", id);
                    cn.Open();
                    int i = cmd.ExecuteNonQuery();
                    mensaje = $"Se ha eliminado {i} producto";
                }
                catch (SqlException ex)
                {
                    mensaje = ex.Message;
                }
                finally { cn.Close(); }
                return mensaje;
            }
        }
    }
}
