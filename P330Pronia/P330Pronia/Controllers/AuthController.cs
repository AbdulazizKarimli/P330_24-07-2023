using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using P330Pronia.Models.Identity;
using P330Pronia.Services.Interfaces;
using P330Pronia.ViewModels;

namespace P330Pronia.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMailService _mailService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMailService mailService, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mailService = mailService;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return BadRequest();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string? returnUrl)
        {
            if (User.Identity.IsAuthenticated)
                return BadRequest();

            if (!ModelState.IsValid)
                return View();

            //loginViewModel.UsernameOrEmail = muhammadma@code.edu.az
            AppUser appUser = await _userManager.FindByNameAsync(loginViewModel.UsernameOrEmail) ?? await _userManager.FindByEmailAsync(loginViewModel.UsernameOrEmail);
            if (appUser is null)
            {
                ModelState.AddModelError("", "Username/Email or password is incorrect");
                return View();
            }

            var signInResult = await _signInManager.PasswordSignInAsync(appUser, loginViewModel.Password, loginViewModel.RememberMe, true);
            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("", "Get sonra gelersen!!!");
                return View();
            }
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Username/Email or password is incorrect");
                return View();
            }

            if (!appUser.LockoutEnabled)
            {
                appUser.LockoutEnabled = true;
                appUser.LockoutEnd = null;
                await _userManager.UpdateAsync(appUser);
            }

            if(returnUrl is not null)
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            if (!User.Identity.IsAuthenticated)
                return BadRequest();

            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordViewModel)
        {
            if (!ModelState.IsValid)
                return View();

            var user = await _userManager.FindByEmailAsync(forgotPasswordViewModel.Email);
            if(user is null)
            {
                ModelState.AddModelError("Email", "User not found by email");
                return View();
            }

            //https://localhost:7142/Auth/ResetPassword?email=example@gmail.com&token=asdlsand;

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var link = Url.Action("ResetPassword", "Auth", new {email = forgotPasswordViewModel.Email, token = token}, HttpContext.Request.Scheme);

            string body = await GetEmailTemplateAsync(link);
            MailRequest mailRequest = new MailRequest
            {
                ToEmail = forgotPasswordViewModel.Email,
                Subject = "Reset your password",
                Body = body
            };

            await _mailService.SendEmailAsync(mailRequest);

            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> ResetPassword(string email, string token)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(token))
                return BadRequest();

            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return NotFound();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel, string email, string token)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(token))
                return BadRequest();

            if (!ModelState.IsValid)
                return View();

            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return NotFound();

           var identityResult = await _userManager.ResetPasswordAsync(user, token, resetPasswordViewModel.NewPassword);
            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }

            return RedirectToAction(nameof(Login));
        }

        private async Task<string> GetEmailTemplateAsync(string link)
        {
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "templates", "reset-password.html");
            using StreamReader sr = new StreamReader(path);
            string result = await sr.ReadToEndAsync();
            return result.Replace("[link]", link);
        }
    }
}