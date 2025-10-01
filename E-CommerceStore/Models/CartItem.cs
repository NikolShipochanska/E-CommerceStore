namespace E_CommerceStore.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int shoppingCartId { get; set; } //Foreign key to ShoppingCart (1 ShoppingCart - Many CartItems)
        public ShoppingCart ShoppingCart { get; set; }
        public int ProductId { get; set; } //Foreign key to Product (1 Product - Many CartItems)
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public CartItem() { } //essential for EF
    }
}
