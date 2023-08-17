using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P330Pronia.Areas.Admin.ViewModels.ProductViewModels;

public class UpdateProductViewModel
{
    public int Id { get; set; }
    [Required, MaxLength(120)]
    public string Name { get; set; }
    [Column(TypeName = "decimal(6, 2)")]
    public decimal Price { get; set; }
    public IFormFile? Image { get; set; }
    [Required, MaxLength(500)]
    public string Description { get; set; }
    [Required, Range(1, 5)]
    public int Rating { get; set; }
    public int CategoryId { get; set; }
}