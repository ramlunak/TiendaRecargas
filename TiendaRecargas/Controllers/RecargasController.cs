using System;
using System.Collections.Generic;
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
    [Authorize(Roles = "Vendedor,Subvendedor")]
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
            var recargasEnLista = new List<Recarga>();
            try
            {
                recargasEnLista = await _context.RT_Recargas.Where(x => x.idCuenta == Logged.IdCuenta && (x.status == RecargaStatus.en_lista || x.status == RecargaStatus.error) && x.activo).ToListAsync();
                ViewBag.RecargasEnLista = recargasEnLista;

                var numeroSemana = DateTime.Now.GetSemana();
                var year = DateTime.Now.ToEasternStandardTime().Year;
                var semana = $"{year}-W{numeroSemana}";
                if (numeroSemana < 10)
                {
                    semana = $"{year}-W0{numeroSemana}";
                }
                var promo = await _context.RT_Promociones.Where(x => x.semana == semana && x.activo).FirstOrDefaultAsync();

                if (promo != null)
                {
                    ViewBag.Promocion = promo.texto;
                    ViewBag.Bono = promo.bono;
                }

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

            if (recargasEnLista.Any() && recargasEnLista.Where(x => x.status == RecargaStatus.error).Any())
            {
                ViewBag.RecargasConError = true;
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

            if (model.Any())
            {
                ViewBag.TotalPagado = model.Sum(x => x.monto);
                ViewBag.TotalRecibido = model.Sum(x => x.recibe);
            }

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

        public async Task<IActionResult> Facturacion()
        {
            IsLogged();
            var semana = DateTime.Now.GetSemana();
            var year = DateTime.Now.ToEasternStandardTime().Year;
            ViewBag.Semana = $"{year}-W{semana}";

            var model = new Facturacion();

            var recargas = await _context.RT_Recargas.Where(x => x.idCuenta == Logged.IdCuenta && x.status == RecargaStatus.success && x.year == year && x.semana == semana).ToListAsync();

            model.Recargas = recargas;

            var cuentas = await _context.RT_Cuentas.Where(x => x.IdCuentaPadre == Logged.IdCuenta).ToListAsync();
            model.cuentas = cuentas;

            foreach (var item in model.cuentas)
            {
                item.Recargas = await _context.RT_Recargas.Where(x => x.idCuenta == item.IdCuenta && x.status == RecargaStatus.success && x.year == year && x.semana == semana).ToListAsync();
            }


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Facturacion(RecargaSearch filtro)
        {
            IsLogged();
            var semana = filtro.semana;
            var year = filtro.year;
            ViewBag.Semana = filtro.input;

            var model = new Facturacion();

            var recargas = await _context.RT_Recargas.Where(x => x.idCuenta == Logged.IdCuenta && x.status == RecargaStatus.success && x.year == year && x.semana == semana).ToListAsync();

            model.Recargas = recargas;

            var cuentas = await _context.RT_Cuentas.Where(x => x.IdCuentaPadre == Logged.IdCuenta).ToListAsync();
            model.cuentas = cuentas;

            foreach (var item in model.cuentas)
            {
                item.Recargas = await _context.RT_Recargas.Where(x => x.idCuenta == item.IdCuenta && x.status == RecargaStatus.success && x.year == year && x.semana == semana).ToListAsync();
            }

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

                var configuracion = await _context.RT_Configuracion.ToListAsync();
                var tasaCambioCUP = configuracion.FirstOrDefault().tasaCambioCUP;

                var RecargasEnLista = await _context.RT_Recargas.Where(x => x.idCuenta == Logged.IdCuenta && (x.status == RecargaStatus.en_lista || x.status == RecargaStatus.error) && x.activo).ToListAsync();

                if (RecargasEnLista.Count() >= 3)
                {
                    PrompErro("Solo puede enviar 3 recargas por vez.");
                    return RedirectToAction(nameof(Index));
                }

                //Validar fondos
                var recargaValor = await _context.RT_RecargaValores.FirstOrDefaultAsync(x => x.id == recarga.idValorRecarga);
                recarga.valor = recargaValor.valor;
                recarga.recibe = recargaValor.valor * tasaCambioCUP;

                var valorLista = RecargasEnLista.Where(x => x.status == RecargaStatus.en_lista && x.idCuenta == Logged.IdCuenta).Sum(x => x.monto);
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
                    ModelState["Numero"].ValidationState = ModelValidationState.Valid;
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        recarga.idCuenta = Logged.IdCuenta;
                        recarga.status = RecargaStatus.en_lista;
                        recarga.activo = true;

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

            var listaRecargas = await _context.RT_Recargas.Where(x => (x.status == RecargaStatus.en_lista || x.status == RecargaStatus.error) && x.activo && x.idCuenta == Logged.IdCuenta).ToListAsync();
            var totalLisat = listaRecargas.Sum(x => x.monto);
            var fondos = await GetFondos();

            if (fondos < totalLisat)
            {
                PrompErro("No tiene saldo suiciente, su crédito puede haber sido actualizado");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                string ding_token = "";
                try
                {
                    var credenciales = await _context.RT_Credenciales.ToListAsync();
                    var dingPeovvedor = credenciales.First(x => x.codigo == "ding");
                    ding_token = dingPeovvedor.token == null ? string.Empty : dingPeovvedor.token;

                }
                catch (Exception ex)
                {
                    PrompErro("Error de credenciales del proveedor, contacte con soporte técnico");
                    return RedirectToAction(nameof(Index));
                }

                foreach (var item in listaRecargas)
                {
                    var result = await Ding.SendTransfer(item, ding_token);

                    //BORRAR >>>>
                    if (item.simularErro)
                    {
                        //BORRAR >>>>
                        result.ResultCode = "3";//SOLO PARA PODER INSERTER EN SIMULACIO
                    }
                    else
                    {
                        //BORRAR >>>>
                        result.ResultCode = "1";//SOLO PARA PODER INSERTER EN SIMULACIO
                    }

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
        public async Task<IActionResult> Edit(int id, Recarga recarga)
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
            if (recarga.status == RecargaStatus.error)
            {
                recarga.activo = false;
                _context.RT_Recargas.Update(recarga);
            }
            else
            {
                _context.RT_Recargas.Remove(recarga);
            }
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
