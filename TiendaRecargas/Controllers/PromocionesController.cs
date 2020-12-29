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
            var model = await _context.RT_Promociones.ToListAsync();
            return View(model);
        }

        // GET: PromocionesController/Details/5
        public ActionResult Details(int id)
        {
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
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PromocionesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PromocionesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PromocionesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
