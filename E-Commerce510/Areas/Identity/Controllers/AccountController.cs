using E_Commerce510.Models;
using E_Commerce510.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace E_Commerce510.Areas.identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _identityRole;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> identityRole)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._identityRole = identityRole;
        }
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            if (_identityRole.Roles.IsNullOrEmpty())
            {
                await _identityRole.CreateAsync(new IdentityRole("Admin"));
                await _identityRole.CreateAsync(new IdentityRole("SuperAdmin"));
                await _identityRole.CreateAsync(new IdentityRole("Customer"));
                await _identityRole.CreateAsync(new IdentityRole("Company"));
            }
            return View(new RegisterVm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVm registerVm)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser applicationUser = new()
                {
                    UserName = registerVm.UserName,
                    Email = registerVm.Email,
                    Address = registerVm.Address,
                };

                var result = await _userManager.CreateAsync(applicationUser, registerVm.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(applicationUser, false);
                    await _userManager.AddToRoleAsync(applicationUser, "Customer");

                    return RedirectToAction("Index", "Home", new { area = "Customer" });
                }
                else
                {
                    //else
                    ModelState.AddModelError("Password", "Password is weak");
                }
            }
            return View(registerVm);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginVm());
        }


        public IActionResult Block() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVm loginVm)
        {
            if (ModelState.IsValid)
            {

                var appUser = await _userManager.FindByEmailAsync(loginVm.Email);

                if (appUser != null)
                {
                    if(appUser.LockoutEnd.HasValue && appUser.LockoutEnd > DateTimeOffset.UtcNow)
                    {

                        return RedirectToAction("Block");
                    }
                    bool result = await _userManager.CheckPasswordAsync(appUser, loginVm.Password);
                    if (result)
                    {
                        await _signInManager.SignInAsync(appUser, loginVm.RememberMe);
                        return RedirectToAction("Index", "Home", new { area = "Customer" });
                    }
                    else
                    {
                        ModelState.AddModelError("Password", "Don't match the password");
                    }
                }
                else
                {
                    ModelState.AddModelError("Email", "Cannot find Your Email");
                }
            }
            return View(loginVm);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account", new { area = "Identity" });
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var appUser = await _userManager.GetUserAsync(User); //Get The Info of the current user
            if (appUser != null)
            {

                ProfileVm profileUser = new ProfileVm()
                {
                    address = appUser.Address,
                    email = appUser.Email,
                    userName = appUser.UserName
                };

                return View(profileUser);
            }
            return RedirectToAction("NotFoundPage", "Home", new { area = "Customer" });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileVm profileVm)
        {

            if (ModelState.IsValid)
            {
                var appUser = await _userManager.GetUserAsync(User);
                if (appUser == null) return RedirectToAction("NotFoundPage", "Home", new { area = "Customer" });
                appUser.Email = profileVm.email;
                appUser.Address = profileVm.address;
                appUser.UserName = profileVm.userName;

                var result = await _userManager.UpdateAsync(appUser);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);

                    }
                    return View(profileVm);
                }
                await _signInManager.RefreshSignInAsync(appUser);

                if (!string.IsNullOrEmpty(profileVm.currentPassword) && !string.IsNullOrEmpty(profileVm.newPassword))
                {
                    var passwordChangeResult = await _userManager.ChangePasswordAsync(appUser, profileVm.currentPassword, profileVm.newPassword);
                    if (!passwordChangeResult.Succeeded)
                    {
                        foreach (var error in passwordChangeResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }

                        return View(profileVm);
                    }
                }
            }
            return RedirectToAction("Index", "Home", new { area = "Customer" });
        }

        public async Task<IActionResult> Delete()
        {
            var appUser = await _userManager.GetUserAsync(User);
            if (appUser == null)
            {
                return RedirectToAction("NotFoundPage", "Home", new { area = "Customer" });
            }
            var result = await _userManager.DeleteAsync(appUser);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return RedirectToAction("Profile");
            }

            await _signInManager.SignOutAsync(); // Logout after deleting account

            return RedirectToAction("Register", "Account"); // Redirect to register page

        }

        public IActionResult AccessDenied() => View();
    }
}
