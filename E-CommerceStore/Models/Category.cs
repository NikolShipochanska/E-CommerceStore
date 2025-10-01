using System.ComponentModel.DataAnnotations;

namespace E_CommerceStore.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public Category() { } //essential for EF
        public Category(string name) //constructor for easy creation of category objects
        {
            Name = name;
        }
    }
}
