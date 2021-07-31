using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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


        public static EPromo Promociones = new EPromo();

        public static class db
        {
            public static List<Esms> sms = new List<Esms>();
        }

        public static class phone
        {
            public static SQLiteAsyncConnection db
            {
                get
                {
                    var con = DependencyService.Get<ISQLiteDB>().GetConnection();
                    con.CreateTableAsync<SQ_Recarga>();
                    return con;
                }
            }

            public static List<EContacto> contactos = new List<EContacto>();

            public static async Task CargarContactos()
            {

                var ListaContactos = new List<EContacto>();
                var PhoneContactos = await Plugin.ContactService.CrossContactService.Current.GetContactListAsync();
                foreach (var item in PhoneContactos)
                {
                    var nauta = "";
                    if (item.Emails.Count > 0)
                    {
                        foreach (var con in item.Emails)
                        {
                            if (con.Contains("@nauta.com.cu"))
                            {
                                nauta = con;
                            }

                        }
                    }
                    else
                    {
                        if (item.Email != null)
                            if (item.Email.Contains("@nauta.com.cu"))
                                nauta = item.Email;
                    }

                    if (nauta != "")
                    {
                        var arr = nauta.Split('@');
                        nauta = arr[0].ToString();
                        ;
                    }
                    var contacto = new EContacto();
                    contacto.Nombre = item.Name;
                    contacto.Telefono = item.Number;
                    contacto.UserNauta = nauta;
                    ListaContactos.Add(contacto);
                }

                contactos = ListaContactos;
                _Global.ListaContactos = contactos;
            }

            public static async Task<List<Esms>> GetAllSms()
            {
                var con = DependencyService.Get<ISQLiteDB>().GetConnection();
                await con.CreateTableAsync<Esms>();
                return con.Table<Esms>().ToListAsync().Result;
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
                                                                                           // public static string MasterURL = "http://192.168.42.145/teleyuma/api/"; // IIS
                                                                                           // public static string MasterURL = "http://192.168.42.145:58723/api/"; // IIS


        //public static string MasterURL = "http://192.168.42.180/service/Service1.svc/"; // url anclaje
        // public static string MasterURL = "http://192.168.1.100/service/Service1.svc/"; // url emulador

        //public static string MasterURL = "http://smsteleyuma.azurewebsites.net/Service1.svc/"; // url nube

        public static ERecMovilConfig RecMovilConfig = new ERecMovilConfig();

        public static EPromocion Promocion = new EPromocion();

        public static string TipoRecarga = string.Empty;

        public static string AccionRecarga = string.Empty;

        public static EPais PaisSeleccionado = new EPais();

        public static EContacto ContactoSeleccionado = new EContacto();

        public static nautaInfo RecargaNauta = new nautaInfo();

        public static ListaRecarga ListaRecargas = new ListaRecarga();

        public static account_info CurrentAccount = new account_info();

        public static float MontoTransferenciaBancaria = 0;

        public static MakeAccountTransactionResponse TransactionResponse = new MakeAccountTransactionResponse();

        public static PaymentMethodObject CurrentCustomer_PaymentMethodObject = new PaymentMethodObject();

        public static Grupos ListaGrupos = new Grupos();

        public static List<GrupoSMS> GruposDeListasSMS = new List<GrupoSMS>();

        public static GrupoSMS GrupoSMS = new GrupoSMS();

        public static List<Esms> ListaSMS = new List<Esms>();

        public static List<EContacto> ListaContactos = new List<EContacto>();

        public static SQ_Login SQLiteLogin = new SQ_Login();

        public static List<SQ_Recarga> Recargas = new List<SQ_Recarga>();

        public static class VM
        {
            public static VMHome VMHome = new VMHome();

            public static VMInicio VMInicio = new VMInicio();

            public static VMMensaje VMMensaje = new VMMensaje();

            public static VMGrupos VMGrupos = new VMGrupos();

            public static VMRecargas VMRecargas = new VMRecargas();

            public static VMCompras VMCompras = new VMCompras();

            public static VMPagar VMPagar = new VMPagar();

            public static VMResumenRecarga VMResumenRecarga = new VMResumenRecarga();

            public static VMListaContactos VMListaContactos = new VMListaContactos();

        }

        public static class Vistas
        {

            public static PagesInicio.ConfirmarTelefono ConfirmarTelefono = new PagesInicio.ConfirmarTelefono();

            public static Pages.Pagar Pagar = new Pages.Pagar();

            public static Pages.TransferenciaBancaria TransferenciaBancaria = new Pages.TransferenciaBancaria();

            public static Pages.ListaRecargas ListaRecargas = new Pages.ListaRecargas();

            public static Pages.NewListaPaises PageNewListaPaises = new Pages.NewListaPaises();

            public static Pages.PhoneContacs PhoneContacs = new Pages.PhoneContacs();

            public static Pages.RespuestaRecarga RespuestaRecarga = new Pages.RespuestaRecarga();

            public static Contactos.ListaContactos ListaContactos = new Contactos.ListaContactos();

            public static Contactos.Llamar Llamar = new Contactos.Llamar();

            public static SMS.Grupos Grupos = new SMS.Grupos();

            public static SMS.Mensaje EnviarSMS = new SMS.Mensaje();

            public static SMS.NewSMS NewSMS = new SMS.NewSMS();
        }

        public async static Task<string> GetAuthInfoAdminJson()
        {
            var credenciales = await Get<Credenciales>("credenciales/2");

            var admin = new AuthInfo
            {
                session_id = credenciales.KeyGenerate
            };
            return JsonConvert.SerializeObject(admin);
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

        public static string GetAuthInfo(string login, string password)
        {

            var admin = new AuthInfo
            {
                login = login,
                password = password
            };
            return JsonConvert.SerializeObject(admin);
        }


        public static string BaseUrlAdmin = "https://mybilling.teleyuma.com/rest/";


        public static string BaseUrlCliente = "https://mybilling.teleyuma.com:8444/rest/";

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

        public static string GetMd5Hash(string input)
        {
            MD5 md5Hash = MD5.Create();
            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
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
