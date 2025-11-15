using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CoffeStore.Models;

namespace CoffeStore.Controllers;

public class HomeController : Controller
{
    public ActionResult Index()
    {
        return View();
    }
}
