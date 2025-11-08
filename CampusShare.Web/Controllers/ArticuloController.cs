using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CampusShare.Web.Context;
using CampusShare.Web.Models;

namespace CampusShare.Web.Controllers
{
    [Authorize(Roles = "Admin")]
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
            ViewData["Articulos"] = articulos;
            return View(new Articulo());
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Descripcion,TipoArticulo,Disponible")] Articulo articulo)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "No se pudo crear el artículo.";
                var articulosError = await _context.Articulos.ToListAsync();
                return View("Index", articulosError);
            }

            _context.Add(articulo);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Artículo creado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Descripcion,TipoArticulo,Disponible")] Articulo articulo)
        {
            if (id != articulo.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "No se pudo actualizar el artículo.";
                var articulosError = await _context.Articulos.ToListAsync();
                return View("Index", articulosError);
            }

            try
            {
                _context.Update(articulo);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Artículo actualizado correctamente.";
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Articulo? articulo = await _context.Articulos.FindAsync(id);
            if (articulo == null)
            {
                TempData["Error"] = "Artículo no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            _context.Articulos.Remove(articulo);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Artículo eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> CambiarDisponibilidad(int id)
        {
            Articulo? articulo = await _context.Articulos.FindAsync(id);
            if (articulo == null)
                return NotFound();

            articulo.Disponible = !articulo.Disponible;
            _context.Update(articulo);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Disponibilidad actualizada.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Manage(int? id)
        {
            ViewData["SelectedId"] = id;
            var articulos = await _context.Articulos.ToListAsync();
            Articulo? articulo = null;
            if (id != null)
                articulo = await _context.Articulos.FindAsync(id);
            ViewData["Articulos"] = articulos;
            return View("Index", articulo ?? new Articulo());
        }
    }
}
