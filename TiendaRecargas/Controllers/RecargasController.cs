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
    public class RecargasController : Controller
    {
        private readonly AppDbContext _context;

        public RecargasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Recargas
        public async Task<IActionResult> Index()
        {
            return View(await _context.RT_Recargas.ToListAsync());
        }

        // GET: Recargas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recarga = await _context.RT_Recargas
                .FirstOrDefaultAsync(m => m.id == id);
            if (recarga == null)
            {
                return NotFound();
            }

            return View(recarga);
        }

        // GET: Recargas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Recargas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,tipoRecarga,numero,idValorRecarga,valor,monto,descripcion,idCuenta,date,transactionStatus,TransactionId,TransactionMsg,TransactionDate")] Recarga recarga)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recarga);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(recarga);
        }

        // GET: Recargas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recarga = await _context.RT_Recargas.FindAsync(id);
            if (recarga == null)
            {
                return NotFound();
            }
            return View(recarga);
        }

        // POST: Recargas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,tipoRecarga,numero,idValorRecarga,valor,monto,descripcion,idCuenta,date,transactionStatus,TransactionId,TransactionMsg,TransactionDate")] Recarga recarga)
        {
            if (id != recarga.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recarga);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecargaExists(recarga.id))
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
            return View(recarga);
        }

        // GET: Recargas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recarga = await _context.RT_Recargas
                .FirstOrDefaultAsync(m => m.id == id);
            if (recarga == null)
            {
                return NotFound();
            }

            return View(recarga);
        }

        // POST: Recargas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recarga = await _context.RT_Recargas.FindAsync(id);
            _context.RT_Recargas.Remove(recarga);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecargaExists(int id)
        {
            return _context.RT_Recargas.Any(e => e.id == id);
        }
    }
}
