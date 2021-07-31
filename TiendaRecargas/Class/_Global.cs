using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading.Tasks;
using TiendaRecargas.Extensions;
using TiendaRecargas.Models;

namespace TiendaRecargas.Class
{
    public enum TipoTransaction
    {
        New,
        Edit,
        Select,
        Llamar
    }

    public enum EstadoReserva
    {
        Espera,
        Error,
        Recargado

    }

    public enum EstadoCompra
    {
        Espera,
        Error,
        Completada

    }

    public enum EstadoTarjeta
    {
        IntentoFallido,
        TarjetaComprovada,
        CuentaBloqueada
    }

    public enum SMSImageInfo
    {
        ic_check_ok_18pt_3x,
        ic_error_outline_red_18pt_3x

    }

    public enum Proveedores
    {
        telinta,
        innoverit,
        ding
    }

    public enum DingProducto
    {
        Movil,
        Nauta
    }

    public static class _Global
    {

        public static bool ModoPrueba = true;

        [DefaultValue(false)]
        public static bool IsOpen { get; set; }

        public static async Task<T> Get<T>(string url)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("apiKey", "a6V9NPooCNWzGaaEMsvPvQ==");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

                try
                {
                    var response = await client.GetAsync(MasterURL + url);
                    var result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(result);
                }
                catch (Exception ex)
                {
                    return default(T);
                }
            }
        }

        public static async Task<T> Post<T>(string url, object entity)
        {

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("apiKey", "a6V9NPooCNWzGaaEMsvPvQ==");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
                try
                {
                    var response = await client.PostAsync(MasterURL + url, entity.AsJsonStringContent());
                    var result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<T>(result);
                }
                catch (Exception ex)
                {
                    return default(T);
                }
            }
        }

        public static string StaticCaptureMonto { get; set; }

        public static bool CargarSmsNew = true;

        public static int IdSmsFromNoti = 0;

        public static bool ShowNitificasion = false;

        [DefaultValue(false)]
        public static bool RunTask { get; set; }

        [DefaultValue(true)]
        public static bool IsInternet { get; set; }

        private static decimal _CaptureMonto { get; set; }
        public static decimal CaptureMonto
        {
            get
            {
                _CaptureMonto = 0;
                try
                {
                    if (CurrentAccount.cont2 == "" || CurrentAccount.cont2 == null)
                        CurrentAccount.cont2 = "0";
                    _CaptureMonto = Convert.ToDecimal(CurrentAccount.cont2);
                }
                catch (Exception ex)
                {

                    ;
                }

                return _CaptureMonto;
            }
            set { _CaptureMonto = value; }
        }

        public static string MasterURL = "https://teleyumarestapi.azurewebsites.net/api/"; // IIS      
              

        //public static string MasterURL = "http://192.168.42.180/service/Service1.svc/"; // url anclaje
        // public static string MasterURL = "http://192.168.1.100/service/Service1.svc/"; // url emulador

        //public static string MasterURL = "http://smsteleyuma.azurewebsites.net/Service1.svc/"; // url nube

        public static string TipoRecarga = string.Empty;

        public static string AccionRecarga = string.Empty;


        public static account_info CurrentAccount = new account_info();

        public static float MontoTransferenciaBancaria = 0;

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

        public static string CodigoVerificacion
        {
            get
            {
                var Codigo = "";

                var ran = new Random();
                var numeros = "123456789".ToCharArray();
                for (int x = 0; x < 4; x++)
                {
                    var ranNumero = ran.Next(numeros.Length);
                    var number = numeros[ranNumero].ToString();
                    Codigo += number;
                }
                return Codigo;
            }

        }


        public static int UltimoRandomIcon = 0;

        public static string RandomIcon
        {
            get
            {

                var ran = new Random();
                string[] iconos = new string[12];
                iconos[0] = "Picture1";
                iconos[1] = "Picture2";
                iconos[2] = "Picture3";
                iconos[3] = "Picture4";
                iconos[4] = "Picture5";
                iconos[5] = "Picture6";
                iconos[6] = "Picture7";
                iconos[7] = "Picture8";
                iconos[8] = "Picture9";
                iconos[9] = "Picture10";
                iconos[10] = "Picture11";

                var ranNumero = ran.Next(1, 11);

            NextRandom:

                if (ranNumero == UltimoRandomIcon)
                {
                    ranNumero = ran.Next(0, 10);
                    goto NextRandom;
                }

                UltimoRandomIcon = ranNumero;


                return iconos[ranNumero];

            }

        }

        public static string BaseUrlAdmin = "https://mybilling.willtech.us/rest/";


        public static string BaseUrlCliente = "https://mybilling.willtech.us:8444/rest/";

        public class Servicio
        {
            public const string Session = "Session";
            public const string Customer = "Customer";
            public const string Account = "Account";
        }

        public class Metodo
        {

            #region  Session

            public const string login = "login";

            #endregion

            #region  Customer

            public const string get_customer_info = "get_customer_info";
            public const string get_customer_list = "get_customer_list";
            public const string add_customer = "add_customer";
            public const string make_transaction = "make_transaction";
            public const string get_payment_method_info = "get_payment_method_info";
            public const string update_payment_method = "update_payment_method";
            public const string update_service_features = "update_service_features";

            #endregion

            #region  Account
            public const string get_account_info = "get_account_info";
            public const string get_account_list = "get_account_list";
            public const string add_account = "add_account";
            public const string update_account = "update_account";
            public const string validate_account_info = "validate_account_info";
            public const string get_xdr_list = "get_xdr_list";
            #endregion

        }

        internal static Task<string> GetAuthInfoAdminJson()
        {
            throw new NotImplementedException();
        }

        public static HttpRequestMessage GetHttpRequestMessage(string url, HttpMethod method = null, HttpContent content = null)
        {
            string api_key = "9f95e245-4ee9-417c-aecb-8afbde62680c";
            string api_secret = "b9900c85-cd83-4a19-a2b4-292e537288a0";

            int epoch = (int)(DateTime.UtcNow - new DateTime(1980, 1, 1)).TotalSeconds;
            string nonce = epoch.ToString();
            string message = api_key + nonce;

            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(api_secret);
            HMACSHA256 hmac = new HMACSHA256(keyByte);
            byte[] messageBytes = encoding.GetBytes(message);
            byte[] hashmessage = hmac.ComputeHash(messageBytes);

            string hmac_base64 = Convert.ToBase64String(hashmessage);
            Console.WriteLine("Hash Base64 code is " + hmac_base64);
            if (method == null)
                method = HttpMethod.Get;
            HttpRequestMessage request = new HttpRequestMessage(method, url);
            request.Headers.Add("X-TransferTo-apikey", api_key);
            request.Headers.Add("X-TransferTo-nonce", nonce);
            request.Headers.Add("X-TransferTo-hmac", hmac_base64);
            if (content != null)
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                request.Content = content;
            }
            return request;
        }

        public static string GetDateFormat_YYMMDD(DateTime DateTime, string Hora = "inicio")
        {
            var YY = DateTime.Year.ToString();
            var MM = DateTime.Month.ToString();
            var DD = DateTime.Day.ToString();

            if (MM.Length == 1)
                MM = "0" + MM;
            if (DD.Length == 1)
                DD = "0" + DD;

            if (Hora == "inicio")
                return YY + "-" + MM + "-" + DD + " 00:00:00";
            else
                return YY + "-" + MM + "-" + DD + " 23:59:59";
        }

        public static decimal Round_2(object value)
        {
            //NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            //nfi.NumberDecimalSeparator = ".";
            var round = decimal.Round(Convert.ToDecimal(value, CultureInfo.CreateSpecificCulture("en-US")), 2);
            return round;
        }

    }
}
