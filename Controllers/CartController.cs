
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

    public async Task<ActionResult> Index()
    {

       var cart=await GetOrCreateCart();

        return View();
    }   
    public async Task<ActionResult> AddToCart(int urunId, int miktar = 1)
    {

       var cart=await GetOrCreateCart();
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


    private async Task<Cart> GetOrCreateCart()
    {
        var CustomerId = User.Identity?.Name; 
        var cart = await _context.Carts
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Urun)
            .FirstOrDefaultAsync(c => c.CustomerId == CustomerId);  
        if (cart == null)
        {
            cart = new Cart
            {
                CustomerId = CustomerId,
            };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        return cart;
    }


}