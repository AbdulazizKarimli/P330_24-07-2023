using P330Pronia.Models.Common;

namespace P330Pronia.Models;

public class Setting : BaseEntity
{
    public string Key { get; set; }
    public string Value { get; set; }
}
