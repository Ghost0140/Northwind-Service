using Microsoft.Data.SqlClient;
using NorthwindWebApi.Models;
using NorthwindWebApi.Repositorio.Interfaces;
using System.Data;

namespace NorthwindWebApi.Repositorio.DAO
{
    public class categoryDAO : ICategory
    {
        private readonly string cadena;
        public categoryDAO()
        {
            cadena = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("sql");
        }
        public Category getCategoria(int id)
        {
            Category categoria = null;
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("USP_Categorias_ListarPorId", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CategoryID", id);
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    categoria = new Category()
                    {
                        CategoryID = dr.GetInt32(0),
                        CategoryName = dr.GetString(1),
                        Description = dr.IsDBNull(2) ? null : dr.GetString(2)
                    };
                }
                dr.Close();
            }
            return categoria;
        }
        public IEnumerable<Category> getCategorias()
        {
            List<Category> temporal = new List<Category>();
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("USP_Categorias_ListarTodo", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    temporal.Add(new Category()
                    {
                        CategoryID = dr.GetInt32(0),
                        CategoryName = dr.GetString(1),
                        Description = dr.IsDBNull(2) ? null : dr.GetString(2)
                    });
                }
                dr.Close();
            }
            return temporal;
        }
    }
}
