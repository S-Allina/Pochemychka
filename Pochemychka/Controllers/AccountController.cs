
using Pochemychka.Models;
using Pochemychka.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Pochemychka.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly PochemychkaContext _context;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, PochemychkaContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<IActionResult> Users()
        {
            return View(await _context.Users.ToListAsync());
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(LoginAndRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    Email = model.Email,
                    UserName = model.Name,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    NormalizedUserName = model.LastName + " " + model.FirstName

                };

                var result = await _userManager.CreateAsync(user, model.Password);

                await _userManager.AddToRoleAsync(user, "user");

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Tests");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> LoginAsync()
        {
            
            return View(new LoginAndRegisterModel
            {
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginAndRegisterModel model)
        {

            var user = await _userManager.FindByNameAsync(model.Name);
            if (user != null)
            {
                var result =
                        await _signInManager.PasswordSignInAsync(model.Name, model.Password,false, false);
                if (result.Succeeded)
                {

                    await _userManager.UpdateAsync(user);
                    return RedirectToAction("Index", "Tests");

                }
                else
                {

                    ModelState.AddModelError("", "Неверный логин или пароль");
                }
            }

            return View(model);
        }

       
        
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Profile(string name)
        {
            
            return View(_context.Users.FirstOrDefault(u=>u.UserName==name));
        }

        [HttpPost]
        public async Task<ActionResult> Edit(User model, string userId)
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            var name = user.UserName;
            if (model.Email != null)
                user.Email = model.Email;
            if (model.UserName != null)
                user.UserName = model.UserName;
            if (model.FirstName != null)
                user.FirstName = model.FirstName;
            if (model.LastName != null)
                user.LastName = model.LastName;
            _context.Users.Update(user);
            _context.SaveChanges();
            return RedirectToAction("Profile", "Account", new { name });
        }

        
        public async Task<IActionResult> Delete(string? id)
        {
            User user = null;
            if (id == null)
            {
                user =  _context.Users.FirstOrDefault(u=>u.UserName==User.Identity.Name);
                   IdentityResult result = await _userManager.DeleteAsync(user);
                return RedirectToAction("Register", "Account");
            }
            else
            {
                user = await _userManager.FindByIdAsync(id);

                IdentityResult result = await _userManager.DeleteAsync(user);
                return RedirectToAction("Users", "Account");
            }
          
        }
    }
}
