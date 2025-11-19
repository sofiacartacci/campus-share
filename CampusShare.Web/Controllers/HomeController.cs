using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CampusShare.Web.Context;
using CampusShare.Web.Models;
using CampusShare.Web.ViewModels;

namespace CampusShare.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CampusShareDBContext _context;

        public HomeController(ILogger<HomeController> logger, CampusShareDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var usuarioIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var nombreUsuario = User.FindFirstValue(ClaimTypes.Name);
            var rolUsuario = User.FindFirstValue(ClaimTypes.Role);

            if (!int.TryParse(usuarioIdClaim, out int usuarioId))
            {
                return RedirectToAction("Login", "Account");
            }

            if (rolUsuario == "Admin")
            {
                var alumnos = await _context.Alumnos.ToListAsync();
                var articulos = await _context.Articulos.ToListAsync();

                var viewModel = new AdminHomeViewModel
                {
                    Alumnos = alumnos,
                    Articulos = articulos,
                    NombreAdmin = nombreUsuario ?? "Administrador"
                };

                return View("AdminHome", viewModel);
            }
            else
            {
                var reservas = await _context.Reservas
                    .Include(r => r.Articulo)
                    .Where(r => r.AlumnoId == usuarioId)
                    .OrderByDescending(r => r.FechaSolicitud)
                    .ToListAsync();

                var prestamos = await _context.Prestamos
                    .Include(p => p.Articulo)
                    .Where(p => p.AlumnoId == usuarioId)
                    .OrderByDescending(p => p.FecInicio)
                    .ToListAsync();

                var viewModel = new AlumnoHomeViewModel
                {
                    Reservas = reservas,
                    Prestamos = prestamos,
                    NombreAlumno = nombreUsuario ?? "Usuario"
                };

                return View("Index", viewModel);
            }
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