using System.ComponentModel.DataAnnotations;

namespace NorthwindWebMvc.Models
{
    public class Shipper
    {
        [Required(ErrorMessage = "El ID del transportista es obligatorio")]
        [Display(Name = "Id Transportista")]
        public int ShipperID { get; set; }

        [Required(ErrorMessage = "El nombre de la empresa es obligatorio.")]
        [StringLength(40, ErrorMessage = "El nombre de la empresa no puede exceder los 40 caracteres.")]
        [Display(Name = "Nombre de la Empresa")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "El número de teléfono es obligatorio.")]
        [StringLength(24, ErrorMessage = "El número de teléfono no puede exceder los 24 caracteres.")]
        [Display(Name = "Teléfono")]
        public string? Phone { get; set; }

        // Propiedades de solo lectura
        public string PhoneDisplay =>
            string.IsNullOrWhiteSpace(Phone) ? "No especificado" : Phone;
    }
}
