using System.ComponentModel.DataAnnotations;

namespace P330Pronia.Areas.Admin.ViewModels.FeatureViewModels;

public class CreateFeatureViewModel
{
    public IFormFile Image { get; set; }
    [Required, MaxLength(120)]
    public string Title { get; set; }
    [Required, MaxLength(255)]
    public string Description { get; set; }
}