using Microsoft.EntityFrameworkCore;
using CampusShare.Web.Models;

namespace CampusShare.Web.Context
{
    public class CampusShareDBContext : DbContext
    {
        public CampusShareDBContext(DbContextOptions<CampusShareDBContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Alumno> Alumnos { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Prestamo> Prestamos { get; set; }
        public DbSet<Articulo> Articulos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Articulo>()
                .Property(a => a.TipoArticulo)
                .HasConversion<string>();

            modelBuilder.Entity<Reserva>()
                .HasOne(r => r.Alumno)
                .WithMany(a => a.Reservas)
                .HasForeignKey(r => r.AlumnoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Prestamo>()
                .HasOne(p => p.Alumno)
                .WithMany(a => a.Prestamos)
                .HasForeignKey(p => p.AlumnoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Prestamo>()
                .HasOne(p => p.Articulo)
                .WithMany()
                .HasForeignKey("ArticuloId")
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}