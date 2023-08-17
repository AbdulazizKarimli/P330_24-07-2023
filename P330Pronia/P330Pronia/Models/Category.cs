using P330Pronia.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P330Pronia.Models;

public class Category : BaseSectionEntity
{
    [Required, MaxLength(100)]
    public string Name { get; set; }
    public ICollection<Product>? Products { get; set; }
}