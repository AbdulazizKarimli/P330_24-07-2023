using P330Pronia.Models.Common;

namespace P330Pronia.Models;

public class Tag : BaseSectionEntity
{
    public string Name { get; set; }
    public ICollection<ProductTag> ProductTags { get; set; }
}