using Microsoft.Data.SqlClient;
using NorthwindWebApi.Models;
using NorthwindWebApi.Repositorio.Interfaces;
using System.Data;

namespace NorthwindWebApi.Repositorio.DAO
{
    public class shipperDAO : IShipper
    {
        private readonly string cadena;
        public shipperDAO()
        {
            cadena = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("sql");
        }
        public Shipper getTransportista(int id)
        {
            Shipper transportista = null;
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("USP_Transportistas_ListarPorId", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ShipperID", id);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    transportista = new Shipper()
                    {
                        ShipperID = dr.GetInt32(0),
                        CompanyName = dr.GetString(1),
                        Phone = dr.IsDBNull(2) ? null : dr.GetString(2)
                    };
                }
                dr.Close();
            }
            return transportista;
        }
        public IEnumerable<Shipper> getTransportistas(string? nombre)
        {
            List<Shipper> temporal = new List<Shipper>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("USP_Transportistas_ListarTodo", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompanyName", string.IsNullOrWhiteSpace(nombre) ? DBNull.Value : nombre);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    temporal.Add(new Shipper()
                    {
                        ShipperID = dr.GetInt32(0),
                        CompanyName = dr.GetString(1),
                        Phone = dr.IsDBNull(2) ? null : dr.GetString(2)
                    });
                }
                dr.Close();
            }
            return temporal;
        }
        public string insertTransportista(Shipper reg)
        {
            string mensaje = string.Empty;
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("USP_Transportistas_Insertar", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CompanyName", reg.CompanyName);
                    cmd.Parameters.AddWithValue("@Phone", reg.Phone);
                    cn.Open();
                    int i = cmd.ExecuteNonQuery();
                    mensaje = $"Se ha agregado {i} transportista";
                }
                catch (SqlException ex)
                {
                    mensaje = ex.Message;
                }
                finally { cn.Close(); }
                return mensaje;
            }
        }
        public string updateTransportista(Shipper reg)
        {
            string mensaje = string.Empty;
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("USP_Transportistas_Actualizar", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ShipperID", reg.ShipperID);
                    cmd.Parameters.AddWithValue("@CompanyName", reg.CompanyName);
                    cmd.Parameters.AddWithValue("@Phone", reg.Phone);
                    cn.Open();
                    int i = cmd.ExecuteNonQuery();
                    mensaje = $"Se ha actualizado {i} transportista";
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
