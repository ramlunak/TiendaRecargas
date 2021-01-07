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
    public class ConfiguracionController : BaseController
    {
        private readonly AppDbContext _context;

        public ConfiguracionController(AppDbContext context)
        {
            _context = context;
        }
           

        public async Task<IActionResult> Edit()
        {
            IsLogged();
            var model = new Configuracion();

            var configuracion = await _context.RT_Configuracion.ToListAsync();
            if (configuracion.Count > 0)
            {
                return View(configuracion.FirstOrDefault());
            }
            return View(model);
        }

        // POST: Credenciales/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Configuracion configuracion)
        {
            IsLogged();
            if (id != configuracion.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(configuracion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
                return RedirectToAction(nameof(Edit));
            }
            return View(configuracion);
        }


    }
}
