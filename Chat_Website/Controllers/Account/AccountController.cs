using Chat_Website.ViewModel.Account;
using Microsoft.AspNetCore.Mvc;

namespace Chat_Website.Controllers.Account
{
    public class AccountController : Controller
    {
        private readonly UserManager<UserApplication> _userManager;
        private readonly SignInManager<UserApplication> _signInManager;
        public AccountController (UserManager<UserApplication> userManager , SignInManager<UserApplication> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View("Register");
        }
        [HttpPost]
        public async Task<IActionResult> Register(registerViewModel registerView)
        {
            if (ModelState.IsValid)
            {
                UserApplication userApplication = new UserApplication()
                {
                    UserName = registerView.FirstName + " " + registerView.LastName,
                    Email = registerView.Email,
                    PhoneNumber = registerView.PhoneNumber,
                };
                var result = await _userManager.CreateAsync(userApplication, registerView.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(userApplication, isPersistent: false);
                    return RedirectToAction("Login", "Account");
                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View(registerView);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View("Login");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(loginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
                if (user != null && !string.IsNullOrEmpty(user.UserName))
                {
                    var result = await _signInManager.PasswordSignInAsync(user.UserName, loginViewModel.Password, loginViewModel.RememberMe, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Contacts", "Home");
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(loginViewModel);
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
