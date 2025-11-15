
using System.Threading.Tasks;
using CoffeStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoffeStore.Controllers;



public class CartController : Controller
{

    private readonly DataContext _context;
    public CartController(DataContext context)
    {
        _context = context;
    }
    public async Task<ActionResult> AddToCart(int urunId, int miktar = 1)
    {
        var customerId = User.Identity?.Name; 

        var cart=await _context.Carts.Include(c => c.CartItems).Where(c => c.CustumerId.ToString() == customerId)
            .FirstOrDefaultAsync();

        if (cart == null)
        {
            cart = new Cart
            {
                CustumerId = int.Parse(customerId!)
            };
            _context.Carts.Add(cart);
        }
        var cartItem = cart.CartItems.FirstOrDefault(ci => ci.UrunId == urunId);
        if (cartItem != null)
        {
            cartItem.Miktar += 1;
        }
        else
        {
            cartItem = new CartItem
            {
                UrunId = urunId,
                Miktar = miktar,
            };
            cart.CartItems.Add(cartItem);
        }
        await _context.SaveChangesAsync();

        return View();
    }


}