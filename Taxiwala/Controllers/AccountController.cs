using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Taxiwala.Data;
using Taxiwala.Models;
using Taxiwala.ViewModel;

namespace Taxiwala.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;


        public AccountController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login","Account");
        }
        [HttpGet]
        public IActionResult RegisterCustomer()
        {
            return View();
        }
        [HttpGet]
        public IActionResult RegisterDriver()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterDriver(RegisterViewModel registerViewModel)
        {

            if (!ModelState.IsValid) return View(registerViewModel);

            var user = await _userManager.FindByEmailAsync(registerViewModel.EmailAddress);
            if (user != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(registerViewModel);
            }

            var newUser = new IdentityUser()
            {
                Email = registerViewModel.EmailAddress,
                UserName = registerViewModel.EmailAddress
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerViewModel.Password);
            var userId = await _userManager.FindByEmailAsync(registerViewModel.EmailAddress);
            Driver newDriver = new Driver
            {
                FirstName = registerViewModel.FirstName,
                LastName = registerViewModel.LastName,
                UserId = Guid.Parse(userId.Id.ToString())
            };
            var customer = await _context.Set<Driver>().AddAsync(newDriver);
            await _context.SaveChangesAsync();
            if (newUserResponse.Succeeded)
                await _userManager.AddToRoleAsync(newUser, UserRoles.Driver);

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> RegisterCustomer(RegisterViewModel registerViewModel)
        {

            if (!ModelState.IsValid) return View(registerViewModel);

            var user = await _userManager.FindByEmailAsync(registerViewModel.EmailAddress);
            if (user != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(registerViewModel);
            }

            var newUser = new IdentityUser()
            {
                Email = registerViewModel.EmailAddress,
                UserName = registerViewModel.EmailAddress
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerViewModel.Password);
            var userId = await _userManager.FindByEmailAsync(registerViewModel.EmailAddress);
            Customer newCustomer = new Customer
            {
                FirstName = registerViewModel.FirstName,
                LastName = registerViewModel.LastName,
                UserId = Guid.Parse(userId.Id.ToString())
            };
            var customer = await _context.Set<Customer>().AddAsync(newCustomer);
            await _context.SaveChangesAsync();
            if (newUserResponse.Succeeded)
                await _userManager.AddToRoleAsync(newUser, UserRoles.Customer);

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {

            if (!ModelState.IsValid) return View(loginViewModel);

            var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);

            if (user != null)
            {
                var role =  await _userManager.GetRolesAsync(user);
                //User is found, check password
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
                if (passwordCheck)
                {
                    //Password correct, sign in
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        if(role.FirstOrDefault().Equals(UserRoles.Driver))
                        {
                            return RedirectToAction("driverlist", "bookingride");
                        }
                        else
                        {
                            return RedirectToAction("customerbook", "bookingride");
                        }
                    }
                }
                //Password is incorrect
                TempData["Error"] = "Wrong credentials. Please try again";
                return View(loginViewModel);
            }
            //User not found
            TempData["Error"] = "Wrong credentials. Please try again";
            return View(loginViewModel);
        }
    }
}
