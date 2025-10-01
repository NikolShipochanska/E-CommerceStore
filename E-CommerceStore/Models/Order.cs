namespace E_CommerceStore.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string? UserId { get; set; } //FK to User (1 User - Many Orders)
        public User? User { get; set; }
        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
        public Order() { } //essential for EF

    }
}
