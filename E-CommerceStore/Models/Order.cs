namespace E_CommerceStore.Models
{
    public class Order
    {
        public int Id { get; set; }
        public List<OrderItem> items { get; set; }

    }
}
