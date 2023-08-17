using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using P330Pronia.Areas.Admin.ViewModels.FeatureViewModels;
using P330Pronia.Exceptions;
using P330Pronia.Models;
using P330Pronia.Services.Implementations;
using P330Pronia.Services.Interfaces;
using P330Pronia.Utils;
using P330Pronia.Utils.Enums;
using F = System.IO;

namespace P330Pronia.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin, Moderator")]
public class FeatureController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IFileService _fileService;

    public FeatureController(AppDbContext context, IWebHostEnvironment webHostEnvironment, IFileService fileService)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
        _fileService = fileService;
    }

    public async Task<IActionResult> Index()
    {
        var features = await _context.Features.AsNoTracking().ToListAsync();
        //var test = _context.ChangeTracker.Entries();

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
        //return Content(createFeatureViewModel.Image.FileName);
        //return Content(createFeatureViewModel.Image.ContentType);
        //return Content(createFeatureViewModel.Image.Length.ToString());

        if (await _context.Features.CountAsync() == 3)
            return BadRequest();

        if (!ModelState.IsValid)
            return View();

        string fileName = string.Empty;
        try
        {
            fileName = await _fileService.CreateFileAsync(file: createFeatureViewModel.Image, path: Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "website-images"), maxFileSize: 100, fileType: "image/");
        }
        catch (FileSizeException ex)
        {
            ModelState.AddModelError("Image", ex.Message);
            return View();
        }
        catch (FileTypeException ex)
        {
            ModelState.AddModelError("Image", ex.Message);
            return View();
        }

        Feature feature = new Feature
        {
            Title = createFeatureViewModel.Title,
            Image = fileName,
            Description = createFeatureViewModel.Description,
        };

        //var state1 = _context.Entry(feature).State;//Detached
        await _context.Features.AddAsync(feature);
        //var state2 = _context.Entry(feature).State;//Added
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

        if (updateFeatureViewModel.Image is not null)
        {
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "website-images", feature.Image);
            _fileService.DeleteFile(path);

            try
            {
                string fileName = await _fileService.CreateFileAsync(file: updateFeatureViewModel.Image, path: Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "website-images"), maxFileSize: 100, fileType: "image/");
                feature.Image = fileName;
            }
            catch (FileSizeException ex)
            {
                ModelState.AddModelError("Image", ex.Message);
                return View();
            }
            catch (FileTypeException ex)
            {
                ModelState.AddModelError("Image", ex.Message);
                return View();
            }
        }

        feature.Title = updateFeatureViewModel.Title;
        feature.Description = updateFeatureViewModel.Description;

        var state2 = _context.Entry(feature).State;//Modified
        //_context.Features.Update(feature);
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

        string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "website-images", feature.Image);
        _fileService.DeleteFile(path);

        feature.IsDeleted = true;
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    //public async Task<IActionResult> Test()
    //{
    //    var features = await _context.Features.IgnoreQueryFilters().ToListAsync();

    //    return Json(features);
    //}

    //private async Task<string> CreateFileAsync(IFormFile image, string path)
    //{
    //    string fileName = $"{Guid.NewGuid()}-{image.FileName}";
    //    string resultPath = Path.Combine(path, fileName);
    //    using (FileStream fileStream = new FileStream(resultPath, FileMode.Create))
    //    {
    //        await image.CopyToAsync(fileStream);
    //    }

    //    return fileName;
    //}

    //private void DeleteFile(string path)
    //{
    //    if (F.File.Exists(path))
    //    {
    //        F.File.Delete(path);
    //    }
    //}
}