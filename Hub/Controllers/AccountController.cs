using Hub.Data;
using Hub.Models;
using Hub.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Hub.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<User> user,SignInManager<User> signInManager, 
            ApplicationDbContext applicationDbContext, RoleManager<IdentityRole> roleManager)
        {
            _userManager = user;
            _signInManager = signInManager;
            _context = applicationDbContext;
            _roleManager = roleManager;
        }

        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) return View(loginViewModel);

            var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
            //User is found
            if(user != null)
            {
                //Checking the password
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
                if (passwordCheck)
                {
                    //Password is correct, signing in
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        //checking the roles
                        var roleFarmer = await _userManager.IsInRoleAsync(user,"Farmer");
                        var roleEmployee = await _userManager.IsInRoleAsync(user, "Employee");

                        if (roleFarmer)
                        {
                            return RedirectToAction("Index", "Products");
                        }

                        if (roleEmployee)
                        {
                            return RedirectToAction("Index", "User");
                        }
                        
                    }
                }
                //Password is incorrect
                TempData["Error"] = "Incorrect password. Please try again";
                return View(loginViewModel);
            }
            //user not found
            TempData["Error"] = "Incorrect credentials. Please try again";
            return View(loginViewModel);
        }
        public IActionResult Register()
        {
            var response = new RegisterViewModel();
            return View(response);
        }

    }
}
