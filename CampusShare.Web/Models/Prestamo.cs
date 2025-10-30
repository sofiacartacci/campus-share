public class Prestamo
{
    private string id;
    private string fecInicio;
    private string fecFin;
    private EstadoPrestamo estadoPrestamo = EstadoPrestamo.VIGENTE;
    private Articulo articulo;

    public Prestamo(string id, string fecInicio, string fecFin, Articulo articulo)
    {
        this.id = id;
        this.fecInicio = fecInicio;
        this.fecFin = fecFin;
        this.articulo = articulo;
    }
}
