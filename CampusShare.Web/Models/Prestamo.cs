using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusShare.Web.Models
{
    public class Prestamo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime FecInicio { get; set; }
        public DateTime FecFin { get; set; }

        public EstadoRP Estado { get; set; } = EstadoRP.Vigente;

        [ForeignKey("Articulo")]
        public int? ArticuloId { get; set; }
        public Articulo? Articulo { get; set; }

        [ForeignKey("Alumno")]
        public int? AlumnoId { get; set; }
        public Alumno? Alumno { get; set; }

        public Prestamo() { }

        public Prestamo(DateTime fecInicio, DateTime fecFin, Articulo articulo, int alumnoId = 0)
        {
            FecInicio = fecInicio;
            FecFin = fecFin;
            Articulo = articulo;
            ArticuloId = articulo?.Id;
            AlumnoId = alumnoId == 0 ? null : alumnoId;
            Estado = EstadoRP.Vigente;
        }

        internal void Devolvete()
        {
            if (Estado == EstadoRP.Vigente)
            {
                Estado = EstadoRP.Realizada;
            }
        }
    }
}
