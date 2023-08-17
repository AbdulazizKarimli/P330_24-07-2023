using Microsoft.AspNetCore.Authorization;
using P330Pronia.Utils.Enums;

namespace P330Pronia.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin, Moderator")]
public class DashboardController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
