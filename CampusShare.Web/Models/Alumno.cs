using System.Collections.Generic;

namespace CampusShare.Web.Models
{
    public class Alumno : User
    {
        public List<Reserva> Reservas { get; set; } = new List<Reserva>();
        public List<Prestamo> Prestamos { get; set; } = new List<Prestamo>();

        public Alumno() { }

        public Alumno(string nombre, string apellido, string dni, string email)
            : base(nombre, apellido, dni, email, role: typeof(Alumno).Name)
        {
            Reservas = new List<Reserva>();
            Prestamos = new List<Prestamo>();
        }

        private void Reservar(string fecInicio, string fecFin, Articulo articulo)
        {
            if (fecInicio != null && fecFin != null && articulo != null)
            {
                this.Reservas.Add(new Reserva(fecInicio, fecFin, articulo));
            }
        }

        private Reserva? BuscarReserva(int idReserva)
        {
            Reserva? buscada = null;
            int i = 0;

            while (i < this.Reservas.Count && buscada == null)
            {
                if (this.Reservas[i].Id == idReserva)
                {
                    buscada = this.Reservas[i];
                }
                else
                {
                    i++;
                }
            }
            return buscada;
        }


        private void CancelarReserva(int idReserva)
        {
            if (idReserva != 0)
            {
                var cancelar = BuscarReserva(idReserva);

                if (cancelar != null)
                {
                    cancelar.Cancelate();
                    Console.WriteLine($"Se ha cancelado la reserva {cancelar}");
                }
                else
                {
                    Console.WriteLine("La reserva buscada no existe o esta vigente");
                }
            }
        }

        private void TomarPrestamo(int idReserva)
        {
            var tomar = BuscarReserva(idReserva);

            if (tomar != null)
            {
                tomar.IrAPrestamo();
                this.Prestamos.Add(new Prestamo(tomar.FecInicio, tomar.FecFin, tomar.Articulo));
            }
        }


        private void DevolverPrestamo(int idPrestamo)
        {
            var aDevolver = BuscarPrestamo(idPrestamo);

            if (aDevolver != null)
            {
                aDevolver.Devolvete();
            }
        }


        private Prestamo? BuscarPrestamo(int idPrestamo)
        {
            Prestamo? buscado = null;
            int i = 0;

            while (i < this.Prestamos.Count && buscado == null)
            {
                if (this.Prestamos[i].Id == idPrestamo)
                {
                    buscado = this.Prestamos[i];
                }
                else
                {
                    i++;
                }
            }
            return buscado;
        }

        public void RecibirCancelacion(int idReserva)
        {
            this.CancelarReserva(idReserva);
        }

        public void RecibirEjecucion(int idReserva)
        {
            this.TomarPrestamo(idReserva);
        }

        public void RecibirDevolucion(int idPrestamo)
        {
            this.DevolverPrestamo(idPrestamo);
        }

        public void RecibirReserva(string fecInicio, string fecFin, Articulo aReservar)
        {
            this.Reservar(fecInicio, fecFin, aReservar);
        }



    }
}
