using System;
using System.ComponentModel.DataAnnotations;

namespace CampusShare.Web.Models
{
    public class Prestamo
    {
        [Key]
        public string? Id { get; set; }
        public string? FecInicio { get; set; }
        public string? FecFin { get; set; }
        public EstadoPrestamo EstadoPrestamo { get; set; } = EstadoPrestamo.VIGENTE;

        public Articulo? Articulo { get; set; }

        public Prestamo() { }

        public Prestamo(string id, string fecInicio, string fecFin, Articulo articulo)
        {
            Id = id;
            FecInicio = fecInicio;
            FecFin = fecFin;
            Articulo = articulo;
        }
    }
}
