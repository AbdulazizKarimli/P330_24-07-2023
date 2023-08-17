namespace P330Pronia.ViewComponents;

public class FooterViewComponent : ViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync()
    {
        return View();
    } 
}