using System.ComponentModel.DataAnnotations;

namespace NorthwindWebMvc.Models
{
    public class Supplier
    {
        [Required(ErrorMessage = "El ID del proveedor es obligatorio")]
        [Display(Name = "Id Proveedor")]
        public int SupplierID { get; set; }

        [Required(ErrorMessage = "El nombre de la empresa es obligatorio.")]
        [StringLength(40, ErrorMessage = "El nombre de la empresa no puede exceder los 40 caracteres.")]
        [Display(Name = "Nombre de la Empresa")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "El nombre del contacto es obligatorio.")]
        [StringLength(30, ErrorMessage = "El nombre del contacto no puede exceder los 30 caracteres.")]
        [Display(Name = "Nombre del Contacto")]
        public string? ContactName { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un cargo")]
        [Display(Name = "Cargo del Contacto")]
        public int? CargoID { get; set; }

        public string? NombreCargo { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria")]
        [StringLength(60, ErrorMessage = "La dirección no puede exceder los 60 caracteres.")]
        [Display(Name = "Dirección")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "El código postal es obligatorio")]
        [StringLength(10, ErrorMessage = "El código postal no puede exceder los 10 caracteres.")]
        [Display(Name = "Código Postal")]
        public string? PostalCode { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un país")]
        [Display(Name = "País")]
        public int? PaisID { get; set; }

        public string? NombrePais { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [StringLength(24, ErrorMessage = "El teléfono no puede exceder los 24 caracteres.")]
        [Display(Name = "Teléfono")]
        public string? Phone { get; set; }

        // Propiedades de solo lectura
        public string ContactNameDisplay =>
            string.IsNullOrWhiteSpace(ContactName) ? "No especificado" : ContactName;
        public string CargoDisplay =>
            string.IsNullOrWhiteSpace(NombreCargo) ? "No especificado" : NombreCargo;
        public string AddressDisplay =>
            string.IsNullOrWhiteSpace(Address) ? "No especificado" : Address;
        public string PostalCodeDisplay =>
            string.IsNullOrWhiteSpace(PostalCode) ? "No especificado" : PostalCode;
        public string PaisDisplay =>
            string.IsNullOrWhiteSpace(NombrePais) ? "No especificado" : NombrePais;
        public string PhoneDisplay =>
            string.IsNullOrWhiteSpace(Phone) ? "No especificado" : Phone;
    }
}
