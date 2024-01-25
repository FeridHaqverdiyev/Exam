using ExamFerid.Areas.Manage.ViewModels;
using ExamFerid.Models;
using ExamFerid.Utilities.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExamFerid.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser>userManager,SignInManager<AppUser>signInManager,RoleManager<IdentityRole>roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM userVM)
        {
            if (!ModelState.IsValid)
            {
                return View(userVM);
            }
            AppUser user = new AppUser
            {
                Name = userVM.Name,
                Email = userVM.Email,
                SurName = userVM.SurName,
                UserName = userVM.Username
            };
            IdentityResult result=await _userManager.CreateAsync(user,userVM.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);

                }
                return View(userVM);
            }
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "" });
        }
        public IActionResult Login()
        {
            return View();  
        }
        [HttpPost]
        public async Task<IActionResult>Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }
            AppUser user = await _userManager.FindByNameAsync(loginVM.UsernameOrEmail);
            if(user == null)
            {
                user = await _userManager.FindByEmailAsync(loginVM.UsernameOrEmail);
                if(user == null)
                {
                    ModelState.AddModelError(string.Empty, "username ve ya email sefdir");
                    return View(loginVM);
                }
            }
            var result=await _signInManager.PasswordSignInAsync(user, loginVM.Password,loginVM.IsRemmembered,true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Bloklandi");
                return View(loginVM);
            }
            if(!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Username ,email ve ya paswword sefdir");
                return View(loginVM);

            }
            return RedirectToAction("Index", "Home", new { area = "" });

        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> CreateRole()
        {
            foreach (var item in Enum.GetValues(typeof(UserRole)))
            {
                if(!await _roleManager.RoleExistsAsync(item.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole
                    {
                        Name = item.ToString(),
                    });
                }

            }
            return RedirectToAction("Index", "Home", new { area = "" });

        }
    }
}
