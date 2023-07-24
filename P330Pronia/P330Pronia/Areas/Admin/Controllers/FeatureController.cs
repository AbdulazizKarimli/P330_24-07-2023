using Microsoft.EntityFrameworkCore;
using P330Pronia.Areas.Admin.ViewModels.FeatureViewModels;
using P330Pronia.Models;

namespace P330Pronia.Areas.Admin.Controllers;

[Area("Admin")]
public class FeatureController : Controller
{
    private readonly AppDbContext _context;

    public FeatureController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var features = await _context.Features.ToListAsync();

        return View(features);
    }

    public async Task<IActionResult> Create()
    {
        if (await _context.Features.CountAsync() == 3)
            return BadRequest();

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateFeatureViewModel createFeatureViewModel)
    {
        if (await _context.Features.CountAsync() == 3)
            return BadRequest();

        if (!ModelState.IsValid)
            return View();

        Feature feature = new Feature
        {
            Title = createFeatureViewModel.Title,
            Image = createFeatureViewModel.Image,
            Description = createFeatureViewModel.Description,
        };

        await _context.Features.AddAsync(feature);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int id)
    {
        var feature = await _context.Features.FirstOrDefaultAsync(f => f.Id == id);
        if (feature is null)
            return NotFound();

        UpdateFeatureViewModel updateFeatureViewModel = new UpdateFeatureViewModel
        {
            Id = feature.Id,
            Image = feature.Image,
            Title = feature.Title,
            Description = feature.Description,
        };

        return View(updateFeatureViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, UpdateFeatureViewModel updateFeatureViewModel)
    {
        if (!ModelState.IsValid)
            return View();

        var feature = await _context.Features.FirstOrDefaultAsync(f => f.Id == id);
        if (feature is null)
            return NotFound();

        feature.Image = updateFeatureViewModel.Image;
        feature.Title = updateFeatureViewModel.Title;
        feature.Description = updateFeatureViewModel.Description;

        _context.Features.Update(feature);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        IQueryable<Feature> query = _context.Features.AsQueryable();

        if (await query.CountAsync() == 1)
            return BadRequest();

        var feature = await query.FirstOrDefaultAsync(f => f.Id == id);
        if (feature is null)
            return NotFound();

        DeleteFeatureViewModel deleteFeatureViewModel = new()
        {
            Image = feature.Image,
            Title = feature.Title,
            Description = feature.Description,
        };

        return View(deleteFeatureViewModel);
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteFeature(int id)
    {
        if (await _context.Features.CountAsync() == 1)
            return BadRequest();

        var feature = await _context.Features.FirstOrDefaultAsync(f => f.Id == id);
        if (feature is null)
            return NotFound();

        feature.IsDeleted = true;
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    //public async Task<IActionResult> Test()
    //{
    //    var features = await _context.Features.IgnoreQueryFilters().ToListAsync();

    //    return Json(features);
    //}
}