using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CampusShare.Web.Context;
using CampusShare.Web.Models;

namespace CampusShare.Web.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Route("api/admin")]
    [ApiController]
    public class AdminApiController : ControllerBase
    {
        private readonly CampusShareDBContext _context;

        public AdminApiController(CampusShareDBContext context)
        {
            _context = context;
        }

        [HttpGet("reservas-cancelables/{alumnoId}")]
        public async Task<IActionResult> GetReservasCancelables(int alumnoId)
        {
            var reservas = await _context.Reservas
                .Include(r => r.Articulo)
                .Where(r => r.AlumnoId == alumnoId &&
                       (r.Estado == EstadoRP.Pendiente || r.Estado == EstadoRP.Aprobada))
                .Select(r => new
                {
                    id = r.Id,
                    articulo = r.Articulo != null ? r.Articulo.Nombre : "Sin artículo",
                    fechaInicio = r.FecInicio.ToString("dd/MM/yyyy"),
                    fechaFin = r.FecFin.ToString("dd/MM/yyyy"),
                    estado = r.Estado.ToString()
                })
                .ToListAsync();

            return Ok(reservas);
        }

        [HttpGet("reservas-pendientes/{alumnoId}")]
        public async Task<IActionResult> GetReservasPendientes(int alumnoId)
        {
            var reservas = await _context.Reservas
                .Include(r => r.Articulo)
                .Where(r => r.AlumnoId == alumnoId && r.Estado == EstadoRP.Pendiente)
                .Select(r => new
                {
                    id = r.Id,
                    articulo = r.Articulo != null ? r.Articulo.Nombre : "Sin artículo",
                    fechaInicio = r.FecInicio.ToString("dd/MM/yyyy"),
                    fechaFin = r.FecFin.ToString("dd/MM/yyyy")
                })
                .ToListAsync();

            return Ok(reservas);
        }

        [HttpGet("reservas-aprobadas/{alumnoId}")]
        public async Task<IActionResult> GetReservasAprobadas(int alumnoId)
        {
            var reservas = await _context.Reservas
                .Include(r => r.Articulo)
                .Where(r => r.AlumnoId == alumnoId && r.Estado == EstadoRP.Aprobada)
                .Select(r => new
                {
                    id = r.Id,
                    articulo = r.Articulo != null ? r.Articulo.Nombre : "Sin artículo",
                    fechaInicio = r.FecInicio.ToString("dd/MM/yyyy"),
                    fechaFin = r.FecFin.ToString("dd/MM/yyyy")
                })
                .ToListAsync();

            return Ok(reservas);
        }

        [HttpGet("prestamos-vigentes/{alumnoId}")]
        public async Task<IActionResult> GetPrestamosVigentes(int alumnoId)
        {
            var prestamos = await _context.Prestamos
                .Include(p => p.Articulo)
                .Where(p => p.AlumnoId == alumnoId && p.Estado == EstadoRP.Vigente)
                .Select(p => new
                {
                    id = p.Id,
                    articulo = p.Articulo != null ? p.Articulo.Nombre : "Sin artículo",
                    fechaInicio = p.FecInicio.ToString("dd/MM/yyyy"),
                    fechaFin = p.FecFin.ToString("dd/MM/yyyy")
                })
                .ToListAsync();

            return Ok(prestamos);
        }

        [HttpPost("crear-reserva")]
        public async Task<IActionResult> CrearReserva([FromBody] CrearReservaRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Datos inválidos");

            var alumno = await _context.Alumnos.FindAsync(request.AlumnoId);
            if (alumno == null)
                return NotFound("Alumno no encontrado");

            var articulo = await _context.Articulos.FindAsync(request.ArticuloId);
            if (articulo == null)
                return NotFound("Artículo no encontrado");

            var reserva = new Reserva
            {
                AlumnoId = request.AlumnoId,
                ArticuloId = request.ArticuloId,
                FecInicio = request.FechaInicio,
                FecFin = request.FechaFin,
                Estado = EstadoRP.Pendiente,
                FechaSolicitud = DateTime.Now
            };

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Reserva creada exitosamente" });
        }

        [HttpPost("cancelar-reserva")]
        public async Task<IActionResult> CancelarReserva([FromBody] CancelarReservaRequest request)
        {
            var reserva = await _context.Reservas
                .Include(r => r.Articulo)
                .FirstOrDefaultAsync(r => r.Id == request.ReservaId);

            if (reserva == null)
                return NotFound("Reserva no encontrada");

            if (reserva.Estado != EstadoRP.Pendiente && reserva.Estado != EstadoRP.Aprobada)
                return BadRequest("Solo se pueden cancelar reservas pendientes o aprobadas");

            reserva.Estado = EstadoRP.Cancelada;

            if (reserva.Articulo != null)
                reserva.Articulo.Disponible = true;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Reserva cancelada exitosamente" });
        }

        [HttpPost("aprobar-reserva")]
        public async Task<IActionResult> AprobarReserva([FromBody] AprobarReservaRequest request)
        {
            var reserva = await _context.Reservas
                .Include(r => r.Articulo)
                .FirstOrDefaultAsync(r => r.Id == request.ReservaId);

            if (reserva == null)
                return NotFound("Reserva no encontrada");

            if (reserva.Estado != EstadoRP.Pendiente)
                return BadRequest("Solo se pueden aprobar reservas pendientes");

            reserva.Estado = EstadoRP.Aprobada;

            if (reserva.Articulo != null)
                reserva.Articulo.Disponible = false;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Reserva aprobada exitosamente" });
        }

        [HttpPost("tomar-prestamo")]
        public async Task<IActionResult> TomarPrestamo([FromBody] TomarPrestamoRequest request)
        {
            var reserva = await _context.Reservas
                .Include(r => r.Articulo)
                .FirstOrDefaultAsync(r => r.Id == request.ReservaId);

            if (reserva == null)
                return NotFound("Reserva no encontrada");

            if (reserva.Estado != EstadoRP.Aprobada)
                return BadRequest("Solo se pueden tomar préstamos de reservas aprobadas");

            if (reserva.Articulo == null)
                return BadRequest("La reserva no tiene artículo asociado");

            var prestamo = new Prestamo
            {
                AlumnoId = reserva.AlumnoId,
                ArticuloId = reserva.ArticuloId,
                FecInicio = reserva.FecInicio,
                FecFin = reserva.FecFin,
                Estado = EstadoRP.Vigente
            };

            reserva.Estado = EstadoRP.Realizada;
            reserva.Articulo.Disponible = false;

            _context.Prestamos.Add(prestamo);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Préstamo tomado exitosamente" });
        }

        [HttpPost("devolver-prestamo")]
        public async Task<IActionResult> DevolverPrestamo([FromBody] DevolverPrestamoRequest request)
        {
            var prestamo = await _context.Prestamos
                .Include(p => p.Articulo)
                .FirstOrDefaultAsync(p => p.Id == request.PrestamoId);

            if (prestamo == null)
                return NotFound("Préstamo no encontrado");

            if (prestamo.Estado != EstadoRP.Vigente)
                return BadRequest("Solo se pueden devolver préstamos vigentes");

            if (prestamo.Articulo != null)
                prestamo.Articulo.Disponible = true;

            prestamo.Estado = EstadoRP.Realizada;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Préstamo devuelto exitosamente" });
        }
    }

    public class CrearReservaRequest
    {
        public int AlumnoId { get; set; }
        public int ArticuloId { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }

    public class CancelarReservaRequest
    {
        public int ReservaId { get; set; }
    }

    public class AprobarReservaRequest
    {
        public int ReservaId { get; set; }
    }

    public class TomarPrestamoRequest
    {
        public int ReservaId { get; set; }
    }

    public class DevolverPrestamoRequest
    {
        public int PrestamoId { get; set; }
    }
}
