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
        public ActionResult Create(IFormCollection collection)
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
    }
}
