using System.Collections.Generic;

namespace CampusShare.Web.Models
{
    public class Alumno : User
    {
        public List<Reserva> Reservas { get; set; } = new List<Reserva>();
        public List<Reserva> ReservasHistorial { get; set; } = new List<Reserva>();
        public List<Prestamo> PrestamosHistorial { get; set; } = new List<Prestamo>();

        public Alumno() { }

        public Alumno(string id, string nombre, string apellido, string dni, string email)
            : base(id, nombre, apellido, dni, email)
        {
            Reservas = new List<Reserva>();
            ReservasHistorial = new List<Reserva>();
            PrestamosHistorial = new List<Prestamo>();
        }
    }
}
