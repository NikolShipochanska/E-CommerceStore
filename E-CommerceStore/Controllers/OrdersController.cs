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
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public OrdersController(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> MakeOrder()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var cart = await _context.ShoppingCarts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (cart == null || !cart.Items.Any())
                return RedirectToAction("Index", "ShoppingCarts");


            // Create new order
            var order = new Order
            {
                UserId = user.Id,
                Status = "Pending",
                TotalPrice = cart.Items.Sum(i => i.Product.Price * i.Quantity)
            };

            // Adding products from cart to order
            foreach (var item in cart.Items)
            {
                order.Items.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.Price
                });

                item.Product.StockQuantity -= item.Quantity;

                if (item.Product.StockQuantity < 0)
                {
                    item.Product.StockQuantity = 0; 
                }
            }
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            order.DisplayOrderNumber = await _context.Orders
            .CountAsync(o => o.UserId == user.Id);

            _context.Update(order);
            await _context.SaveChangesAsync();

            // Empty cart after the order
            _context.CartItems.RemoveRange(cart.Items);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Orders");
        }

        // Show all orders of current user
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var orders = await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == user.Id)
                .ToListAsync();

            return View(orders);
        }

        public async Task<IActionResult> Delete()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");
            var orders = await _context.Orders
                .Where(o => o.UserId == user.Id)
                .ToListAsync();
            _context.Orders.RemoveRange(orders);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Orders");
        }
    }
}