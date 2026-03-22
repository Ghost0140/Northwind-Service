using System.ComponentModel.DataAnnotations;

namespace NorthwindWebMvc.Models
{
    public class Product
    {
        [Required(ErrorMessage = "El ID del producto es obligatorio")]
        [Display(Name = "Id Producto")]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "El nombre del producto es obligatorio")]
        [StringLength(40, ErrorMessage = "El nombre del producto no puede exceder los 40 caracteres.")]
        [Display(Name = "Nombre del Producto")]
        public string ProductName { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un proveedor")]
        [Display(Name = "Proveedor")]
        public int? SupplierID { get; set; }

        public string? SupplierName { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una categoría")]
        [Display(Name = "Categoría")]
        public int? CategoryID { get; set; }

        public string? CategoryName { get; set; }

        [Required(ErrorMessage = "La cantidad por unidad es obligatoria")]
        [StringLength(40, ErrorMessage = "La cantidad por unidad no puede exceder los 20 caracteres.")]
        [Display(Name = "Cantidad por Unidad")]
        public string? QuantityPerUnit { get; set; }

        [Required(ErrorMessage = "El precio unitario es obligatorio")]
        [Display(Name = "Precio Unitario")]
        public decimal? UnitPrice { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio")]
        [Display(Name = "Stock")]
        public short? UnitsInStock { get; set; }

        [Required(ErrorMessage = "Las unidades en pedido son obligatorias")]
        [Display(Name = "Unidades en Pedido")]
        public short? UnitsOnOrder { get; set; }

        [Required(ErrorMessage = "El nivel de reorden es obligatorio")]
        [Display(Name = "Nivel de Reorden")]
        public short? ReorderLevel { get; set; }

        // Propiedades de solo lectura
        public string SupplierDisplay =>
            string.IsNullOrWhiteSpace(SupplierName) ? "No especificado" : SupplierName;

        public string CategoryDisplay => 
            string.IsNullOrWhiteSpace(CategoryName) ? "No especificado" : CategoryName;

        public string QuantityPerUnitDisplay =>
            string.IsNullOrWhiteSpace(QuantityPerUnit) ? "No especificado" : QuantityPerUnit;

        public string UnitPriceDisplay =>
            UnitPrice.HasValue ? UnitPrice.Value.ToString("F2") : "No especificado";

        public string UnitsInStockDisplay =>
            UnitsInStock.HasValue ? UnitsInStock.Value.ToString() : "No especificado";

        public string UnitsOnOrderDisplay =>
            UnitsOnOrder.HasValue ? UnitsOnOrder.Value.ToString() : "No especificado";

        public string ReorderLevelDisplay =>
            ReorderLevel.HasValue ? ReorderLevel.Value.ToString() : "No especificado";
    }
}
