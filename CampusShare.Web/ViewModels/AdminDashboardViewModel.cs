namespace CampusShare.Web.ViewModels
{
    public class AdminDashboardViewModel
    {
        public int TotalAlumnos { get; set; }
        public int AlumnosConReservas { get; set; }
        public int AlumnosConPrestamos { get; set; }
        public int PorcAlumnosConReserva { get; set; }
        public int PorcAlumnosConPrestamo { get; set; }

        public int TotalPrestamos { get; set; }
        public int PrestamosVigentes { get; set; }
        public int PrestamosDevueltos { get; set; }
        public int PorcPrestamosVigentes { get; set; }
        public int PorcPrestamosDevueltos { get; set; }

        public int TotalReservas { get; set; }
        public int ReservasVigentes { get; set; }
        public int ReservasCanceladas { get; set; }
        public int PorcReservasVigentes { get; set; }
        public int PorcReservasCanceladas { get; set; }

        public int TotalArticulos { get; set; }
        public int ArticulosReservados { get; set; }
        public int ArticulosDisponibles { get; set; }
        public int PorcArticulosReservados { get; set; }
        public int PorcArticulosDisponibles { get; set; }

        public int TotalUsuarios { get; set; }
        public int TotalAdmins { get; set; }
        public int PorcAlumnos { get; set; }
        public int PorcAdmins { get; set; }
    }
}
