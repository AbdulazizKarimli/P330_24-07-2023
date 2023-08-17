using P330Pronia.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P330Pronia.Models;

public class Product : BaseSectionEntity
{
    [Required, MaxLength(120)]
    public string Name { get; set; }
    [Column(TypeName = "decimal(6, 2)")]
    public decimal Price { get; set; }
    public string Image { get; set; }
    [Required, MaxLength(500)]
    public string Description { get; set; }
    [Required, Range(1, 5)]
    public int Rating { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public ICollection<ProductTag> ProductTags { get; set; }
}