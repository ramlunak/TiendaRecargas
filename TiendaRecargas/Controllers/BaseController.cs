using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TiendaRecargas.Models;
using TiendaRecargas.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using TiendaRecargas.Provedores;
using System.Globalization;

namespace TiendaRecargas.Controllers
{
    public class BaseController : Controller
    {
        public async Task SignIn(Cuenta logged, bool logof = true)
        {
            if (logof)
            {
                Logof();
            }

            try
            {
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim("logged", JsonConvert.SerializeObject(logged)));
                claims.Add(new Claim(ClaimTypes.Role, logged.Rol));
                ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                var authProperties = new AuthenticationProperties
                {
                    //AllowRefresh = <bool>,
                    // Refreshing the authentication session should be allowed.

                    //ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1),
                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.

                    IsPersistent = true,
                    // Whether the authentication session is persisted across 
                    // multiple requests. When used with cookies, controls
                    // whether the cookie's lifetime is absolute (matching the
                    // lifetime of the authentication ticket) or session-based.

                    //IssuedUtc = <DateTimeOffset>,
                    // The time at which the authentication ticket was issued.

                    //RedirectUri = <string>
                    // The full path or absolute URI to be used as an http 
                    // redirect response value.
                };

                await HttpContext.SignInAsync(principal, authProperties);
            }
            catch (Exception)
            {

            }
        }

        public void Logof()
        {
            try
            {
                Response.Cookies.Delete("TiendaRecargas");
                HttpContext.Session.Clear();
            }
            catch (Exception)
            {

            }
        }

        public void IsLogged()
        {
            if (Logged is null || !Logged.Activo)
            {
                Logof();
                Response.Redirect("/Login");
            }
            try
            {
                try
                {
                    TempData["Email"] = Logged.Email;
                    TempData["Simulate"] = Ding.simulate;
                    TempData["Fondos"] = (Logged.Credito - Logged.Balance - Logged.CreditoBloqueado).ToString("F2");
                }
                catch
                {
                    Logof();
                    Response.Redirect("/Login");
                }
            }
            catch (Exception ex)
            {
                Response.Redirect("/Login");
            }
        }

        public void SessionClear()
        {
            HttpContext.Session.Clear();
        }

        public void CookiesClear()
        {
            Response.Cookies.Delete("TiendaRecargas");
        }

        public void SetSession(string key, string value)
        {
            HttpContext.Session.SetString(key, value);
        }

        public void SetSession(string key, object o)
        {
            var json = JsonConvert.SerializeObject(o);
            HttpContext.Session.SetString(key, json);
        }

        public string GetSession(string key)
        {
            return string.IsNullOrEmpty(HttpContext.Session.GetString(key)) ? null : HttpContext.Session.GetString(key);
        }

        public T GetSession<T>(string key)
        {
            try
            {
                var value = string.IsNullOrEmpty(HttpContext.Session.GetString(key)) ? null : HttpContext.Session.GetString(key);
                if (value is null) return default(T);
                return JsonConvert.DeserializeObject<T>(value);
            }
            catch (Exception ex)
            {
                ;
                return default;
            }
        }

        public Cuenta Logged
        {
            get
            {
                var logged = new Cuenta();
                string json = null;
                try
                {
                    json = User.Claims.First(x => x.Type == "logged").Value;
                }
                catch
                {

                }

                if (json != null)
                {
                    var cuenta = JsonConvert.DeserializeObject<Cuenta>(json);
                    TempData["Email"] = cuenta.Usuario;
                    TempData["Simulate"] = Ding.simulate;
                    TempData["Fondos"] = (cuenta.Credito - cuenta.Balance - cuenta.CreditoBloqueado).ToString("F2");
                    return cuenta;
                }
                else
                {
                    return null;
                }

            }
        }

        public void NotifySuccess(string message)
        {
            var msg = new
            {
                message = message,
                icon = NotificationType.success.ToString(),
                type = NotificationType.success.ToString(),
                provider = "sweetAlertNotify"
            };

            TempData["Message"] = JsonConvert.SerializeObject(msg);
        }

        public void NotifyError(string message)
        {
            var msg = new
            {
                message = message,
                icon = NotificationType.error.ToString(),
                type = NotificationType.error.ToString(),
                provider = "sweetAlertNotify"
            };

            TempData["Message"] = JsonConvert.SerializeObject(msg);
        }

        public void Notifywarning(string message)
        {
            var msg = new
            {
                message = message,
                icon = NotificationType.warning.ToString(),
                type = NotificationType.warning.ToString(),
                provider = "sweetAlertNotify"
            };

            TempData["Message"] = JsonConvert.SerializeObject(msg);
        }

        public void PrompErro(string message)
        {
            var msg = new
            {
                message = message,
                icon = NotificationType.error.ToString(),
                type = NotificationType.error.ToString(),
                provider = "sweetAlertPromp"
            };

            TempData["Message"] = JsonConvert.SerializeObject(msg);
        }



        public void PrompInfo(string message)
        {
            var msg = new
            {
                message = message,
                icon = NotificationType.info.ToString(),
                type = NotificationType.info.ToString(),
                provider = "sweetAlertPromp"
            };

            TempData["Message"] = JsonConvert.SerializeObject(msg);
        }

        //private string GetProvider()
        //{
        //    var builder = new ConfigurationBuilder()
        //                    .SetBasePath(Directory.GetCurrentDirectory())
        //                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //                    .AddEnvironmentVariables();

        //    IConfigurationRoot configuration = builder.Build();

        //    var value = configuration["NotificationProvider"];

        //    return value;
        //}
    }
}
