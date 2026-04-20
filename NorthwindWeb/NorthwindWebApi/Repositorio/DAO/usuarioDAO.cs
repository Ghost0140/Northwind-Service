using Microsoft.Data.SqlClient;
using NorthwindWebApi.Models;
using NorthwindWebApi.Repositorio.Interfaces;

namespace NorthwindWebApi.Repositorio.DAO
{
    public class UsuarioDAO : IUsuarioDAO
    {
        private readonly IConfiguration _configuration;

        public UsuarioDAO(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public LoginResponse ValidarLogin(LoginRequest request)
        {
            LoginResponse response = new LoginResponse
            {
                Success = false,
                Mensaje = "Usuario o contraseña incorrectos"
            };

            string cadena = _configuration.GetConnectionString("sql");

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();

                string sql = @"SELECT NombreUsuario, Rol
                               FROM Usuarios
                               WHERE NombreUsuario = @NombreUsuario
                               AND Clave = @Clave
                               AND Estado = 1";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@NombreUsuario", request.NombreUsuario);
                    cmd.Parameters.AddWithValue("@Clave", request.Clave);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            response.Success = true;
                            response.Mensaje = "Login correcto";
                            response.NombreUsuario = dr["NombreUsuario"].ToString();
                            response.Rol = dr["Rol"].ToString();
                        }
                    }
                }
            }

            return response;
        }

        public List<Usuario> ListarUsuarios()
        {
            List<Usuario> lista = new List<Usuario>();
            string cadena = _configuration.GetConnectionString("sql");

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();

                string sql = @"SELECT IdUsuario, NombreUsuario, Clave, Rol, Estado
                               FROM Usuarios
                               ORDER BY IdUsuario DESC";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        lista.Add(new Usuario
                        {
                            IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                            NombreUsuario = dr["NombreUsuario"].ToString(),
                            Clave = dr["Clave"].ToString(),
                            Rol = dr["Rol"].ToString(),
                            Estado = Convert.ToBoolean(dr["Estado"])
                        });
                    }
                }
            }

            return lista;
        }

        public string RegistrarUsuario(Usuario usuario)
        {
            string mensaje = "Error al registrar usuario";
            string cadena = _configuration.GetConnectionString("sql");

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();

                string sql = @"INSERT INTO Usuarios (NombreUsuario, Clave, Rol, Estado)
                               VALUES (@NombreUsuario, @Clave, @Rol, @Estado)";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
                    cmd.Parameters.AddWithValue("@Clave", usuario.Clave);
                    cmd.Parameters.AddWithValue("@Rol", usuario.Rol);
                    cmd.Parameters.AddWithValue("@Estado", usuario.Estado);

                    int filas = cmd.ExecuteNonQuery();

                    if (filas > 0)
                        mensaje = "Usuario registrado correctamente";
                }
            }

            return mensaje;
        }
    }
}