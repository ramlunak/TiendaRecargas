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
    public class CuentaController : BaseController
    {
        private readonly AppDbContext _context;

        public CuentaController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Cuenta
        public async Task<IActionResult> Index()
        {
            IsLogged();
            try
            {
                var model = await _context.RT_Cuentas.Where(x => x.IdCuentaPadre == Logged.IdCuenta).ToListAsync();
                return View(model.Where(x => x.Rol != "Administrador"));
            }
            catch (Exception ex)
            {
                ;
            }
            return View(new List<Cuenta>());
        }

        // GET: Cuenta/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            IsLogged();
            if (id == null)
            {
                return NotFound();
            }

            var cuenta = await _context.RT_Cuentas
                .FirstOrDefaultAsync(m => m.IdCuenta == id);
            if (cuenta == null)
            {
                return NotFound();
            }

            return View(cuenta);
        }

        // GET: Cuenta/Create
        public IActionResult Create()
        {
            IsLogged();
            if (Logged.Rol != RolesSistema.Administrador.ToString())
                if (Logged.Fondos < 100)
                {
                    PrompInfo("Para crear una cuenta asociada usted deve tener mas de  $100.00 USD de fondos");
                    return RedirectToAction(nameof(Index));
                }
            return View();
        }

        // POST: Cuenta/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cuenta cuenta)
        {
            IsLogged();


            //Validaciones para crear sub cuentas
            if (Logged.Rol != RolesSistema.Administrador.ToString())
            {
                if (cuenta.Credito < 100)
                {
                    PrompInfo($"El crédito debe ser igual o mayor que $100.00 USD");
                    return View(cuenta);
                }
                
            }

            //Validaciones para crear sub cuentas
            if (Logged.Rol != RolesSistema.Administrador.ToString())
            {
                if (cuenta.Porciento < Logged.Porciento)
                {
                    PrompInfo($"El porciento debe ser igual o mayor que {Logged.Porciento.ToString("F2")}");
                    return View(cuenta);
                }
              
            }


            cuenta.IdCuentaPadre = Logged.IdCuenta;
            cuenta.Activo = true;
            cuenta.Rol = RolesSistema.Vendedor.ToString();

            if (ModelState.IsValid)
            {
                if (CuentaExistsByUsuario(cuenta.IdCuenta, cuenta.Usuario))
                {
                    PrompInfo("El nombre de usuario no está disponoble, por favor ingrese otro nombre usuario");
                    return View(cuenta);
                }

                _context.Add(cuenta);
                try
                {
                    await _context.SaveChangesAsync();

                    if (Logged.Rol != RolesSistema.Administrador.ToString())
                    {
                        var padre = await _context.RT_Cuentas.FindAsync(Logged.IdCuenta);
                        padre.CreditoBloqueado += cuenta.Credito;
                        _context.Update(padre);
                        await _context.SaveChangesAsync();
                        await GetFondos();
                    }

                }
                catch (Exception ex)
                {
                    NotifyError(MensajeError.ErroServidor);
                    return View(cuenta);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                //var errorList = (from item in ModelState
                //                 where item.Value.Errors.Any()
                //                 select item.Value.Errors[0].ErrorMessage).ToList();
                //NotifyError(String.Join("<br/>",errorList));
                return View(cuenta);
            }

        }

        // GET: Cuenta/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            IsLogged();
            if (id == null)
            {
                return NotFound();
            }

            var cuenta = await _context.RT_Cuentas.FindAsync(id);
            if (cuenta == null)
            {
                return NotFound();
            }
            return View(cuenta);
        }

        // POST: Cuenta/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cuenta cuenta)
        {
            IsLogged();
            if (id != cuenta.IdCuenta)
            {
                PrompErro(MensajeError.ErroServidor);
                return View(cuenta);
            }

            if (ModelState.IsValid)
            {
                if (CuentaExistsByUsuario(cuenta.IdCuenta, cuenta.Usuario))
                {
                    PrompErro("El nombre de usuario no está disponoble, por favor ingrese otro nombre usuario");
                    return View(cuenta);
                }

                try
                {
                    _context.Update(cuenta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CuentaExists(cuenta.IdCuenta))
                    {
                        PrompErro("Cuenta con identificador repetido, entre en contacto con soporte técnico.");
                        return View(cuenta);
                    }
                    else
                    {
                        NotifyError(MensajeError.ErroServidor);
                        return View(cuenta);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(cuenta);
        }

        // GET: Cuenta/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            IsLogged();
            if (id == null)
            {
                return NotFound();
            }

            var cuenta = await _context.RT_Cuentas
                .FirstOrDefaultAsync(m => m.IdCuenta == id);
            if (cuenta == null)
            {
                return NotFound();
            }

            return View(cuenta);
        }

        // POST: Cuenta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            IsLogged();
            var cuenta = await _context.RT_Cuentas.FindAsync(id);
            _context.RT_Cuentas.Remove(cuenta);
            await _context.SaveChangesAsync();
            
            if (Logged.Rol != RolesSistema.Administrador.ToString())
            {
                var padre = await _context.RT_Cuentas.FindAsync(Logged.IdCuenta);
                padre.CreditoBloqueado -= cuenta.Credito;
                _context.Update(padre);
                await _context.SaveChangesAsync();
                await GetFondos();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Cuenta/Edit/5
        public async Task<IActionResult> UpdatePassword(int id, string usuario)
        {
            IsLogged();
            var cuentaUpdatePassword = new CuentaUpdatePassword();
            cuentaUpdatePassword.idCuenta = id;
            cuentaUpdatePassword.Usuario = usuario;
            return View(cuentaUpdatePassword);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePassword(CuentaUpdatePassword cuentaUpdatePassword)
        {
            IsLogged();
            var cuenta = await _context.RT_Cuentas.FindAsync(cuentaUpdatePassword.idCuenta);
            if (cuenta == null)
            {
                PrompErro("No se pudo actualizar la contraseña, entre en contacto con soporte técnico.");
                return View(cuentaUpdatePassword);
            }
            try
            {
                cuenta.Password = cuentaUpdatePassword.Password;
                _context.RT_Cuentas.Update(cuenta);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                NotifyError(ex.ToString());
                return View(cuentaUpdatePassword);
            }
            NotifySuccess($"La contraseña ha sido actualizada para el usuario {cuentaUpdatePassword.Usuario}.");
            return RedirectToAction(nameof(Index));
        }

        private bool CuentaExists(int id)
        {
            IsLogged();
            if (Logged.Rol == RolesSistema.Administrador.ToString())
            {
                return _context.RT_Cuentas.Any(e => e.IdCuenta == id);
            }
            else
            {
                return _context.RT_Cuentas.Any(e => e.IdCuenta == id && e.IdCuentaPadre == Logged.IdCuenta);
            }

        }

        private bool CuentaExistsByUsuario(int idCuenta, string usuario)
        {
            IsLogged();
            if (idCuenta == 0)
            {
                return _context.RT_Cuentas.Any(e => e.Usuario == usuario);
            }
            else
            {
                return _context.RT_Cuentas.Any(e => e.Usuario == usuario && e.IdCuenta != idCuenta);
            }
        }

        public async Task<decimal> GetFondos()
        {
            var cuenta = await _context.RT_Cuentas.FirstOrDefaultAsync(x => x.IdCuenta == Logged.IdCuenta);
            var fondos = cuenta.Credito - cuenta.Balance - cuenta.CreditoBloqueado;
            Response.Cookies.Delete("TiendaRecargas");
            SignIn(cuenta, false);
            return fondos;
        }

    }
}
