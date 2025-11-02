using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusShare.Web.Models
{
    public class Reserva
    {
        [Key]
        public string? Id { get; set; }

        [Required]
        public string? FecInicio { get; set; }

        [Required]
        public string? FecFin { get; set; }

        public DateTime FechaSolicitud { get; set; } = DateTime.Now;

        public EstadoReserva EstadoReserva { get; set; } = EstadoReserva.Pendiente;

        [StringLength(500)]
        public string? MotivoRechazo { get; set; }

        public Articulo? Articulo { get; set; }

        [ForeignKey("Alumno")]
        public string? AlumnoId { get; set; }

        public Alumno? Alumno { get; set; }

        public Reserva() { }

        public Reserva(string id, string fecInicio, string fecFin, Articulo articulo, Alumno alumno)
        {
            Id = id;
            FecInicio = fecInicio;
            FecFin = fecFin;
            Articulo = articulo;
            Alumno = alumno;
            AlumnoId = alumno.Id;
            FechaSolicitud = DateTime.Now;
        }
    }
}