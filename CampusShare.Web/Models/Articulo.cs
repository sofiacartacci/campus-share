namespace CampusShare.Web.Models
{
    public class Articulo
    {
        public string Id { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public bool Disponible { get; set; }
        public TipoArticulo TipoArticulo { get; set; }

        public Articulo() { }

        public Articulo(string id, string nombre, string descripcion, bool disponible, TipoArticulo tipoArticulo)
        {
            Id = id;
            Nombre = nombre;
            Descripcion = descripcion;
            Disponible = disponible;
            TipoArticulo = tipoArticulo;
        }
    }
}
