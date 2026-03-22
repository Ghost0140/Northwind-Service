using Microsoft.Data.SqlClient;
using NorthwindWebApi.Models;
using NorthwindWebApi.Repositorio.Interfaces;
using System.Data;

namespace NorthwindWebApi.Repositorio.DAO
{
    public class customerDAO : ICustomer
    {
        private readonly string cadena;
        public customerDAO()
        {
            cadena = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("sql");
        }
        public Customer getCliente(string id)
        {
            Customer cliente = null;
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("USP_Clientes_ListarPorId", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerID", id);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    cliente = new Customer()
                    {
                        CustomerID = dr.GetString(0),
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
            return cliente;
        }
        public IEnumerable<Customer> getClientes(string? nombre)
        {
            List<Customer> lista = new List<Customer>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("USP_Clientes_ListarTodo", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompanyName", string.IsNullOrWhiteSpace(nombre) ? DBNull.Value : nombre);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new Customer()
                    {
                        CustomerID = dr.GetString(0),
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
            return lista;
        }
        public string insertCliente(Customer reg)
        {
            string mensaje = string.Empty;
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("USP_Clientes_Insertar", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CustomerID", reg.CustomerID);
                    cmd.Parameters.AddWithValue("@CompanyName", reg.CompanyName);
                    cmd.Parameters.AddWithValue("@ContactName", reg.ContactName);
                    cmd.Parameters.AddWithValue("@CargoID", reg.CargoID);
                    cmd.Parameters.AddWithValue("@Address", reg.Address);
                    cmd.Parameters.AddWithValue("@PostalCode", reg.PostalCode);
                    cmd.Parameters.AddWithValue("@PaisID", reg.PaisID);
                    cmd.Parameters.AddWithValue("@Phone", reg.Phone);
                    cn.Open();
                    int i = cmd.ExecuteNonQuery();
                    mensaje = $"Se ha agregado {i} cliente";
                }
                catch (SqlException ex)
                {
                    mensaje = ex.Message;
                }
                finally { cn.Close(); }
            }
            return mensaje;
        }
        public string updateCliente(Customer reg)
        {
            string mensaje = string.Empty;
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("USP_Clientes_Actualizar", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CustomerID", reg.CustomerID);
                    cmd.Parameters.AddWithValue("@CompanyName", reg.CompanyName);
                    cmd.Parameters.AddWithValue("@ContactName", reg.ContactName);
                    cmd.Parameters.AddWithValue("@CargoID", reg.CargoID);
                    cmd.Parameters.AddWithValue("@Address", reg.Address);
                    cmd.Parameters.AddWithValue("@PostalCode", reg.PostalCode);
                    cmd.Parameters.AddWithValue("@PaisID", reg.PaisID);
                    cmd.Parameters.AddWithValue("@Phone", reg.Phone);
                    cn.Open();
                    int i = cmd.ExecuteNonQuery();
                    mensaje = $"Se ha actualizado {i} cliente";
                }
                catch (SqlException ex)
                {
                    mensaje = ex.Message;
                }
                finally { cn.Close(); }
            }
            return mensaje;
        }
    }
}
