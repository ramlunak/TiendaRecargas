using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TiendaRecargas.Controllers
{
    public class ConfigController : BaseController
    {
        // GET: ConfigController
        public ActionResult Index()
        {
            IsLogged();
            ViewBag.Cuenta = Logged;
            return View();
        }
    }
}
