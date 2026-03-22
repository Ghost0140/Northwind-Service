using Microsoft.Data.SqlClient;
using NorthwindWebApi.Models;
using NorthwindWebApi.Repositorio.Interfaces;
using System.Data;

namespace NorthwindWebApi.Repositorio.DAO
{
    public class homeDAO : IHome
    {
        private readonly string cadena;
        public homeDAO()
        {
            cadena = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("sql");
        }
        public Home getTotales()
        {
            Home totales = new Home();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("USP_Adicional_ObtenerTotales", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    totales.TotalProductos = dr.GetInt32(0);
                    totales.TotalClientes = dr.GetInt32(1);
                    totales.TotalTransportistas = dr.GetInt32(2);
                    totales.TotalProveedores = dr.GetInt32(3);
                }
                dr.Close();
            }
            return totales;
        }
    }
}
