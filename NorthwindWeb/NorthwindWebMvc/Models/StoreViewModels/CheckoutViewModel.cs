namespace NorthwindWebMvc.Models.StoreViewModels
{
    public class CheckoutViewModel
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal Total => Items.Sum(x => x.SubTotal);
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string ShippingAddress { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
    }
}
