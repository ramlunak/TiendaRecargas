using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TiendaRecargas.Class;
using TiendaRecargas.Extensions;
using TiendaRecargas.Models;
using static TiendaRecargas.Models.account_info;

namespace TiendaRecargas.Controllers
{
    public class AccountController : BaseController
    {
        // GET: AccountController
        public async Task<ActionResult> Index(string nombre, int pagina = 1)
        {
            var cantidadRegistrosPorPagina = 10; // parámetro
            var skip = ((pagina - 1) * cantidadRegistrosPorPagina);

            var lista = new List<account_info>();

            if (nombre != null)
            {
                lista = await GetAccounList(0, 1000);
                lista = lista.Where(x =>
                x.firstname.ToLower().Contains(nombre.ToLower()) ||
                (x.login != null && x.login.ToLower().Contains(nombre.ToLower())) ||
                (x.lastname != null && x.lastname.ToLower().Contains(nombre.ToLower()))
                                    ).ToList();
            }
            else
            {
                lista = await GetAccounList(skip, cantidadRegistrosPorPagina);
            }

            SetSession("ss_accounts", lista);

            var modelo = new ViewModels.VMAccount();
            modelo.Accounts = lista.OrderBy(x => x.firstname).ToList();
            modelo.PaginaActual = pagina;
            modelo.TotalDeRegistros = 300;
            modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
            modelo.ValoresQueryString = new RouteValueDictionary();
            modelo.ValoresQueryString["pagina"] = pagina;
            modelo.ValoresQueryString["nombre"] = nombre;

            ViewBag.FlrNombre = nombre;
            return View(modelo);

        }

        public async Task<List<account_info>> GetAccounList(int skip, int limit)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var security = new
                    {
                        login = "appuser",
                        password = "th89>)wam2020*"
                    };
                    var param = new
                    {
                        offset = skip,
                        limit = limit,
                        i_batch = 203968,
                        i_customer = 260271
                    };
                    var URL = "https://mybilling.willtech.us/rest/Account/get_account_list/" + security.AsJson() + "/" + param.AsJson();
                    var response = await client.GetAsync(URL);
                    var Result = await response.Content.ReadAsStringAsync();
                    var accountList = JsonConvert.DeserializeObject<GetAccountListResponse>(Result).account_list.ToList();
                    return accountList;
                }
                catch (Exception ex)
                {
                    return new List<account_info>();
                }
            }
        }

        // GET: AccountController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AccountController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccountController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AccountEditar account)
        {
            ErrorHandling errorHandling = null;

            if (ModelState.IsValid)
            {
                var newAccount = SetAccount(account);
                errorHandling = await ValidarCuenta(newAccount);

                if (errorHandling != null && errorHandling.faultcode != null)
                {
                    ViewBag.Error = errorHandling?.faultstring;
                    return View(account);
                }
                else
                {
                    await CreateCuenta(newAccount);
                }
            }
            else
            {
                return View(account);
            }

            return RedirectToAction(nameof(Index));
        }

        public account_info SetAccount(AccountEditar accountEditar)
        {
            var account = new account_info();
            var now = DateTime.Now;

            var YY = now;
            var MM = now.Month.ToString();
            var DD = now.Day.ToString();

            if (MM.Length == 1)
                MM = "0" + MM;
            if (DD.Length == 1)
                DD = "0" + DD;

            var activationDate = now.Year + "-" + MM + "-" + DD;

            account.id = "a" + accountEditar.Telefono;
            account.phone1 = accountEditar.Telefono;
            account.iso_4217 = "USD";
            account.i_customer = 260271;  //Online customers
            account.i_distributor = 282645;  //distributor  customers
            account.batch_name = "260271-di-pinless";
            account.country = "US";
            account.billing_model = -1;
            account.control_number = 1;
            account.h323_password = ServicePassword;
            account.i_product = 22791;
            account.activation_date = activationDate.Trim();
            account.firstname = accountEditar.Nombre;
            account.lastname = accountEditar.Apellido;

            account.balance = 0;
            account.email = accountEditar.Email;
            account.login = accountEditar.Email;
            account.password = "Acc7o2554unt**,,";
            return account;
        }

        public static string ServicePassword
        {
            get
            {
                var Password = "";

                var ran = new Random();
                var cadena = "abcdefghijqmnlopqrstuvwxyz".ToCharArray();
                var numeros = "0123456789".ToCharArray();

                for (int x = 0; x < 5; x++)
                {
                    var ranCadena = ran.Next(cadena.Length);
                    var ranNumero = ran.Next(numeros.Length);
                    var alpha = cadena[ranCadena].ToString();
                    var number = numeros[ranNumero].ToString();
                    Password += alpha;
                    Password += number;
                }
                return Password;
            }
        }

        public async Task<ErrorHandling> ValidarCuenta(account_info account)
        {

            using (HttpClient client = new HttpClient())
            {
                var URL = "";
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var security = new
                    {
                        login = "appuser",
                        password = "th89>)wam2020*"
                    };
                    var param = JsonConvert.SerializeObject(new { account_info = account });
                    URL = _Global.BaseUrlAdmin + _Global.Servicio.Account + "/" + _Global.Metodo.validate_account_info + "/" + security.AsJson() + "/" + param;

                    var response = await client.GetAsync(URL);
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ErrorHandling>(json);
                }
                catch (Exception ex)
                {
                    return default(ErrorHandling);
                }

            }

        }

        public async Task<ErrorHandling> CreateCuenta(account_info account)
        {

            using (HttpClient client = new HttpClient())
            {
                var URL = "";
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var security = new
                    {
                        login = "appuser",
                        password = "th89>)wam2020*"
                    };
                    var param = JsonConvert.SerializeObject(new { account_info = account });
                    URL = _Global.BaseUrlAdmin + _Global.Servicio.Account + "/" + _Global.Metodo.add_account + "/" + security.AsJson() + "/" + param;

                    var response = await client.GetAsync(URL);
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ErrorHandling>(json);
                }
                catch (Exception ex)
                {
                    return default(ErrorHandling);
                }

            }

        }

        // GET: AccountController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        public ActionResult Balance(int? id)
        {
            if (id is null)
            {
                return View();
            }
            var accounts = GetSession<List<account_info>>("ss_accounts");
            if (accounts is null)
            {
                return View();
            }
            var acc = accounts.Where(x => x.i_account == id).First();
            var editar = new EditarBalance();
            editar.i_account = acc.i_account;
            editar.fullname = acc.fullname;
            editar.balance = acc.balance;
            return View(editar);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Balance(EditarBalance editarBalance)
        {

            if (ModelState.IsValid)
            {
                ErrorHandling errorHandling = null;
                if (editarBalance.editar_balance > 0)
                {
                    errorHandling = await MakeTransaction__Manualcredit(editarBalance.i_account, editarBalance.editar_balance, "Manual Transaction(WEB)");

                }
                else
                {
                    errorHandling = await MakeTransaction_Manualcharge(editarBalance.i_account , (editarBalance.editar_balance * -1), "Manual Transaction(WEB)");
                }

                if (errorHandling != null && errorHandling.faultcode != null)
                {
                    ViewBag.Error = errorHandling?.faultstring;
                    return View(editarBalance);
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                return View(editarBalance);
            }
        }

        // POST: AccountController/Edit/5
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

        // GET: AccountController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AccountController/Delete/5
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


        public async Task<ErrorHandling> MakeTransaction_Manualcharge(int i_account, decimal monto, string visible_comment)
        {

            var MakeAccountTransactionRequest = new MakeAccountTransactionRequest();
            MakeAccountTransactionRequest.i_account = i_account;
            MakeAccountTransactionRequest.amount = Convert.ToDecimal(monto);
            MakeAccountTransactionRequest.action = "Manual charge";
            MakeAccountTransactionRequest.visible_comment = visible_comment;
            var MakeAccountTransactionResponse = new MakeAccountTransactionResponse();

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var security = new
                    {
                        login = "appuser",
                        password = "th89>)wam2020*"
                    };
                    var param = JsonConvert.SerializeObject(MakeAccountTransactionRequest);
                    var URL = _Global.BaseUrlAdmin + _Global.Servicio.Account + "/" + _Global.Metodo.make_transaction + "/" + security.AsJson() + "/" + param;

                    var response = await client.GetAsync(URL);
                    var Result = await response.Content.ReadAsStringAsync();
                    //var r = JsonConvert.DeserializeObject<MakeAccountTransactionResponse>(Result);
                    return JsonConvert.DeserializeObject<ErrorHandling>(Result);
                }
                catch (Exception ex)
                {
                    return default(ErrorHandling);
                }
            }

        }


        public async Task<ErrorHandling> MakeTransaction__Manualcredit(int i_account, decimal monto, string visible_comment)
        {
            var MakeAccountTransactionRequest = new MakeAccountTransactionRequest();

            MakeAccountTransactionRequest.i_account = i_account;
            MakeAccountTransactionRequest.amount = Convert.ToDecimal(monto);
            MakeAccountTransactionRequest.action = "Manual credit";
            MakeAccountTransactionRequest.visible_comment = visible_comment;

            var MakeAccountTransactionResponse = new MakeAccountTransactionResponse();

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var security = new
                    {
                        login = "appuser",
                        password = "th89>)wam2020*"
                    };
                    var param = JsonConvert.SerializeObject(MakeAccountTransactionRequest);
                    var URL = _Global.BaseUrlAdmin + _Global.Servicio.Account + "/" + _Global.Metodo.make_transaction + "/" + security.AsJson() + "/" + param;

                    var response = await client.GetAsync(URL);
                    var Result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<ErrorHandling>(Result);
                }
                catch (Exception ex)
                {
                    return default(ErrorHandling);
                }
            }

        }

    }
}
