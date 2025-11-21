using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CampusShare.Web.Context;
using CampusShare.Web.Models;
using System.Security.Claims;

namespace CampusShare.Web.Controllers
{
    public class PrestamosController : Controller
    {
        private readonly CampusShareDBContext _context;

        public PrestamosController(CampusShareDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var prestamos = await _context.Prestamos
                .Include(p => p.Articulo)
                .ToListAsync();
            return View(prestamos);
        }

        public async Task<IActionResult> MisPrestamos()
        {
            var usuarioIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (!int.TryParse(usuarioIdClaim, out int usuarioId))
            {
                return RedirectToAction("Login", "Account");
            }

            var prestamos = await _context.Prestamos
                .Include(p => p.Articulo)
                .Where(p => p.AlumnoId == usuarioId)
                .OrderByDescending(p => p.FecInicio)
                .ToListAsync();

            return View(prestamos);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var prestamo = await _context.Prestamos
                .Include(p => p.Articulo)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (prestamo == null)
                return NotFound();

            return View(prestamo);
        }

        public IActionResult Create()
        {
            ViewData["Articulos"] = _context.Articulos.Where(a => a.Disponible).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FecInicio,FecFin,Articulo")] Prestamo prestamo)
        {
            if (ModelState.IsValid)
            {
                if (prestamo.Articulo == null)
                    return BadRequest("Debe seleccionar un artículo.");

                var articulo = await _context.Articulos.FindAsync(prestamo.Articulo.Id);
                if (articulo == null || !articulo.Disponible)
                    return BadRequest("El artículo no está disponible.");

                articulo.Disponible = false;
                _context.Add(prestamo);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(prestamo);
        }

        [HttpPost]
        public async Task<IActionResult> Devolver(int id)
        {
            var prestamo = await _context.Prestamos
                .Include(p => p.Articulo)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (prestamo == null)
                return NotFound();

            if (prestamo.Articulo == null)
                return BadRequest("El préstamo no tiene artículo asociado.");

            prestamo.Estado = EstadoRP.Realizada;
            prestamo.Articulo.Disponible = true;
            
            _context.Update(prestamo);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Préstamo devuelto exitosamente.";
            return RedirectToAction("Index", "Home", new { tab = "prestamos" });
        }
    }
}