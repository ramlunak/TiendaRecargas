using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Administrador")]
    public class CredencialesController : BaseController
    {
        private readonly AppDbContext _context;

        public CredencialesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Credenciales
        public async Task<IActionResult> Index()
        {
            IsLogged();

            var model = await _context.RT_Credenciales.ToListAsync();
            return View(model);

        }

        // GET: Credenciales/Create
        public IActionResult Create()
        {
            IsLogged();
            return View();
        }

        // POST: Credenciales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Credenciales credenciales)
        {
            IsLogged();
            if (ModelState.IsValid)
            {
                credenciales.idCuenta = Logged.IdCuenta;
                _context.Add(credenciales);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(credenciales);
        }

        // GET: Credenciales/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            IsLogged();
            if (id == null)
            {
                return NotFound();
            }

            var recargaValor = await _context.RT_Credenciales.FindAsync(id);
            if (recargaValor == null)
            {
                return NotFound();
            }
            return View(recargaValor);
        }

        // POST: Credenciales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Credenciales credenciales)
        {
            IsLogged();
            if (id != credenciales.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(credenciales);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(credenciales);
        }

        // GET: Credenciales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            IsLogged();
            if (id == null)
            {
                return NotFound();
            }

            var credenciales = await _context.RT_Credenciales
                .FirstOrDefaultAsync(m => m.id == id);
            if (credenciales == null)
            {
                return NotFound();
            }

            return View(credenciales);
        }

        // POST: Credenciales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            IsLogged();
            var credenciales = await _context.RT_Credenciales.FindAsync(id);
            _context.RT_Credenciales.Remove(credenciales);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecargaValorExists(int id)
        {
            return _context.RT_Credenciales.Any(e => e.id == id);
        }
    }
}
