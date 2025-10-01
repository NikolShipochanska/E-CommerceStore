using System.ComponentModel.DataAnnotations;

namespace E_CommerceStore.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        public int? CategoryId { get; set; } //Foreign key to category (1 Category - Many products)
        public Category? Category { get; set; }
        public int StockQuantity { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
        public Product() { } //essential for EF
        public Product(string name, string? description, decimal price, Category? category, int stockQuantity)
        {
            Name = name;
            Description = description;
            Price = price;
            Category = category;
            CategoryId = category?.Id;
            StockQuantity = stockQuantity;
        }
    }
}
