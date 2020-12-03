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
    public class RecargaValoresController : BaseController
    {
        private readonly AppDbContext _context;

        public RecargaValoresController(AppDbContext context)
        {
            _context = context;
        }

        // GET: RecargaValores
        public async Task<IActionResult> Index()
        {
            IsLogged();
            try
            {
                var model = await _context.RT_RecargaValores.ToListAsync();
                return View(model.OrderBy(x=>x.tipoRecarga).ThenByDescending(x=>x.valor).ToList());
            }
            catch (Exception ex)
            {
                var sms = ex.ToString();
                return View(new List<RecargaValor>());
            }          
        }

        // GET: RecargaValores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            IsLogged();
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
        public async Task<IActionResult> Create([Bind("id,valor,tipoRecarga")] RecargaValor recargaValor)
        {
            IsLogged();
            if (ModelState.IsValid)
            {
                _context.Add(recargaValor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(recargaValor);
        }

        // GET: RecargaValores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            IsLogged();
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

        // POST: RecargaValores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,valor,tipoRecarga")] RecargaValor recargaValor)
        {
            IsLogged();
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

        // GET: RecargaValores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            IsLogged();
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

        // POST: RecargaValores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            IsLogged();
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
