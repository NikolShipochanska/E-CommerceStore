using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceStore.Models

{
    public class User : IdentityUser
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public List<Order> Orders { get; set; } 
        public ShoppingCart ShoppingCart { get; set; }
        public User() { } //essential for EF
        public User(string firstName, string lastName) //constructor for easy creation of user objects
        {
            FirstName = firstName;
            LastName = lastName;
        }
        public string GetFullName()
        {
            return FirstName + " " + LastName;
        }
    }
}
