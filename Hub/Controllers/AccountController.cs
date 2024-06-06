using Hub.Data;
using Hub.Data.Enum;
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
                            return RedirectToAction("Index", "Users");
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
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid) return View(registerViewModel);

            var user = await _userManager.FindByEmailAsync(registerViewModel.Email);

            if (user != null)
            {
                TempData["Error"] = "This email address is already in user";
                return View(registerViewModel);
            }

            var newUser = new User()
            {
                FirstName = registerViewModel.First,
                LastName = registerViewModel.Last,
                Email = registerViewModel.Email,
                UserName = registerViewModel.UserName,
                Password = registerViewModel.Password
            };

            var newUserResponse = await _userManager.CreateAsync(newUser,registerViewModel.Password);

            if (!newUserResponse.Succeeded)
            {
                var error = newUserResponse.Errors;
                return BadRequest(error);
            }
            else
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.Employee);
            }
            return RedirectToAction("Index","Products");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
    }
}
