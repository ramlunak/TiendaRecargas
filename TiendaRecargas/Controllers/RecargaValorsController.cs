using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TiendaRecargas.Data;
using TiendaRecargas.Models;

namespace TiendaRecargas.Controllers
{
    public class RecargaValorsController : Controller
    {
        private readonly AppDbContext _context;

        public RecargaValorsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: RecargaValors
        public async Task<IActionResult> Index()
        {
            return View(await _context.RT_RecargaValores.ToListAsync());
        }

        // GET: RecargaValors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recargaValor = await _context.RT_RecargaValores
                .FirstOrDefaultAsync(m => m.id == id);
            if (recargaValor == null)
            {
                return NotFound();
            }

            return View(recargaValor);
        }

        // GET: RecargaValors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RecargaValors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,valor,tipoRecarga")] RecargaValor recargaValor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recargaValor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(recargaValor);
        }

        // GET: RecargaValors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recargaValor = await _context.RT_RecargaValores.FindAsync(id);
            if (recargaValor == null)
            {
                return NotFound();
            }
            return View(recargaValor);
        }

        // POST: RecargaValors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,valor,tipoRecarga")] RecargaValor recargaValor)
        {
            if (id != recargaValor.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recargaValor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecargaValorExists(recargaValor.id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(recargaValor);
        }

        // GET: RecargaValors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recargaValor = await _context.RT_RecargaValores
                .FirstOrDefaultAsync(m => m.id == id);
            if (recargaValor == null)
            {
                return NotFound();
            }

            return View(recargaValor);
        }

        // POST: RecargaValors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recargaValor = await _context.RT_RecargaValores.FindAsync(id);
            _context.RT_RecargaValores.Remove(recargaValor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecargaValorExists(int id)
        {
            return _context.RT_RecargaValores.Any(e => e.id == id);
        }
    }
}
