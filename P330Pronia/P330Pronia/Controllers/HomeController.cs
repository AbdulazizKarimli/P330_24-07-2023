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

        return View(homeViewModel);
    }
}