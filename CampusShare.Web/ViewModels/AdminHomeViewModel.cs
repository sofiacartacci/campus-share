using CampusShare.Web.Models;

namespace CampusShare.Web.ViewModels
{
    public class AdminHomeViewModel
    {
        public List<Alumno> Alumnos { get; set; } = new List<Alumno>();
        public string NombreAdmin { get; set; } = string.Empty;
        public List<Articulo> Articulos { get; set; } = new List<Articulo>();
    }
}