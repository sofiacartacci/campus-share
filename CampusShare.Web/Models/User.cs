using System.ComponentModel.DataAnnotations;

namespace CampusShare.Web.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, Display(Name = "Nombre completo")]
        public string? Name { get; set; }

        [Required, EmailAddress, Display(Name = "Email")]
        public string? Email { get; set; }

        [Required, MinLength(8), Display(Name = "Contraseña")]
        public string? Password { get; set; }

        [Required, Display(Name = "Rol")]
        public string? Role { get; set; } = "Alumno"; // Alumno o Admin
    }
}
