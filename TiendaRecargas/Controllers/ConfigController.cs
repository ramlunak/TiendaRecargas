using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TiendaRecargas.Data;
using TiendaRecargas.Models;

namespace TiendaRecargas.Controllers
{
    public class ConfigController : BaseController
    {
        private readonly AppDbContext _context;

        public ConfigController(AppDbContext context)
        {
            _context = context;
        }

        // GET: ConfigController
        public ActionResult Index()
        {
            IsLogged();
            return View(Logged);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword([FromBody] CuentaUpdatePassword cuentaUpdatePassword)
        {
            IsLogged();
            var cuenta = await _context.RT_Cuentas.FindAsync(cuentaUpdatePassword.idCuenta);
            if (cuenta == null)
            {
                return Ok(new { error = true, errorMsg = "No se pudo actualizar la contraseña, entre en contacto con soporte técnico." });
            }
            try
            {
                cuenta.Password = cuentaUpdatePassword.Password;
                _context.RT_Cuentas.Update(cuenta);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Ok(new { error = true, errorMsg = ex.Message });
            }
            return Ok(new { error = false });
        }

    }
}
