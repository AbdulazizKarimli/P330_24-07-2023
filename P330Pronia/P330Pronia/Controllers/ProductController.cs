using Microsoft.AspNetCore.Mvc;

namespace P330Pronia.Controllers;

public class ProductController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}