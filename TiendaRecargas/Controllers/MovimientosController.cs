using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaRecargas.Data;

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
            var model = await _context.RT_Movimientos.ToListAsync();

            foreach (var item in model)
            {
                var userLog = _context.RT_Cuentas.Where(x => x.IdCuenta == item.idUserLogged).FirstAsync().Result.Nombre;
                var cuenta = _context.RT_Cuentas.Where(x => x.IdCuenta == item.idCuenta).FirstAsync().Result.Nombre;

                item.UsuarioLoggado = userLog;
                item.Cuenta = cuenta;
            }

            return View(model);
        }

    }
}
