namespace CampusShare.Web.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public DateTime FechaReserva { get; set; }
        public EstadoReserva Estado { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
