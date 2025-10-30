<<<<<<< HEAD
=======
using CampusShare.Web.Models;

>>>>>>> e043d0e15ebed0e6cfeedfdb596138f97edc53dc
public class Reserva
{
    private string id;
    private string fecInicio;
    private string fecFin;
<<<<<<< HEAD
    private EstadoReserva estadoReserva = EstadoReserva.VIGENTE;
=======
    private EstadoReserva estadoReserva = EstadoReserva.Pendiente;
>>>>>>> e043d0e15ebed0e6cfeedfdb596138f97edc53dc
    private Articulo articulo;

    public Reserva(string id, string fecInicio, string fecFin, Articulo articulo)
    {
        this.id = id;
        this.fecInicio = fecInicio;
        this.fecFin = fecFin;
        this.articulo = articulo;
<<<<<<< HEAD
        // inicia siempre como VIGENTE
=======
>>>>>>> e043d0e15ebed0e6cfeedfdb596138f97edc53dc
    }
}
