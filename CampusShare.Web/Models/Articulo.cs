using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CampusShare.Web.Models
{
    public class Articulo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public EstadoRP Estado { get; set; }
        public TipoArticulo TipoArticulo { get; set; }

        public Articulo() { }

        public Articulo(string nombre, string descripcion, TipoArticulo tipoArticulo)
        {
            Nombre = nombre;
            Descripcion = descripcion;
            Estado = EstadoRP.Disponible;
            TipoArticulo = tipoArticulo;
        }
    }
}
