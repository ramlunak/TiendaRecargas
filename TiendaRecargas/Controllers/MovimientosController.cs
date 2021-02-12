using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TiendaRecargas.Data;
using TiendaRecargas.Extensions;
using TiendaRecargas.Models;
using TiendaRecargas.Models.Enums;

namespace TiendaRecargas.Controllers
{
    public class MovimientosController : Controller
    {
        private readonly AppDbContext _context;
        public MovimientosController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> Index()
        {
            var semana = DateTime.Now.ToEasternStandardTime().GetSemana();
            var year = DateTime.Now.ToEasternStandardTime().Year;
            var s = $"{year}-W{semana}";

            var textsemana = semana < 10 ? $"0{semana}" : semana.ToString();
            ViewBag.Semana = $"{year}-W{textsemana}";

            var primerdia = s.FirstDateOfWeek().AddDays(0).ToEasternStandardTime();
            var ultimodia = s.FirstDateOfWeek().AddDays(6).ToEasternStandardTime();

            ViewBag.PrimerDiaSemana = primerdia.ToString("dddd, dd MMMM yyyy", CultureInfo.CreateSpecificCulture("es-ES"));
            ViewBag.UltimoDiaSemana = ultimodia.ToString("dddd, dd MMMM yyyy", CultureInfo.CreateSpecificCulture("es-ES"));


            var model = await _context.RT_Movimientos.Where(x => x.fecha >= primerdia && x.fecha <= ultimodia).ToListAsync();

            foreach (var item in model)
            {
                var userLog = _context.RT_Cuentas.Where(x => x.IdCuenta == item.idUserLogged).FirstAsync().Result.Nombre;
                var cuenta = _context.RT_Cuentas.Where(x => x.IdCuenta == item.idCuenta).FirstAsync().Result.Nombre;

                item.UsuarioLoggado = userLog;
                item.Cuenta = cuenta;
            }

            ViewBag.Cuentas = await _context.RT_Cuentas.Where(x => x.Rol == RolesSistema.Administrador.ToString() || x.Rol == RolesSistema.Vendedor.ToString()).ToListAsync();

            return View(model.OrderByDescending(x => x.fecha).ThenBy(x => x.UsuarioLoggado).ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(RecargaSearch filtro)
        {
            var semana = filtro.semana;
            var year = filtro.year;
            var s = $"{year}-W{semana}";

            var textsemana = semana < 10 ? $"0{semana}" : semana.ToString();
            ViewBag.Semana = $"{year}-W{textsemana}";
            ViewBag.Numero = filtro.numero;

            var primerdia = s.FirstDateOfWeek().AddDays(0).ToEasternStandardTime();
            var ultimodia = s.FirstDateOfWeek().AddDays(6).ToEasternStandardTime();

            ViewBag.PrimerDiaSemana = primerdia.ToString("dddd, dd MMMM yyyy", CultureInfo.CreateSpecificCulture("es-ES"));
            ViewBag.UltimoDiaSemana = ultimodia.ToString("dddd, dd MMMM yyyy", CultureInfo.CreateSpecificCulture("es-ES"));

            var model = await _context.RT_Movimientos.Where(x => x.fecha >= primerdia && x.fecha <= ultimodia).ToListAsync();
            if (filtro.idCuenta.HasValue)
            {
                model = await _context.RT_Movimientos.Where(x => x.idUserLogged == filtro.idCuenta && x.fecha >= primerdia && x.fecha <= ultimodia).ToListAsync();
            }

            foreach (var item in model)
            {
                var userLog = _context.RT_Cuentas.Where(x => x.IdCuenta == item.idUserLogged).FirstAsync().Result.Nombre;
                var cuenta = _context.RT_Cuentas.Where(x => x.IdCuenta == item.idCuenta).FirstAsync().Result.Nombre;

                item.UsuarioLoggado = userLog;
                item.Cuenta = cuenta;
            }

            ViewBag.Cuentas = await _context.RT_Cuentas.Where(x => x.Rol == RolesSistema.Administrador.ToString() || x.Rol == RolesSistema.Vendedor.ToString()).ToListAsync();

            return View(model.OrderByDescending(x => x.fecha).ThenBy(x => x.UsuarioLoggado).ToList());

        }

    }
}
