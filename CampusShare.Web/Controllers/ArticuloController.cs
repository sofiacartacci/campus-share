using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CampusShare.Web.Context;
using CampusShare.Web.Models;

namespace CampusShare.Web.Controllers
{
    public class ArticulosController : Controller
    {
        private readonly CampusShareDBContext _context;

        public ArticulosController(CampusShareDBContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var articulos = await _context.Articulos.ToListAsync();
            return View(articulos);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var articulo = await _context.Articulos.FirstOrDefaultAsync(a => a.Id == id);
            if (articulo == null)
                return NotFound();

            return View(articulo);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Descripcion,TipoArticulo")] Articulo articulo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(articulo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(articulo);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var articulo = await _context.Articulos.FindAsync(id);
            if (articulo == null)
                return NotFound();

            return View(articulo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,TipoArticulo,Disponible")] Articulo articulo)
        {
            if (id != articulo.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(articulo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Articulos.Any(a => a.Id == articulo.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(articulo);
        }

        [HttpPost]
        public async Task<IActionResult> CambiarDisponibilidad(int id)
        {
            var articulo = await _context.Articulos.FindAsync(id);
            if (articulo == null)
                return NotFound();

            articulo.Disponible = !articulo.Disponible;
            _context.Update(articulo);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
