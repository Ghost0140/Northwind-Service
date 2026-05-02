namespace NorthwindWebMvc.Models.StoreViewModels
{
    public class CartViewModel
    {
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public decimal Total => Items.Sum(x => x.SubTotal);
        public int ItemCount => Items.Sum(x => x.Quantity);
    }
}
