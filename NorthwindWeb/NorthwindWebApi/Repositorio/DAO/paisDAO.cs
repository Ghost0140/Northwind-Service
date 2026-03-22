using Microsoft.Data.SqlClient;
using NorthwindWebApi.Models;
using NorthwindWebApi.Repositorio.Interfaces;
using System.Data;

namespace NorthwindWebApi.Repositorio.DAO
{
    public class paisDAO : IPais
    {
        private readonly string cadena;
        public paisDAO()
        {
            cadena = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("sql");
        }
        public Pais getPais(int id)
        {
            Pais pais = null;
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("USP_Paises_ListarPorId", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PaisID", id);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    pais = new Pais()
                    {
                        PaisID = dr.GetInt32(0),
                        NombrePais = dr.GetString(1)
                    };
                }
                dr.Close();
            }
            return pais;
        }
        public IEnumerable<Pais> getPaises()
        {
            List<Pais> temporal = new List<Pais>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("USP_Paises_ListarTodo", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    temporal.Add(new Pais()
                    {
                        PaisID = dr.GetInt32(0),
                        NombrePais = dr.GetString(1)
                    });
                }
                dr.Close();
            }
            return temporal;
        }
    }
}
