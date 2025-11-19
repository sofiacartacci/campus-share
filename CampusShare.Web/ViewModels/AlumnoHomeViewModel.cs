using CampusShare.Web.Models;

namespace CampusShare.Web.ViewModels
{
    public class AlumnoHomeViewModel
    {
        public List<Reserva> Reservas { get; set; } = new List<Reserva>();
        public List<Prestamo> Prestamos { get; set; } = new List<Prestamo>();
        public string NombreAlumno { get; set; } = string.Empty;
    }
}