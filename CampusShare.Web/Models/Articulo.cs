using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Identity.Client;
using Microsoft.Net.Http.Headers;

namespace CampusShare.Web.Models
{
    public class Articulo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public TipoArticulo TipoArticulo { get; set; }
        public Boolean Disponible { get; set; }

        public Articulo() { }

        public Articulo(string nombre, string descripcion, TipoArticulo tipoArticulo)
        {
            Nombre = nombre;
            Descripcion = descripcion;
            this.Disponible = true;
            TipoArticulo = tipoArticulo;
        }

        private void SetDisponible()
        {
            if (this.Disponible == true)
                this.Disponible = false;
            else
                this.Disponible = true;
        }

    }
}
