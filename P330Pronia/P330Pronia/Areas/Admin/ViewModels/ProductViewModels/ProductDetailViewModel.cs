namespace P330Pronia.Areas.Admin.ViewModels.ProductViewModels;

public class ProductDetailViewModel
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Image { get; set; }
    public string Description { get; set; }
    public int Rating { get; set; }
    public string CategoryName { get; set; }
    public List<string> TagNames { get; set; }
    public DateTime CreatedDate { get; set; }
    public string CreatedBy { get; set; }
    public DateTime UpdatedDate { get; set; }
    public string UpdatedBy { get; set; }
}