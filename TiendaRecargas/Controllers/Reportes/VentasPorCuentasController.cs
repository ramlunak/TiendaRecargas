using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TiendaRecargas.Data;
using TiendaRecargas.Extensions;
using TiendaRecargas.Models;
using TiendaRecargas.Models.Enums;
using TiendaRecargas.Provedores;

namespace TiendaRecargas.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class VentasPorCuentasController : BaseController
    {
        private readonly AppDbContext _context;

        public VentasPorCuentasController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IsLogged();

            var semana = DateTime.Now.GetSemana();
            var year = DateTime.Now.ToEasternStandardTime().Year;
            var s = $"{year}-W{semana}";

            var textsemana = semana < 10 ? $"0{semana}" : semana.ToString();
            ViewBag.Semana = $"{year}-W{textsemana}";

            ViewBag.PrimerDiaSemana = s.FirstDateOfWeek().AddDays(0).ToEasternStandardTime().ToString("dddd, dd MMMM yyyy", CultureInfo.CreateSpecificCulture("es-ES"));
            ViewBag.UltimoDiaSemana = s.FirstDateOfWeek().AddDays(6).ToEasternStandardTime().ToString("dddd, dd MMMM yyyy", CultureInfo.CreateSpecificCulture("es-ES"));

            var vendedores = await _context.RT_Cuentas.Where(x => x.IdCuentaPadre == Logged.IdCuenta).ToListAsync();
            foreach (var item in vendedores)
            {
                item.Recargas = await _context.RT_Recargas.Where(x => x.idCuenta == item.IdCuenta && x.status == RecargaStatus.success && x.year == year && x.semana == semana).ToListAsync();
                item.Subvendedores = await _context.RT_Cuentas.Where(x => x.IdCuentaPadre == item.IdCuenta).ToListAsync();

                foreach (var subvendedor in item.Subvendedores)
                {
                    subvendedor.Recargas = await _context.RT_Recargas.Where(x => x.idCuenta == subvendedor.IdCuenta && x.status == RecargaStatus.success && x.year == year && x.semana == semana).ToListAsync();
                }
            }

            ViewBag.Vendedores = vendedores;

            return View();
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

            ViewBag.PrimerDiaSemana = s.FirstDateOfWeek().AddDays(0).ToEasternStandardTime().ToString("dddd, dd MMMM yyyy", CultureInfo.CreateSpecificCulture("es-ES"));
            ViewBag.UltimoDiaSemana = s.FirstDateOfWeek().AddDays(6).ToEasternStandardTime().ToString("dddd, dd MMMM yyyy", CultureInfo.CreateSpecificCulture("es-ES"));



            var model = await _context.RT_Recargas.Where(x => x.idCuenta == Logged.IdCuenta && x.status == RecargaStatus.success && x.year == year && x.semana == semana).ToListAsync();
            if (!string.IsNullOrEmpty(filtro.numero))
            {
                model = await _context.RT_Recargas.Where(x => x.idCuenta == Logged.IdCuenta && x.status == RecargaStatus.success && x.year == year && x.semana == semana && x.numero.Contains(filtro.numero)).ToListAsync();
            }


            if (model.Any())
            {
                ViewBag.TotalPagado = model.Sum(x => x.monto);
                ViewBag.TotalRecibido = model.Sum(x => x.recibe);
            }
            return View();
        }

    }
}
