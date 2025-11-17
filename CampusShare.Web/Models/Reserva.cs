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
        public DateTime FecInicio { get; set; }

        [Required]
        public DateTime FecFin { get; set; }

        public DateTime FechaSolicitud { get; set; } = DateTime.Now;

        [Required]
        public EstadoRP Estado { get; set; } = EstadoRP.Pendiente;

        public string? MotivoRechazo { get; set; }

        [ForeignKey("Alumno")]
        public int? AlumnoId { get; set; }
        public Alumno? Alumno { get; set; }

        [ForeignKey("Articulo")]
        public int? ArticuloId { get; set; }
        public Articulo? Articulo { get; set; }

        public Reserva() { }

        public Reserva(DateTime fecInicio, DateTime fecFin, int alumnoId, int articuloId)
        {
            FecInicio = fecInicio;
            FecFin = fecFin;
            AlumnoId = alumnoId;
            ArticuloId = articuloId;
            Estado = EstadoRP.Pendiente;
            FechaSolicitud = DateTime.Now;
        }

        internal void Cancelate()
        {
            if (Estado == EstadoRP.Pendiente || Estado == EstadoRP.Aprobada)
            {
                Estado = EstadoRP.Cancelada;
            }
        }

        internal void IrAPrestamo()
        {
            if (Estado == EstadoRP.Aprobada)
            {
                Estado = EstadoRP.Realizada;
            }
        }
    }
}
