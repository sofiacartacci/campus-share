using System;
using System.Collections.Generic;
using System.Linq;
using CampusShare.Web.Context;
using Microsoft.EntityFrameworkCore;

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

        private void Reservar(DateTime fecInicio, DateTime fecFin, Articulo articulo)
        {
            if (articulo == null)
                return;

            using var context = new CampusShareDBContextFactory().CreateDbContext();
            var reserva = new Reserva(fecInicio, fecFin, this.Id, articulo.Id);
            context.Reservas.Add(reserva);
            context.SaveChanges();
        }

        private Reserva? BuscarReserva(int idReserva)
        {
            using var context = new CampusShareDBContextFactory().CreateDbContext();
            return context.Reservas
                .Include(r => r.Articulo)
                .FirstOrDefault(r => r.Id == idReserva && r.AlumnoId == this.Id);
        }

        private void CancelarReserva(int idReserva)
        {
            using var context = new CampusShareDBContextFactory().CreateDbContext();
            var cancelar = context.Reservas.FirstOrDefault(r => r.Id == idReserva && r.AlumnoId == this.Id);

            if (cancelar == null)
            {
                Console.WriteLine("La reserva buscada no existe o está vigente");
                return;
            }

            cancelar.Cancelate();
            context.SaveChanges();
            Console.WriteLine($"Se ha cancelado la reserva {cancelar}");
        }

        private void TomarPrestamo(int idReserva)
        {
            using var context = new CampusShareDBContextFactory().CreateDbContext();
            var tomar = context.Reservas
                .Include(r => r.Articulo)
                .FirstOrDefault(r => r.Id == idReserva && r.AlumnoId == this.Id);

            if (tomar == null || tomar.Articulo == null)
                return;

            tomar.IrAPrestamo();
            var prestamo = new Prestamo(tomar.FecInicio, tomar.FecFin, tomar.Articulo)
            {
                AlumnoId = this.Id
            };

            context.Prestamos.Add(prestamo);
            context.SaveChanges();
        }

        private void DevolverPrestamo(int idPrestamo)
        {
            using var context = new CampusShareDBContextFactory().CreateDbContext();
            var aDevolver = context.Prestamos.FirstOrDefault(p => p.Id == idPrestamo && p.AlumnoId == this.Id);

            if (aDevolver == null)
                return;

            aDevolver.Devolvete();
            context.SaveChanges();
        }

        private Prestamo? BuscarPrestamo(int idPrestamo)
        {
            using var context = new CampusShareDBContextFactory().CreateDbContext();
            return context.Prestamos
                .Include(p => p.Articulo)
                .FirstOrDefault(p => p.Id == idPrestamo && p.AlumnoId == this.Id);
        }

        public void RecibirCancelacion(int idReserva) => CancelarReserva(idReserva);

        public void RecibirEjecucion(int idReserva) => TomarPrestamo(idReserva);

        public void RecibirDevolucion(int idPrestamo) => DevolverPrestamo(idPrestamo);

        public void RecibirReserva(DateTime fecInicio, DateTime fecFin, Articulo aReservar) =>
            Reservar(fecInicio, fecFin, aReservar);
    }
}
