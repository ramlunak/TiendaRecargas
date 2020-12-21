using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
    public class RecargasController : BaseController
    {
        private readonly AppDbContext _context;

        public RecargasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Recargas
        public async Task<IActionResult> Index()
        {
            IsLogged();

            var recarga = new Recarga();
            try
            {
                ViewBag.RecargasEnLista = await _context.RT_Recargas.Where(x => x.idCuenta == Logged.IdCuenta && x.status == RecargaStatus.en_lista).ToListAsync();

                if (GetSession<Recarga>("Recarga") != null)
                {
                    recarga = GetSession<Recarga>("Recarga");
                    if (recarga.numero is null && recarga.tipoRecarga == TipoRecarga.movil)
                        PrompErro("Ingrese el número");
                    if (recarga.nauta is null && recarga.tipoRecarga == TipoRecarga.nauta)
                        PrompErro("Ingrese el usuario");

                }
            }
            catch (Exception ex)
            {
                PrompErro(ex.Message);
            }

            return View(recarga);
        }

        public async Task<IActionResult> Historial()
        {
            IsLogged();
            var semana = DateTime.Now.GetSemana();
            var year = DateTime.Now.ToEasternStandardTime().Year;
            ViewBag.Semana = $"{year}-W{semana}";
            var model = await _context.RT_Recargas.Where(x => x.idCuenta == Logged.IdCuenta && x.status == RecargaStatus.success && x.year == year && x.semana == semana).ToListAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Historial(RecargaSearch filtro)
        {
            IsLogged();
            var model = await _context.RT_Recargas.Where(x => x.idCuenta == Logged.IdCuenta && x.status == RecargaStatus.success && x.year == filtro.year && x.semana == filtro.semana).ToListAsync();
            ViewBag.Semana = filtro.input;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetValores(string id)
        {
            var tipoRecarga = id.ToLower() == "movil" ? TipoRecarga.movil : TipoRecarga.nauta;
            try
            {
                var valores = await _context.RT_RecargaValores.Where(x => x.tipoRecarga == tipoRecarga).ToListAsync();
                var json = JsonConvert.SerializeObject(valores);
                return Ok(new { valores = json });
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        // GET: Recargas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recarga = await _context.RT_Recargas.Where(x => x.idCuenta == Logged.IdCuenta)
                .FirstOrDefaultAsync(m => m.id == id);
            if (recarga == null)
            {
                return NotFound();
            }

            return View(recarga);
        }


        // POST: Recargas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Recarga recarga)
        {

            IsLogged();
            try
            {
                var cuentaActiva = await ValidarCuentaActiva();
                if (!cuentaActiva)
                {
                    return RedirectToAction("Salir", "Login");
                }

                //Validar fondos
                var recargaValor = await _context.RT_RecargaValores.FirstOrDefaultAsync(x => x.id == recarga.idValorRecarga);
                recarga.valor = recargaValor.valor;

                var valorLista = await _context.RT_Recargas.Where(x => x.status == RecargaStatus.en_lista && x.idCuenta == Logged.IdCuenta).SumAsync(x => x.monto);
                var totalLisat = valorLista + recarga.GetMonto(Logged.Porciento);
                var fondos = await GetFondos();

                if (fondos < totalLisat)
                {
                    PrompErro("No tiene saldo suiciente para agregar esta recarga");
                    return RedirectToAction(nameof(Index));
                }

                if (recarga.nauta is not null && recarga.tipoRecarga == TipoRecarga.nauta)
                {
                    recarga.numero = recarga.nauta;
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        recarga.idCuenta = Logged.IdCuenta;
                        recarga.status = RecargaStatus.en_lista;

                        if (recarga.tipoRecarga == TipoRecarga.movil)
                        {
                            recarga.numero = "53" + recarga.numero;
                        }
                        else if (recarga.tipoRecarga == TipoRecarga.nauta)
                        {
                            recarga.numero = recarga.numero + "@nauta.com.cu";
                        }

                        _context.Add(recarga);

                        await _context.SaveChangesAsync();

                        SetSession("Recarga", "");

                        return RedirectToAction(nameof(Index));

                    }
                    catch (Exception ex)
                    {
                        PrompErro(ex.Message);
                    }

                }
                SetSession("Recarga", recarga);
            }
            catch (Exception ex)
            {
                PrompErro(ex.Message);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Recargas/Edit/5
        public async Task<IActionResult> RecargarLista()
        {

            IsLogged();
            var cuentaActiva = await ValidarCuentaActiva();
            if (!cuentaActiva)
            {
                return RedirectToAction("Salir", "Login");
            }

            //Validar fondos

            var listaRecargas = await _context.RT_Recargas.Where(x => x.status == RecargaStatus.en_lista && x.idCuenta == Logged.IdCuenta).ToListAsync();
            var totalLisat = listaRecargas.Sum(x => x.monto);
            var fondos = await GetFondos();

            if (fondos < totalLisat)
            {
                PrompErro("No tiene saldo suiciente, su crédito puede haber sido actualizado");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                foreach (var item in listaRecargas)
                {
                    var result = await Ding.SendTransfer(item);
                    item.TransactionDate = DateTime.Now.ToEasternStandardTime();
                    item.TransactionResultCode = result.ResultCode;
                    item.TransactionMsg = result.ErrorCodes != null && result.ErrorCodes.Length > 0 ? result.ErrorCodes.FirstOrDefault().Code : null;
                    if (Convert.ToInt32(result.ResultCode) > 2)
                    {
                        item.status = RecargaStatus.error;
                    }
                    else
                    {
                        item.status = RecargaStatus.success;
                    }
                    _context.RT_Recargas.Update(item);
                    await _context.SaveChangesAsync();
                }

                var Sumarbalance = listaRecargas.Where(x => x.status == RecargaStatus.success).Sum(x => x.monto);
                var cuenta = await _context.RT_Cuentas.FindAsync(Logged.IdCuenta);
                cuenta.Balance = cuenta.Balance + Sumarbalance;
                _context.RT_Cuentas.Update(cuenta);
                await _context.SaveChangesAsync();
                await GetFondos();

            }

            return RedirectToAction(nameof(Index));
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

            var recarga = await _context.RT_Recargas.FindAsync(id);
            _context.RT_Recargas.Remove(recarga);
            await _context.SaveChangesAsync();
            return Ok(true);
        }

        private bool RecargaExists(int id)
        {
            return _context.RT_Recargas.Any(e => e.id == id);
        }

        public async Task<decimal> GetFondos()
        {
            var cuenta = await _context.RT_Cuentas.FirstOrDefaultAsync(x => x.IdCuenta == Logged.IdCuenta);
            var fondos = cuenta.Credito - cuenta.Balance - cuenta.CreditoBloqueado;
            Response.Cookies.Delete("TiendaRecargas");
            SignIn(cuenta, false);
            return fondos;
        }

        public async Task<bool> ValidarCuentaActiva()
        {
            var cuenta = await _context.RT_Cuentas.FirstOrDefaultAsync(x => x.IdCuenta == Logged.IdCuenta);
            return cuenta.Activo;
        }

    }
}
