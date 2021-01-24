using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaRecargas.Data;
using TiendaRecargas.Models;

namespace TiendaRecargas.Controllers
{
    public class PromocionesController : BaseController
    {
        private readonly AppDbContext _context;

        public PromocionesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: PromocionesController
        public async Task<ActionResult> Index()
        {
            IsLogged();
            var model = await _context.RT_Promociones.ToListAsync();
            return View(model);
        }

        // GET: PromocionesController/Details/5
        public ActionResult Details(int id)
        {
            IsLogged();
            return View();
        }

        // GET: RecargaValores/Create
        public IActionResult Create()
        {
            IsLogged();
            return View();
        }

        // POST: RecargaValores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Promociones promocion)
        {
            IsLogged();
            if (ModelState.IsValid)
            {
                _context.Add(promocion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(promocion);
        }

        // GET: PromocionesController/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            IsLogged();
            if (id == null)
            {
                return NotFound();
            }

            var promocion = await _context.RT_Promociones.FindAsync(id);
            if (promocion == null)
            {
                return NotFound();
            }
            return View(promocion);
        }

        // POST: PromocionesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Promociones promocion)
        {
            IsLogged();
            if (id != promocion.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(promocion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();

                }
                return RedirectToAction(nameof(Index));
            }
            return View(promocion);
        }

        // GET: RecargaValores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            IsLogged();
            if (id == null)
            {
                return NotFound();
            }

            var recargaValor = await _context.RT_Promociones
                .FirstOrDefaultAsync(m => m.id == id);
            if (recargaValor == null)
            {
                return NotFound();
            }

            return View(recargaValor);
        }

        // POST: RecargaValores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            IsLogged();
            var recargaValor = await _context.RT_Promociones.FindAsync(id);
            _context.RT_Promociones.Remove(recargaValor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
