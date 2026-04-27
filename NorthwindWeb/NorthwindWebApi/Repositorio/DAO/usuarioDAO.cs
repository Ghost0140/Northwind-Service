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
                               ORDER BY IdUsuario ASC";

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

        public string EliminarUsuario(int id)
        {
            string mensaje = "Error al eliminar usuario";
            string cadena = _configuration.GetConnectionString("sql");

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();

                string sql = "UPDATE Usuarios SET Estado = 0 WHERE IdUsuario = @Id";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    int filas = cmd.ExecuteNonQuery();

                    if (filas > 0)
                        mensaje = "Usuario eliminado correctamente";
                }
            }

            return mensaje;
        }

        public Usuario ObtenerUsuario(int id)
        {
            Usuario usuario = new Usuario();
            string cadena = _configuration.GetConnectionString("sql");

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();

                string sql = @"SELECT IdUsuario, NombreUsuario, Clave, Rol, Estado
                       FROM Usuarios WHERE IdUsuario = @Id";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            usuario.IdUsuario = Convert.ToInt32(dr["IdUsuario"]);
                            usuario.NombreUsuario = dr["NombreUsuario"].ToString();
                            usuario.Clave = dr["Clave"].ToString();
                            usuario.Rol = dr["Rol"].ToString();
                            usuario.Estado = Convert.ToBoolean(dr["Estado"]);
                        }
                    }
                }
            }

            return usuario;
        }

        public string ActualizarUsuario(Usuario usuario)
        {
            string mensaje = "Error al actualizar";
            string cadena = _configuration.GetConnectionString("sql");

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                cn.Open();

                string sql = @"UPDATE Usuarios 
                       SET NombreUsuario=@NombreUsuario,
                           Clave=@Clave,
                           Rol=@Rol,
                           Estado=@Estado
                       WHERE IdUsuario=@Id";

                using (SqlCommand cmd = new SqlCommand(sql, cn))
                {
                    cmd.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
                    cmd.Parameters.AddWithValue("@Clave", usuario.Clave);
                    cmd.Parameters.AddWithValue("@Rol", usuario.Rol);
                    cmd.Parameters.AddWithValue("@Estado", usuario.Estado);
                    cmd.Parameters.AddWithValue("@Id", usuario.IdUsuario);

                    int filas = cmd.ExecuteNonQuery();
                    if (filas > 0)
                        mensaje = "Usuario actualizado correctamente";
                }
            }

            return mensaje;
        }
    }
}