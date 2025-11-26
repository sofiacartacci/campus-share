using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CampusShare.Web.Context;
using CampusShare.Web.Models;

namespace CampusShare.Web.Controllers
{
    [Authorize(Roles = "Administrador,Admin,ADMIN,administrador")]
    public class AlumnosController : Controller
    {
        
        private readonly CampusShareDBContext _context;

        public AlumnosController(CampusShareDBContext context)
        {
            _context = context;
        }

        // Lista de alumnos (para "Gestionar Usuarios")
        public IActionResult Index()
        {
            var alumnos = _context.Users
                .Where(u => u.Role == "Alumno")
                .ToList();

            return View(alumnos);
        }
    }
}
