using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CampusShare.Web.ViewModels;
using CampusShare.Web.Context;   // DbContext
using CampusShare.Web.Models;    // Reserva, Prestamo, etc.

namespace CampusShare.Web.Controllers
{
    [Authorize(Roles = "Administrador,Admin,ADMIN,administrador")]
    public class DashboardController : Controller
    {
        private readonly CampusShareDBContext _context;

        public DashboardController(CampusShareDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // =========================================================
            // 1) CONSTANTES DE ESTADO  (AJÚSTALAS SI TUS CÓDIGOS CAMBIAN)
            // =========================================================

            // Reservas
            const int ESTADO_RESERVA_VIGENTE   = 3; // Aprobada/Vigente
            const int ESTADO_RESERVA_CANCELADA = 0; // Cancelada

            // Préstamos
            const int ESTADO_PRESTAMO_VIGENTE  = 3; // Vigente
            const int ESTADO_PRESTAMO_DEVUELTO = 4; // Devuelto

            // =========================================================
            // Helper para porcentajes
            // =========================================================
            int Porcentaje(int parte, int total)
                => total == 0 ? 0 : (int)Math.Round(parte * 100.0 / total);

            // =========================================================
            // 2) ALUMNOS
            // =========================================================

            var totalAlumnos = _context.Users
                .Count(u => u.Role == "Alumno");

            var alumnosConReservas = _context.Reservas
                .Where(r => r.AlumnoId != null)
                .Select(r => r.AlumnoId!.Value)
                .Distinct()
                .Count();

            var alumnosConPrestamos = _context.Prestamos
                .Where(p => p.AlumnoId != null)
                .Select(p => p.AlumnoId!.Value)
                .Distinct()
                .Count();

            // =========================================================
            // 3) PRÉSTAMOS
            // =========================================================

            var totalPrestamos = _context.Prestamos.Count();

            var prestamosVigentes = _context.Prestamos
                .Count(p => (int)p.Estado == ESTADO_PRESTAMO_VIGENTE);

            var prestamosDevueltos = _context.Prestamos
                .Count(p => (int)p.Estado == ESTADO_PRESTAMO_DEVUELTO);

            // =========================================================
            // 4) RESERVAS
            // =========================================================

            var totalReservas = _context.Reservas.Count();

            var reservasVigentes = _context.Reservas
                .Count(r => (int)r.Estado == ESTADO_RESERVA_VIGENTE);

            var reservasCanceladas = _context.Reservas
                .Count(r => (int)r.Estado == ESTADO_RESERVA_CANCELADA);

            // =========================================================
            // 5) ARTÍCULOS
            // =========================================================

            var totalArticulos = _context.Articulos.Count();

            var articulosReservados = _context.Reservas
                .Where(r => (int)r.Estado == ESTADO_RESERVA_VIGENTE)
                .Select(r => r.ArticuloId)
                .Distinct()
                .Count();

            var articulosDisponibles = totalArticulos - articulosReservados;
            // =========================================================
            // 5bis) USUARIOS (para la tarjeta "Gestionar Usuarios")
            // =========================================================

            var totalUsuarios = _context.Users.Count();

            var totalAdmins = _context.Users
                .Count(u => u.Role == "Admin" || u.Role == "Administrador");

            // porcentajes de alumnos y admins sobre el total de usuarios
            var porcAlumnos = Porcentaje(totalAlumnos, totalUsuarios);
            var porcAdmins  = Porcentaje(totalAdmins,  totalUsuarios);
            // =========================================================
            // 6) ARMAR VIEWMODEL
            // =========================================================

            var vm = new AdminDashboardViewModel
            {
                // Alumnos
                TotalAlumnos = totalAlumnos,
                AlumnosConReservas = alumnosConReservas,
                PorcAlumnosConReserva = Porcentaje(alumnosConReservas, totalAlumnos),
                AlumnosConPrestamos = alumnosConPrestamos,
                PorcAlumnosConPrestamo = Porcentaje(alumnosConPrestamos, totalAlumnos),

                // Préstamos
                TotalPrestamos = totalPrestamos,
                PrestamosVigentes = prestamosVigentes,
                PorcPrestamosVigentes = Porcentaje(prestamosVigentes, totalPrestamos),
                PrestamosDevueltos = prestamosDevueltos,
                PorcPrestamosDevueltos = Porcentaje(prestamosDevueltos, totalPrestamos),

                // Reservas
                TotalReservas = totalReservas,
                ReservasVigentes = reservasVigentes,
                PorcReservasVigentes = Porcentaje(reservasVigentes, totalReservas),
                ReservasCanceladas = reservasCanceladas,
                PorcReservasCanceladas = Porcentaje(reservasCanceladas, totalReservas),

                // Artículos
                TotalArticulos = totalArticulos,
                ArticulosReservados = articulosReservados,
                PorcArticulosReservados = Porcentaje(articulosReservados, totalArticulos),
                ArticulosDisponibles = articulosDisponibles,
                PorcArticulosDisponibles = Porcentaje(articulosDisponibles, totalArticulos),

                // Usuarios
                TotalUsuarios = totalUsuarios,
                TotalAdmins   = totalAdmins,
                PorcAlumnos   = porcAlumnos,
                PorcAdmins    = porcAdmins
            };

            return View(vm);
        }
    }
}
