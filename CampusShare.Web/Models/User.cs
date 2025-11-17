using System.ComponentModel.DataAnnotations;

namespace CampusShare.Web.Models
{
    public class User
    {
<<<<<<< HEAD
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
=======
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
>>>>>>> f6a8f063785a5cd1b3266daee39b6ab879b8e059

        public User() { }

        public User(string id, string name, string email, string password, string role)
        {
            Id = id;
<<<<<<< HEAD
            Name = name;
            Email = email;
            Password = password;
            Role = role;
=======
            Nombre = nombre;
            Apellido = apellido;
            Dni = dni;
            Email = email;
>>>>>>> f6a8f063785a5cd1b3266daee39b6ab879b8e059
        }
    }
}
