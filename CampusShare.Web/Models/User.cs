using System.ComponentModel.DataAnnotations;

namespace CampusShare.Web.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public string Apellido { get; set; } = string.Empty;

        [Required]
        public string Dni { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public User() { }

        public User(string nombre, string apellido, string dni, string email, string role)
        {
            Nombre = nombre;
            Apellido = apellido;
            Dni = dni;
            Email = email;
            Role = role;
        }
    }
}