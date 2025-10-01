namespace E_CommerceStore.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; } //FK to Order (1 Order - Many OrderItems)
        public Order? Order { get; set; }
        public int ProductId { get; set; } //FK to Product (1 Product - Many OrderItems)
        public Product? Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; } //Price at the time of order
        public OrderItem() { } //essential for EF
    }
}
