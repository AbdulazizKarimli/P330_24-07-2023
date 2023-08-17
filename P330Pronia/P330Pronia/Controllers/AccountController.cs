using AutoMapper;
using Microsoft.AspNetCore.Identity;
using P330Pronia.Models.Identity;
using P330Pronia.Utils.Enums;
using P330Pronia.ViewModels;

namespace P330Pronia.Controllers;

public class AccountController : Controller
{
    private readonly IMapper _mapper;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AccountController(IMapper mapper, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _mapper = mapper;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public IActionResult Register()
    {
        if (User.Identity.IsAuthenticated)
            return BadRequest();

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
    {
        if (User.Identity.IsAuthenticated)
            return BadRequest();

        if (!ModelState.IsValid)
            return View();

        AppUser newUser = _mapper.Map<AppUser>(registerViewModel);
        newUser.IsActive = true;

        IdentityResult identityResult = await _userManager.CreateAsync(newUser, registerViewModel.Password);
        if (!identityResult.Succeeded)
        {
            foreach (var error in identityResult.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View();
        }

        await _userManager.AddToRoleAsync(newUser, Roles.Member.ToString());

        return RedirectToAction("Login", "Auth");
    }

    //public async Task<IActionResult> CreateRoles()
    //{
    //    foreach (var role in Enum.GetValues(typeof(Roles)))
    //    {
    //        await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
    //    }
    //    return Content("OK");
    //}
}