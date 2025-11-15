
using CoffeStore.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoffeStore.ViewComponents;

public class Slider : ViewComponent
{
    private readonly DataContext _context;

    public Slider(DataContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        return View(_context.Sliders.Where(i => i.Aktif).OrderBy(i => i.Id).ToList());
    }
}