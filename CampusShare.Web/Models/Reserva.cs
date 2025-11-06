using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CampusShare.Web.Models
{
    public class Reserva
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string? FecInicio { get; set; }

        [Required]
        public string? FecFin { get; set; }

        public DateTime FechaSolicitud { get; set; } = DateTime.Now;


        public EstadoRP Estado { get; set; } = EstadoRP.Pendiente;

        [StringLength(500)]
        public string? MotivoRechazo { get; set; }

        public Articulo? Articulo { get; set; }

        [ForeignKey("Alumno")]
        public string? AlumnoId { get; set; }

        public Alumno? Alumno { get; set; }

        public Reserva() { }

        public Reserva(string fecInicio, string fecFin, Articulo articulo)
        {
            FecInicio = fecInicio;
            FecFin = fecFin;
            Articulo = articulo;
            Estado = EstadoRP.Pendiente;
            FechaSolicitud = DateTime.Now;
        }

        internal void Cancelate()
        {
            this.Estado = EstadoRP.Cancelada;
        }

        internal void IrAPrestamo()
        {
            this.Estado = EstadoRP.Realizada;
        }

        internal void Autorizarse()
        {
            this.Estado = EstadoRP.Vigente;
        }
    }
}
