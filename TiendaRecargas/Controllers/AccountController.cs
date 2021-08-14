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
    public class AccountController : Controller
    {
        // GET: AccountController
        public async Task<ActionResult> Index(string nombre, int pagina = 1)
        {
            var cantidadRegistrosPorPagina = 10; // parámetro
            var skip = ((pagina - 1) * cantidadRegistrosPorPagina);

            var lista = await GetAccounList(skip, cantidadRegistrosPorPagina);

            ViewBag.FlrNombre = nombre;

            var modelo = new ViewModels.VMAccount();
            modelo.Accounts = lista.OrderBy(x => x.firstname).ToList();
            modelo.PaginaActual = pagina;
            modelo.TotalDeRegistros = 300;
            modelo.RegistrosPorPagina = cantidadRegistrosPorPagina;
            modelo.ValoresQueryString = new RouteValueDictionary();
            modelo.ValoresQueryString["pagina"] = pagina;
            modelo.ValoresQueryString["nombre"] = nombre;

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
        public async Task<ActionResult> Create(account_info account)
        {
           await SetAccount();
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AccountController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
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


        public async Task SetAccount()
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

            account.id = "a" + "31982185513";
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
            account.firstname = "royber";
            account.lastname = "arias";

            account.email = "test@gmail.com";
            account.login = "test@gmail.com";
            account.password = "test@gmail.com**,,";

            var valid = await ValidarCuenta(account);

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


        public async Task<bool> ValidarCuenta(account_info account)
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
                    var ErrorHandling = JsonConvert.DeserializeObject<ErrorHandling>(json);
                    if (ErrorHandling.faultstring is null)
                    {
                        var id = JsonConvert.DeserializeObject<AccountObject>(json).account_info.id;
                        if (id is null)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                       
                        return false;
                    }

                }
                catch(Exception ex)
                {

                    return false;
                }

            }

        }



    }
}
