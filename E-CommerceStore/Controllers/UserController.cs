using E_CommerceStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = "User,Admin")]
public class UserController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly AppDbContext _context;

    public UserController(UserManager<User> userManager, AppDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<IActionResult> Profile()
    {
        var user = await _userManager.GetUserAsync(User);
        return View(user);
    }

    public async Task<IActionResult> Cart()
    {
        var user = await _userManager.GetUserAsync(User);
        var cart = await _context.ShoppingCarts
                    .Include(c => c.Items)
                    .ThenInclude(ci => ci.Product)
                    .FirstOrDefaultAsync(c => c.UserId == user.Id);

        return View(cart);
    }
}
