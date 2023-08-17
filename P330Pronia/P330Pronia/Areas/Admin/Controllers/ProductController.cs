using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using P330Pronia.Areas.Admin.ViewModels.ProductViewModels;
using P330Pronia.Exceptions;
using P330Pronia.Models;
using P330Pronia.Services.Interfaces;

namespace P330Pronia.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin, Moderator")]
public class ProductController : Controller
{
    private readonly AppDbContext _context;
    private readonly IFileService _fileService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IMapper _mapper;

    public ProductController(AppDbContext context, IFileService fileService, IWebHostEnvironment webHostEnvironment, IMapper mapper)
    {
        _context = context;
        _fileService = fileService;
        _webHostEnvironment = webHostEnvironment;
        _mapper = mapper;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var products = await _context.Products.OrderByDescending(p => p.UpdatedDate).AsNoTracking().ToListAsync();

        return View(products);
    }

    public IActionResult Create()
    {
        ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateProductViewModel createProductViewModel)
    {
        ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");

        if (!ModelState.IsValid)
            return View();

        //Product product = new Product
        //{
        //    Name = createProductViewModel.Name,
        //    Description = createProductViewModel.Description,
        //    Price = createProductViewModel.Price,
        //    Rating = createProductViewModel.Rating,
        //    CategoryId = createProductViewModel.CategoryId
        //};

        var product = _mapper.Map<Product>(createProductViewModel);

        try
        {
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "website-images");
            product.Image = await _fileService.CreateFileAsync(createProductViewModel.Image, path, maxFileSize: 100, fileType: "image/");
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

        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int id)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product is null)
            return NotFound();

        ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");

        var updateProductViewModel = _mapper.Map<UpdateProductViewModel>(product);

        return View(updateProductViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, UpdateProductViewModel updateProductViewModel)
    {
        ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");

        if (!ModelState.IsValid)
            return View();

        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product is null)
            return NotFound();

        string fileName = product.Image;
        if (updateProductViewModel.Image is not null)
        {
            try
            {
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", "website-images");
                _fileService.DeleteFile(Path.Combine(path, product.Image));

                fileName = await _fileService.CreateFileAsync(updateProductViewModel.Image, path, 100, "image/");
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

        _mapper.Map(updateProductViewModel, product);
        product.Image = fileName;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Detail(int id)
    {
        Product? product = await _context.Products.Include(p => p.Category)
            .Include(p => p.ProductTags).ThenInclude(pt => pt.Tag).FirstOrDefaultAsync(p => p.Id == id);
        if (product is null)
            return NotFound();

        //ProductDetailViewModel productDetailViewModel = new ProductDetailViewModel
        //{
        //    Name = product.Name,
        //    Description = product.Description,
        //    Price = product.Price,
        //    Rating = product.Rating,
        //    Image = product.Image,
        //    CategoryName = product.Category.Name,
        //    CreatedDate = product.CreatedDate,
        //    CreatedBy = product.CreatedBy,
        //    UpdatedDate = product.UpdatedDate,
        //    UpdatedBy = product.UpdatedBy
        //};

        ProductDetailViewModel productDetailViewModel = _mapper.Map<ProductDetailViewModel>(product);

        return View(productDetailViewModel);
    }
}