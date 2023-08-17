using AutoMapper;
using Microsoft.EntityFrameworkCore;
using P330Pronia.Models;
using P330Pronia.ViewModels;

namespace P330Pronia.ViewComponents;

public class ProductViewComponent : ViewComponent
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ProductViewComponent(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        List<Product> products = await _context.Products.ToListAsync();

        var productsViewModel = _mapper.Map<List<ProductViewModel>>(products);

        return View(productsViewModel);
    }
}