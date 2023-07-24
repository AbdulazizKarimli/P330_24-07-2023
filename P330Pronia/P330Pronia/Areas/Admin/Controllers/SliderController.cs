using Microsoft.EntityFrameworkCore;
using P330Pronia.Models;

namespace P330Pronia.Areas.Admin.Controllers;

[Area("Admin")]
public class SliderController : Controller
{
    private readonly AppDbContext _context;

    public SliderController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var sliders = await _context.Sliders.ToListAsync();

        return View(sliders);
    }

    public async Task<IActionResult> Detail(int id)
    {
        Slider? slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
        if (slider is null)
            return NotFound();

        return View(slider);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Slider slider)
    {
        if (!ModelState.IsValid)
            return View();

        await _context.Sliders.AddAsync(slider);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
        if (slider is null)
            return NotFound();

        return View(slider);
    }

    [HttpPost]
    [ActionName("Delete")]
    public async Task<IActionResult> DeleteSlider(int id)
    {
        var slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
        if (slider is null)
            return NotFound();

        _context.Sliders.Remove(slider);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}