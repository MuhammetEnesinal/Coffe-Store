
using CoffeStore.Models;
using Microsoft.AspNetCore.Identity;

namespace CoffeStore.Models;


public class AppUser : IdentityUser<int>
{
    public string AdSoyad  { get; set; } = null!;

}