namespace E_CommerceStore.Models
{
    public class ShoppingCart
    { 
        public int Id { get; set; }
        public string? UserId { get; set; } //FK to User (1 User - 1 Shopping Cart)
        public User? User { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public ShoppingCart() { }
    }
}
