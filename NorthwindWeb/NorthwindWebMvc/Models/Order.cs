using System.ComponentModel.DataAnnotations;

namespace NorthwindWebMvc.Models
{
    public class Order
    {
        [Required(ErrorMessage = "El ID del pedido es obligatorio")]
        [Display(Name = "Id Pedido")]
        public int OrderID { get; set; }

        [Required(ErrorMessage = "El cliente es obligatorio")]
        [StringLength(5)]
        [Display(Name = "Cliente")]
        public string CustomerID { get; set; }

        [Display(Name = "Cliente")]
        public string? CompanyName { get; set; }

        [Display(Name = "Fecha del Pedido")]
        public string? OrderDate { get; set; }

        [Display(Name = "Fecha Requerida")]
        public string? RequiredDate { get; set; }

        [Display(Name = "Fecha de Envío")]
        public string? ShippedDate { get; set; }

        [Display(Name = "Destinatario")]
        public string? ShipName { get; set; }

        [Display(Name = "Ciudad")]
        public string? ShipCity { get; set; }

        [Display(Name = "País")]
        public string? ShipCountry { get; set; }

        //PROPIEDADES DE SOLO LECTURA

        public string OrderDateDisplay =>
            string.IsNullOrWhiteSpace(OrderDate) ? "No especificado" : OrderDate;

        public string RequiredDateDisplay =>
            string.IsNullOrWhiteSpace(RequiredDate) ? "No especificado" : RequiredDate;

        public string ShippedDateDisplay =>
            string.IsNullOrWhiteSpace(ShippedDate) ? "Pendiente" : ShippedDate;

        public string ShipNameDisplay =>
            string.IsNullOrWhiteSpace(ShipName) ? "No especificado" : ShipName;

        public string ShipCityDisplay =>
            string.IsNullOrWhiteSpace(ShipCity) ? "No especificado" : ShipCity;

        public string ShipCountryDisplay =>
            string.IsNullOrWhiteSpace(ShipCountry) ? "No especificado" : ShipCountry;
    }
}

