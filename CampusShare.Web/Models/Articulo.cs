public class Articulo
{
    private string id;
    private string nombre;
    private string descripcion;
    private bool disponible;
    private TipoArticulo tipoArticulo;

    public Articulo(string id, string nombre, string descripcion, bool disponible, TipoArticulo tipoArticulo)
    {
        this.id = id;
        this.nombre = nombre;
        this.descripcion = descripcion;
        this.disponible = disponible;
        this.tipoArticulo = tipoArticulo;
    }
}
