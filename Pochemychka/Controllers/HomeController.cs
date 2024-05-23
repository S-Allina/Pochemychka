using Pochemychka.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Pochemychka.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PochemychkaContext _context;


        public HomeController(ILogger<HomeController> logger,PochemychkaContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index(string? message)
        {
            if (message != null) ViewBag.Message = message;

            return View();
        }
      
        //public async Task<IActionResult> LogIn([Bind("Email,Password")] User user)
        //{
        //    var UserFind = _context.Users.FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);

        //    if (UserFind != null)
        //    {
        //        bool isAdmin = false;
        //        if (UserFind.Type != null) {  isAdmin = true; }
        //       ViewBag.User = UserFind;
        //        return RedirectToAction("index", "Tests", new {email =UserFind.Email, name = UserFind.UserName,isAdmin,idUse=UserFind.IdUser});
        //    }
        //    else {
        //        ViewBag.Message = "Не верный логин или пароль";
        //        return
              
        //        View("Index", new { message = "Не верный логин или пароль" });
        //    }
        //}

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUser,UserName,Email,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(user);

                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index", "Home", new { message = "Вы были успешно зарегистрированы" });
                }
                catch
                {
                    return RedirectToAction("Index", "Home", new { message = "неверные данные для регистрации. Возможно такой логин уже присутствует в базе" });

                }

            }
            return RedirectToAction("Index", "Home", new { message="неверные данные для регистрации. Возможно такой логин уже присутствует в базе"});

        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}