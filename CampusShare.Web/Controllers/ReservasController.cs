using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CampusShare.Web.Models;
using CampusShare.Web.Data;
using System.Security.Claims;

namespace CampusShare.Web.Controllers
{
    public class ReservasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reservas/Pendientes (solo para administradores)
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Pendientes()
        {
            var reservasPendientes = await _context.Reservas
                .Include(r => r.Articulo)
                .Include(r => r.Alumno)
                .Where(r => r.EstadoReserva == EstadoReserva.Pendiente)
                .OrderBy(r => r.FechaSolicitud)
                .ToListAsync();

            return View(reservasPendientes);
        }

        // POST: Reservas/Aprobar/id
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Aprobar(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.Articulo)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reserva == null)
            {
                return NotFound();
            }

            if (reserva.EstadoReserva != EstadoReserva.Pendiente)
            {
                TempData["Error"] = "Solo se pueden aprobar reservas pendientes.";
                return RedirectToAction(nameof(Pendientes));
            }

            // Verificar disponibilidad del artículo en las fechas solicitadas
            var conflicto = await _context.Reservas
                .AnyAsync(r => r.Articulo != null 
                            && r.Articulo.Id == reserva.Articulo.Id
                            && r.EstadoReserva == EstadoReserva.Aprobada
                            && r.Id != reserva.Id
                            && ((string.Compare(r.FecInicio, reserva.FecFin) <= 0 
                                && string.Compare(r.FecFin, reserva.FecInicio) >= 0)));

            if (conflicto)
            {
                TempData["Error"] = "El artículo ya está reservado en esas fechas.";
                return RedirectToAction(nameof(Pendientes));
            }

            reserva.EstadoReserva = EstadoReserva.Aprobada;
            
            try
            {
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Reserva #{reserva.Id} aprobada correctamente.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al aprobar la reserva: {ex.Message}";
            }

            return RedirectToAction(nameof(Pendientes));
        }

        // POST: Reservas/Rechazar/id
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rechazar(string id, string motivo)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var reserva = await _context.Reservas.FindAsync(id);

            if (reserva == null)
            {
                return NotFound();
            }

            if (reserva.EstadoReserva != EstadoReserva.Pendiente)
            {
                TempData["Error"] = "Solo se pueden rechazar reservas pendientes.";
                return RedirectToAction(nameof(Pendientes));
            }

            if (string.IsNullOrWhiteSpace(motivo))
            {
                TempData["Error"] = "Debe proporcionar un motivo para el rechazo.";
                return RedirectToAction(nameof(Pendientes));
            }

            reserva.EstadoReserva = EstadoReserva.Rechazada;
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

        // POST: Reservas/Cancelar/id (Usuario cancela su propia reserva o Admin cancela cualquiera)
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancelar(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var esAdmin = User.IsInRole("Administrador");

            var reserva = await _context.Reservas.FindAsync(id);

            if (reserva == null)
            {
                return NotFound();
            }

            // Verificar que el usuario sea el dueño de la reserva o sea admin
            if (reserva.AlumnoId != usuarioId && !esAdmin)
            {
                return Forbid();
            }

            // Solo se pueden cancelar reservas Pendientes o Aprobadas
            if (reserva.EstadoReserva != EstadoReserva.Pendiente 
                && reserva.EstadoReserva != EstadoReserva.Aprobada)
            {
                TempData["Error"] = "Esta reserva no puede ser cancelada.";
                return esAdmin ? RedirectToAction(nameof(Pendientes)) : RedirectToAction(nameof(MisReservas));
            }

            reserva.EstadoReserva = EstadoReserva.Cancelada;
            
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

        // GET: Reservas/MisReservas (Para usuarios)
        [Authorize]
        public async Task<IActionResult> MisReservas()
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var reservas = await _context.Reservas
                .Include(r => r.Articulo)
                .Where(r => r.AlumnoId == usuarioId)
                .OrderByDescending(r => r.FechaSolicitud)
                .ToListAsync();

            return View(reservas);
        }

        // GET: Reservas/Details/id
        [Authorize]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .Include(r => r.Articulo)
                .Include(r => r.Alumno)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reserva == null)
            {
                return NotFound();
            }

            // Verificar permisos
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var esAdmin = User.IsInRole("Administrador");

            if (reserva.AlumnoId != usuarioId && !esAdmin)
            {
                return Forbid();
            }

            return View(reserva);
        }
    }
}