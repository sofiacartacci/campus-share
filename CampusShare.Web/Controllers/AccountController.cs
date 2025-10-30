using Microsoft.AspNetCore.Mvc;
using CampusShare.Web.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace CampusShare.Web.Controllers
{
    public class AccountController : Controller
    {
        private static readonly List<User> _users = new();

        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (_users.Any(u => u.Email == user.Email))
            {
                ModelState.AddModelError("Email", "Este correo ya está registrado.");
                return View(user);
            }

            if (ModelState.IsValid)
            {
                user.Id = (_users.Count + 1).ToString();

                _users.Add(user);
                TempData["Success"] = "Registro exitoso. Ahora puedes iniciar sesión.";
                return RedirectToAction("Login");
            }

            return View(user);
        }

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetString("UserName", user.Name ?? string.Empty);
                HttpContext.Session.SetString("UserRole", user.Role ?? string.Empty);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Credenciales incorrectas";
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
