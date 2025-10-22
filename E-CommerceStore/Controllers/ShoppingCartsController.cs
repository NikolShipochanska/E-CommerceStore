using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using E_CommerceStore.Models;
using Microsoft.AspNetCore.Identity;

namespace E_CommerceStore.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public ShoppingCartsController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Add product to shopping cart
        [HttpPost, ActionName("AddToCart")]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = await _context.ShoppingCarts
               .Include(c => c.Items)
               .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (cart == null)
            {
                cart = new ShoppingCart { UserId = user.Id };
                _context.ShoppingCarts.Add(cart);
                await _context.SaveChangesAsync();
            }

            // Check if we already have this product in the cart
            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                var newItem = new CartItem
                {
                    ProductId = productId,
                    shoppingCartId = cart.Id,
                    Quantity = quantity
                };
                _context.CartItems.Add(newItem);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "ShoppingCarts");

        }

        // View of the shopping cart
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var cart = await _context.ShoppingCarts
                .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            return View(cart);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> RemoveFromCart(int productId, int quantity)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = await _context.ShoppingCarts
               .Include(c => c.Items)
               .ThenInclude(i => i.Product)
               .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (cart == null)
            {
                return RedirectToAction("Index", "ShoppingCarts");
            }

            // Check if we already have this product in the cart
            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null)
            {
                if (quantity >= existingItem.Quantity)
                {
                    _context.CartItems.Remove(existingItem);
                }
                else
                {
                    existingItem.Quantity -= quantity;
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "ShoppingCarts");

        }
    }
}
