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

        public string? FecInicio { get; set; }
        public string? FecFin { get; set; }

        public EstadoRP Estado { get; set; } = EstadoRP.Vigente;

        public Articulo? Articulo { get; set; }

        public Prestamo() { }

        public Prestamo(string fecInicio, string fecFin, Articulo articulo)
        {
            FecInicio = fecInicio;
            FecFin = fecFin;
            Articulo = articulo;
        }

        internal void Devolvete()
        {
            this.Estado = EstadoRP.Realizada;
        }
    }
}
