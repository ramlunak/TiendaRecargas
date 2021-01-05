using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TiendaRecargas.Data;
using TiendaRecargas.Models;
using TiendaRecargas.Models.Enums;

namespace TiendaRecargas.Controllers
{
    public class LoginController : BaseController
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Index()
        {
            if (Logged != null)
            {
                if (Logged.Rol == RolesSistema.Administrador.ToString())
                {
                    return RedirectToAction(nameof(Index), "Home", null);
                }
                else
                {
                    return RedirectToAction(nameof(Index), "Recargas", null);
                }
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Salir()
        {
            Logof();
            return RedirectToAction(nameof(Index), null);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Index(Cuenta login)
        {
            try
            {
                var cuenta = await _context.RT_Cuentas.Where(x => x.Usuario == login.Usuario && x.Password == login.Password).FirstOrDefaultAsync();
                if (cuenta is null)
                {
                    ViewBag.Erro = "Usuario no cadastrado";
                }
                else if (!cuenta.Activo)
                {
                    ViewBag.Erro = "Su cuenta está desativada, contacte com suporte técnico.";
                }
                else
                {
                    await SignIn(cuenta);
                    if (cuenta.Rol == RolesSistema.Administrador.ToString())
                    {
                        return RedirectToAction(nameof(Index), "Home", null);
                    }
                    else
                    {
                        return RedirectToAction(nameof(Index), "Recargas", null);
                    }
                }
            }
            catch (Exception ex)
            {
                //ViewBag.Erro = "Error de conexión, contacte com suporte técnico.";
                ViewBag.Erro = ex.Message;
            }

            return View(login);
        }

        private bool CuentaExists(int id)
        {
            return _context.RT_Cuentas.Any(e => e.IdCuenta == id);
        }
    }
}
