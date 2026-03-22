using Microsoft.Data.SqlClient;
using NorthwindWebApi.Models;
using NorthwindWebApi.Repositorio.Interfaces;
using System.Data;

namespace NorthwindWebApi.Repositorio.DAO
{
    public class cargoDAO : ICargo
    {
        private readonly string cadena;
        public cargoDAO()
        {
            cadena = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("sql");
        }
        public Cargo getCargo(int id)
        {
            Cargo cargo = null;
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("USP_Cargos_ListarPorId", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CargoID", id);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    cargo = new Cargo()
                    {
                        CargoID = dr.GetInt32(0),
                        NombreCargo = dr.GetString(1)
                    };
                }
                dr.Close();
            }
            return cargo;
        }
        public IEnumerable<Cargo> getCargos()
        {
            List<Cargo> temporal = new List<Cargo>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("USP_Cargos_ListarTodo", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    temporal.Add(new Cargo()
                    {
                        CargoID = dr.GetInt32(0),
                        NombreCargo = dr.GetString(1)
                    });
                }
                dr.Close();
            }
            return temporal;
        }
    }
}
