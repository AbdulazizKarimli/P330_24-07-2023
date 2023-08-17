using Microsoft.EntityFrameworkCore;

namespace P330Pronia.ViewComponents;

public class HeaderViewComponent : ViewComponent
{
    private readonly AppDbContext _context;

    public HeaderViewComponent(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var settings = await _context.Settings.ToDictionaryAsync(s => s.Key, s => s.Value);

        return View(settings);
    }
}