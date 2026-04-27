using System.ComponentModel.DataAnnotations;

namespace NorthwindWebMvc.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }

        [Required(ErrorMessage = "Ingrese el nombre de usuario")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "Ingrese la clave")]
        public string Clave { get; set; }

        [Required(ErrorMessage = "Seleccione el rol")]
        public string Rol { get; set; }

        public bool Estado { get; set; }
    }
}