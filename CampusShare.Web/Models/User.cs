using System.ComponentModel.DataAnnotations;

namespace CampusShare.Web.Models
{
    public class User
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

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
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = "Alumno"; // por defecto “Alumno”

        // Alias opcional por compatibilidad con las vistas (Register.cshtml usa “Name”)
        public string Name => $"{Nombre} {Apellido}";

        public User() { }

        public User(string id, string nombre, string apellido, string dni, string email)
        {
            Id = id;
            Nombre = nombre;
            Apellido = apellido;
            Dni = dni;
            Email = email;
        }
    }
}
