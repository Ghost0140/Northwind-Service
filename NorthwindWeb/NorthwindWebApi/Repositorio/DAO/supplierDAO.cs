using Microsoft.Data.SqlClient;
using NorthwindWebApi.Models;
using NorthwindWebApi.Repositorio.Interfaces;
using System.Data;

namespace NorthwindWebApi.Repositorio.DAO
{
    public class supplierDAO : ISupplier
    {
        private readonly string cadena;
        public supplierDAO()
        {
            cadena = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("sql");
        }
        public Supplier getProveedor(int id)
        {
            Supplier proveedor = null;
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("USP_Proveedores_ListarPorId", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SupplierID", id);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    proveedor = new Supplier()
                    {
                        SupplierID = dr.GetInt32(0),
                        CompanyName = dr.GetString(1),
                        ContactName = dr.IsDBNull(2) ? null : dr.GetString(2),
                        CargoID = dr.IsDBNull(3) ? null : dr.GetInt32(3),
                        NombreCargo = dr.IsDBNull(4) ? null : dr.GetString(4),
                        Address = dr.IsDBNull(5) ? null : dr.GetString(5),
                        PostalCode = dr.IsDBNull(6) ? null : dr.GetString(6),
                        PaisID = dr.IsDBNull(7) ? null : dr.GetInt32(7),
                        NombrePais = dr.IsDBNull(8) ? null : dr.GetString(8),
                        Phone = dr.IsDBNull(9) ? null : dr.GetString(9)
                    };
                }
                dr.Close();
            }
            return proveedor;
        }
        public IEnumerable<Supplier> getProveedores(string? nombre)
        {
            List<Supplier> temporal = new List<Supplier>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("USP_Proveedores_ListarTodo", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompanyName", string.IsNullOrWhiteSpace(nombre) ? DBNull.Value : nombre);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    temporal.Add(new Supplier()
                    {
                        SupplierID = dr.GetInt32(0),
                        CompanyName = dr.GetString(1),
                        ContactName = dr.IsDBNull(2) ? null : dr.GetString(2),
                        CargoID = dr.IsDBNull(3) ? null : dr.GetInt32(3),
                        NombreCargo = dr.IsDBNull(4) ? null : dr.GetString(4),
                        Address = dr.IsDBNull(5) ? null : dr.GetString(5),
                        PostalCode = dr.IsDBNull(6) ? null : dr.GetString(6),
                        PaisID = dr.IsDBNull(7) ? null : dr.GetInt32(7),
                        NombrePais = dr.IsDBNull(8) ? null : dr.GetString(8),
                        Phone = dr.IsDBNull(9) ? null : dr.GetString(9)
                    });
                }
                dr.Close();
            }
            return temporal;
        }
        public string insertProveedor(Supplier reg)
        {
            string mensaje = string.Empty;
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("USP_Proveedores_Insertar", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CompanyName", reg.CompanyName);
                    cmd.Parameters.AddWithValue("@ContactName", reg.ContactName);
                    cmd.Parameters.AddWithValue("@CargoID", reg.CargoID);
                    cmd.Parameters.AddWithValue("@Address", reg.Address);
                    cmd.Parameters.AddWithValue("@PostalCode", reg.PostalCode);
                    cmd.Parameters.AddWithValue("@PaisID", reg.PaisID);
                    cmd.Parameters.AddWithValue("@Phone", reg.Phone);
                    cn.Open();
                    int i = cmd.ExecuteNonQuery();
                    mensaje = $"Se ha agregado {i} proveedor";
                }
                catch (SqlException ex)
                {
                    mensaje = ex.Message;
                }
                finally { cn.Close(); }
                return mensaje;
            }
        }
        public string updateProveedor(Supplier reg)
        {
            string mensaje = string.Empty;
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("USP_Proveedores_Actualizar", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SupplierID", reg.SupplierID);
                    cmd.Parameters.AddWithValue("@CompanyName", reg.CompanyName);
                    cmd.Parameters.AddWithValue("@ContactName", reg.ContactName);
                    cmd.Parameters.AddWithValue("@CargoID", reg.CargoID);
                    cmd.Parameters.AddWithValue("@Address", reg.Address);
                    cmd.Parameters.AddWithValue("@PostalCode", reg.PostalCode);
                    cmd.Parameters.AddWithValue("@PaisID", reg.PaisID);
                    cmd.Parameters.AddWithValue("@Phone", reg.Phone);
                    cn.Open();
                    int i = cmd.ExecuteNonQuery();
                    mensaje = $"Se ha actualizado {i} proveedor";
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
