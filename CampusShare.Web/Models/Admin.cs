using System;

namespace CampusShare.Web.Models
{
    public class Admin : User
    {
        public Admin() { }

        public Admin(string nombre, string apellido, string dni, string email)
            : base(nombre, apellido, dni, email, role: typeof(Admin).Name)
        {
        }

        private void AlumnoReservar(Alumno alumno, DateTime fecInicio, DateTime fecFin, Articulo articulo)
        {
            alumno.RecibirReserva(fecInicio, fecFin, articulo);
        }

        private void AlumnoCancelarReserva(Alumno alumno, int idReserva)
        {
            alumno.RecibirCancelacion(idReserva);
        }

        private void AlumnoTomarPrestamo(Alumno alumno, int idReserva)
        {
            alumno.RecibirEjecucion(idReserva);
        }

        private void AlumnoDevolverPrestamo(Alumno alumno, int idPrestamo)
        {
            alumno.RecibirDevolucion(idPrestamo);
        }
    }
}
