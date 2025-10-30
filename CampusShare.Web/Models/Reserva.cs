public class Reserva
{
    private string id;
    private string fecInicio;
    private string fecFin;
    private EstadoReserva estadoReserva = EstadoReserva.VIGENTE;
    private Articulo articulo;

    public Reserva(string id, string fecInicio, string fecFin, Articulo articulo)
    {
        this.id = id;
        this.fecInicio = fecInicio;
        this.fecFin = fecFin;
        this.articulo = articulo;
        // inicia siempre como VIGENTE
    }
}
