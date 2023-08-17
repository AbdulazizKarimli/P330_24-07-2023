using P330Pronia.ViewModels;

namespace P330Pronia.Controllers;

public class HomeController : Controller
{
    private readonly AppDbContext _context;

    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var sliders = _context.Sliders;
        var features = _context.Features;

        HomeViewModel homeViewModel = new HomeViewModel
        {
            Sliders = sliders,
            Features = features
        };

        HttpContext.Session.SetString("name", "Togrul");
        HttpContext.Response.Cookies.Append("surname", "Memmedov", new CookieOptions
        {
            MaxAge = TimeSpan.FromMinutes(1)
        });

        return View(homeViewModel);
    }

    public IActionResult Test()
    {
        string name = HttpContext.Session.GetString("name");
        string surname = HttpContext.Request.Cookies["surname"];
        return Content(surname);
    }
}