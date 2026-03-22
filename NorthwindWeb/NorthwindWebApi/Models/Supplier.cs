namespace NorthwindWebApi.Models
{
    public class Supplier
    {
        public int SupplierID { get; set; }
        public string CompanyName { get; set; }
        public string? ContactName { get; set; }
        public int? CargoID { get; set; }
        public string? NombreCargo { get; set; }
        public string? Address { get; set; }
        public string? PostalCode { get; set; }
        public int? PaisID { get; set; }
        public string? NombrePais { get; set; }
        public string? Phone { get; set; }
    }
}
