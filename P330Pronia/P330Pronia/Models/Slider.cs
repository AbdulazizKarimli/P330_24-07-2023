using P330Pronia.Models.Common;
using System.ComponentModel.DataAnnotations;

namespace P330Pronia.Models;

public class Slider : BaseEntity
{
    [Required(ErrorMessage = "Bosh buraxmaq olmaz"), MaxLength(100)]
    public string Suptitle { get; set; }
    [Required, MaxLength(120)]
    public string Title { get; set; }
    [Required, MaxLength(300)]
    public string Description { get; set; }
    public string Image { get; set; }
}