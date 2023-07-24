using P330Pronia.Models;

namespace P330Pronia.ViewModels;

public class HomeViewModel
{
    public IEnumerable<Slider> Sliders { get; set; }
    public IEnumerable<Feature> Features { get; set; }
}