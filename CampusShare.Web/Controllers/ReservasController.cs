using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CampusShare.Web.Models;
using CampusShare.Web.Context;
using System.Security.Claims;

namespace CampusShare.Web.Controllers
{
    public class ReservasController : Controller
    {
        private readonly CampusShareDBContext _context;

        public ReservasController(CampusShareDBContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Pendientes()
        {
            var reservasPendientes = await _context.Reservas
                .Include(r => r.Articulo)
                .Include(r => r.Alumno)
                .Where(r => r.Estado == EstadoRP.Pendiente)
                .OrderBy(r => r.FechaSolicitud)
                .ToListAsync();

            return View(reservasPendientes);
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Aprobar(int id)
        {
            var reserva = await _context.Reservas
                .Include(r => r.Alumno)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reserva == null)
            {
                TempData["Error"] = "Reserva no encontrada.";
                return RedirectToAction(nameof(Pendientes));
            }

            if (reserva.Estado != EstadoRP.Pendiente)
            {
                TempData["Error"] = "Solo se pueden aprobar reservas pendientes.";
                return RedirectToAction(nameof(Pendientes));
            }

            try
            {
                var admin = new Admin();
                admin.AlumnoAprobarReserva(reserva.Alumno!, id);
                TempData["Success"] = $"Reserva #{id} aprobada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al aprobar la reserva: {ex.Message}";
            }

            return RedirectToAction(nameof(Pendientes));
        }

        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rechazar(int id, string motivo)
        {
            var reserva = await _context.Reservas.FindAsync(id);

            if (reserva == null)
                return NotFound();

            if (reserva.Estado != EstadoRP.Pendiente)
            {
                TempData["Error"] = "Solo se pueden rechazar reservas pendientes.";
                return RedirectToAction(nameof(Pendientes));
            }

            if (string.IsNullOrWhiteSpace(motivo))
            {
                TempData["Error"] = "Debe proporcionar un motivo para el rechazo.";
                return RedirectToAction(nameof(Pendientes));
            }

            reserva.Estado = EstadoRP.Rechazada;
            reserva.MotivoRechazo = motivo;

            try
            {
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Reserva #{reserva.Id} rechazada.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al rechazar la reserva: {ex.Message}";
            }

            return RedirectToAction(nameof(Pendientes));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancelar(int id)
        {
            var usuarioIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(usuarioIdClaim, out int usuarioId);
            var esAdmin = User.IsInRole("Administrador");

            var reserva = await _context.Reservas.FindAsync(id);

            if (reserva == null)
                return NotFound();

            if (reserva.AlumnoId != usuarioId && !esAdmin)
                return Forbid();

            if (reserva.Estado != EstadoRP.Pendiente && reserva.Estado != EstadoRP.Aprobada)
            {
                TempData["Error"] = "Esta reserva no puede ser cancelada.";
                return esAdmin ? RedirectToAction(nameof(Pendientes)) : RedirectToAction(nameof(MisReservas));
            }

            reserva.Estado = EstadoRP.Cancelada;

            try
            {
                await _context.SaveChangesAsync();
                TempData["Success"] = "Reserva cancelada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al cancelar la reserva: {ex.Message}";
            }

            return esAdmin ? RedirectToAction(nameof(Pendientes)) : RedirectToAction(nameof(MisReservas));
        }

        [Authorize]
        public async Task<IActionResult> MisReservas()
        {
            var usuarioIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(usuarioIdClaim, out int usuarioId);

            var reservas = await _context.Reservas
                .Include(r => r.Articulo)
                .Where(r => r.AlumnoId == usuarioId)
                .OrderByDescending(r => r.FechaSolicitud)
                .ToListAsync();

            return View(reservas);
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var usuarioIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(usuarioIdClaim, out int usuarioId);
            var esAdmin = User.IsInRole("Administrador");

            var reserva = await _context.Reservas
                .Include(r => r.Articulo)
                .Include(r => r.Alumno)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reserva == null)
                return NotFound();

            if (reserva.AlumnoId != usuarioId && !esAdmin)
                return Forbid();

            return View(reserva);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            ViewData["Articulos"] = _context.Articulos.Where(a => a.Disponible).ToList();
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FecInicio,FecFin,ArticuloId")] Reserva reserva)
        {
            var usuarioIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(usuarioIdClaim, out int usuarioId);

            if (!ModelState.IsValid)
            {
                ViewData["Articulos"] = await _context.Articulos.Where(a => a.Disponible).ToListAsync();
                return View(reserva);
            }

            reserva.AlumnoId = usuarioId;
            reserva.FechaSolicitud = DateTime.Now;
            reserva.Estado = EstadoRP.Pendiente;

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Reserva creada correctamente. Pendiente de aprobación.";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var usuarioIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(usuarioIdClaim, out int usuarioId);

            var reserva = await _context.Reservas
                .FirstOrDefaultAsync(r => r.Id == id && r.AlumnoId == usuarioId);

            if (reserva == null)
                return NotFound();

            if (reserva.Estado != EstadoRP.Pendiente)
            {
                TempData["Error"] = "Solo se pueden editar reservas pendientes.";
                return RedirectToAction("Index", "Home");
            }

            ViewData["Articulos"] = await _context.Articulos.Where(a => a.Disponible).ToListAsync();
            return View(reserva);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FecInicio,FecFin,ArticuloId")] Reserva reserva)
        {
            if (id != reserva.Id)
                return NotFound();

            var usuarioIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            int.TryParse(usuarioIdClaim, out int usuarioId);

            var reservaExistente = await _context.Reservas.FindAsync(id);
            if (reservaExistente == null || reservaExistente.AlumnoId != usuarioId)
                return NotFound();

            if (reservaExistente.Estado != EstadoRP.Pendiente)
            {
                TempData["Error"] = "Solo se pueden editar reservas pendientes.";
                return RedirectToAction("Index", "Home");
            }

            reservaExistente.FecInicio = reserva.FecInicio;
            reservaExistente.FecFin = reserva.FecFin;
            reservaExistente.ArticuloId = reserva.ArticuloId;

            try
            {
                await _context.SaveChangesAsync();
                TempData["Success"] = "Reserva actualizada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al actualizar: {ex.Message}";
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
