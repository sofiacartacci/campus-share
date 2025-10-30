using System.Collections.Generic;

public class Alumno : User
{
    private List<Reserva> reservas;
    private List<Reserva> reservasHistorial;
    private List<Prestamo> prestamosHistorial;

    public Alumno(string id, string nombre, string apellido, string dni, string email)
        : base(id, nombre, apellido, dni, email)
    {
        reservas = new List<Reserva>();
        reservasHistorial = new List<Reserva>();
        prestamosHistorial = new List<Prestamo>();
    }
}
