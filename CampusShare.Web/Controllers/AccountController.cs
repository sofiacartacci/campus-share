using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using CampusShare.Web.Models;
using CampusShare.Web.Context;

namespace CampusShare.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly CampusShareDBContext _context;

        public AccountController(CampusShareDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            if (!ModelState.IsValid)
                return View(user);

            var emailExistente = await _context.Users
                .AnyAsync(u => u.Email == user.Email);

            if (emailExistente)
            {
                ModelState.AddModelError("Email", "Este email ya está registrado.");
                return View(user);
            }

            var dniExistente = await _context.Users
                .AnyAsync(u => u.Dni == user.Dni);

            if (dniExistente)
            {
                ModelState.AddModelError("Dni", "Este DNI ya está registrado.");
                return View(user);
            }

            User nuevoUsuario;
            
            if (user.Role == "Admin")
            {
                nuevoUsuario = new Admin
                {
                    Nombre = user.Nombre,
                    Apellido = user.Apellido,
                    Dni = user.Dni,
                    Email = user.Email,
                    Password = user.Password,
                    Role = "Admin"
                };
            }
            else
            {
                nuevoUsuario = new Alumno
                {
                    Nombre = user.Nombre,
                    Apellido = user.Apellido,
                    Dni = user.Dni,
                    Email = user.Email,
                    Password = user.Password,
                    Role = "Alumno"
                };
            }

            await _context.Users.AddAsync(nuevoUsuario);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Usuario registrado correctamente.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                TempData["Error"] = "Debe ingresar correo y contraseña.";
                return View();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                TempData["Error"] = "Credenciales inválidas.";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Nombre),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Role, user.Role ?? "Alumno")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            TempData["Success"] = $"Bienvenido, {user.Nombre}.";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        // 🔹 Acción para mostrar los datos del usuario logueado
        [HttpGet]
        public async Task<IActionResult> MiPerfil()
        {
            var idUsuario = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var usuario = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == idUsuario);

            if (usuario == null)
                return NotFound();

            return View(usuario);
        }
    }
}
