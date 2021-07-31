using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace TiendaRecargas.Models
{
    public class AccountObject
    {
        [DataMember]
        public account_info account_info { get; set; }
    }


    public class account_info
    {

        [DataMember]
        public int i_account { get; set; }
        [DataMember]

        public string id { get; set; }
        [DataMember]

        public int i_customer { get; set; }
        [DataMember]

        public int billing_model { get; set; }
        [DataMember]

        public string activation_date { get; set; }
        [DataMember]

        public int i_product { get; set; }
        [DataMember]

        public string baddr1 { get; set; }
        [DataMember]

        public string baddr2 { get; set; }
        [DataMember]

        public string AuthorizationOnlyTransaction_id
        {
            get
            {
                if (baddr3 == "" || baddr3 == null)
                    return "0";
                else
                    return baddr3.Replace(" ", "");
            }
            set
            {
                baddr3 = value;
            }
        }
        [DataMember]

        public string baddr3 { get; set; }
        [DataMember]

        public string baddr4 { get; set; }
        [DataMember]

        public string baddr5 { get; set; }
        [DataMember]

        public int i_distributor { get; set; }

        [DataMember]

        public string batch_name { get; set; }
        [DataMember]

        public int control_number { get; set; }
        [DataMember]
        public string bill_status { get; set; }
        [DataMember]

        public string iso_4217 { get; set; }
        [DataMember]

        public float opening_balance { get; set; }
        [DataMember]

        public decimal balance { get; set; }
        [DataMember]

        public decimal balance2
        {
            get
            {
                return _Global.Round_2(balance);
            }
        }

        [DataMember]

        public string login { get; set; }
        [DataMember]

        public string password { get; set; }
        [DataMember]

        public string firstname { get; set; }
        [DataMember]

        public string lastname { get; set; }

        [DataMember]

        public string fullname { get { return firstname + " " + lastname; } }

        [DataMember]

        public string iniciales
        {
            get
            {
                try
                {
                    var inicial_nombre = firstname.ToCharArray()[0].ToString().ToUpper();
                    var inicial_apellido = lastname.ToCharArray()[0].ToString().ToUpper();
                    return inicial_nombre + inicial_apellido;
                }
                catch
                {
                    return "";
                }

            }
        }
        [DataMember]

        public string cont1 { get; set; }
        [DataMember]

        public string phone1 { get; set; }
        [DataMember]

        public string phone2 { get; set; }
        [DataMember]

        public string email { get; set; }
        [DataMember]

        public string country { get; set; }
        [DataMember]

        public string h323_password { get; set; }
        [DataMember]

        public string ecommerce_enabled = "Y";
        [DataMember]

        public string blocked { get; set; }
        [DataMember]
        [DefaultValue("0")]
        public string cont2 { get; set; }

        //METODOS

        #region NUEVOS METODOS

        public async Task<MakeAccountTransactionResponse> New_MakeTransaction_AuthorizationOnly(decimal monto)
        {
            var AccountTransactionRequest = new MakeAccountTransactionRequest();

            AccountTransactionRequest.i_account = this.i_account;
            AccountTransactionRequest.amount = Convert.ToDecimal(monto);
            AccountTransactionRequest.action = "Authorization only";
            AccountTransactionRequest.visible_comment = "Authorization only";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var param = JsonConvert.SerializeObject(AccountTransactionRequest);
                    var URL = _Global.BaseUrlAdmin + _Global.Servicio.Account + "/" + _Global.Metodo.make_transaction + "/" + await _Global.GetAuthInfoAdminJson() + "/" + param;

                    var response = await client.GetAsync(URL);
                    var Result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<MakeAccountTransactionResponse>(Result);

                }
                catch
                {
                    return new MakeAccountTransactionResponse();
                }
            }

        }

        public async Task<MakeAccountTransactionResponse> New_MakeTransaction_Refund(decimal monto, string TransacAuthorizationOnlyTransaction_id)
        {
            var MakeAccountTransactionRequest = new MakeAccountTransactionRequest();
            MakeAccountTransactionRequest.i_account = this.i_account;
            MakeAccountTransactionRequest.amount = Convert.ToDecimal(monto);
            MakeAccountTransactionRequest.action = "E-commerce refund";
            MakeAccountTransactionRequest.visible_comment = "E-commerce refund";
            MakeAccountTransactionRequest.transaction_id = TransacAuthorizationOnlyTransaction_id;
            var MakeAccountTransactionResponse = new MakeAccountTransactionResponse();

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var param = JsonConvert.SerializeObject(MakeAccountTransactionRequest);
                    var URL = _Global.BaseUrlAdmin + _Global.Servicio.Account + "/" + _Global.Metodo.make_transaction + "/" + await _Global.GetAuthInfoAdminJson() + "/" + param;

                    var response = await client.GetAsync(URL);
                    var Result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<MakeAccountTransactionResponse>(Result);

                }
                catch
                {
                    return new MakeAccountTransactionResponse();
                }
            }

        }

        public async Task<MakeAccountTransactionResponse> New_MakeTransaction_CapturePayment(decimal monto, MakeAccountTransactionResponse TransactionResponse)
        {
            var MakeAccountTransactionRequest = new MakeAccountTransactionRequest();
            MakeAccountTransactionRequest.i_account = this.i_account;
            MakeAccountTransactionRequest.amount = Convert.ToDecimal(monto);
            MakeAccountTransactionRequest.action = "Capture payment";
            MakeAccountTransactionRequest.visible_comment = "Capture payment";
            MakeAccountTransactionRequest.transaction_id = TransactionResponse.transaction_id;
            var MakeAccountTransactionResponse = new MakeAccountTransactionResponse();

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var param = JsonConvert.SerializeObject(MakeAccountTransactionRequest);
                    var URL = _Global.BaseUrlAdmin + _Global.Servicio.Account + "/" + _Global.Metodo.make_transaction + "/" + await _Global.GetAuthInfoAdminJson() + "/" + param;

                    var response = await client.GetAsync(URL);
                    var Result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<MakeAccountTransactionResponse>(Result);

                }
                catch
                {
                    return new MakeAccountTransactionResponse();
                }
            }

        }

        public async Task<MakeAccountTransactionResponse> New_MakeTransaction_ECommercePayment(decimal monto)
        {
            var MakeAccountTransactionRequest = new MakeAccountTransactionRequest();
            MakeAccountTransactionRequest.i_account = this.i_account;
            MakeAccountTransactionRequest.amount = Convert.ToDecimal(monto);
            MakeAccountTransactionRequest.action = "E-commerce payment";
            MakeAccountTransactionRequest.visible_comment = "E-commerce payment";

            var MakeAccountTransactionResponse = new MakeAccountTransactionResponse();

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var param = JsonConvert.SerializeObject(MakeAccountTransactionRequest);
                    var URL = _Global.BaseUrlAdmin + _Global.Servicio.Account + "/" + _Global.Metodo.make_transaction + "/" + await _Global.GetAuthInfoAdminJson() + "/" + param;

                    var response = await client.GetAsync(URL);
                    var Result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<MakeAccountTransactionResponse>(Result);

                }
                catch
                {
                    return new MakeAccountTransactionResponse();
                }
            }

        }

        public async Task<bool> New_Actualizar(string capture = null)
        {

            if (capture != null)
                _Global.CurrentAccount.cont2 = capture;

            using (HttpClient client = new HttpClient())
            {
                var URL = "";
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var param = JsonConvert.SerializeObject(new { account_info = _Global.CurrentAccount });
                    URL = _Global.BaseUrlAdmin + _Global.Servicio.Account + "/" + _Global.Metodo.update_account + "/" + await _Global.GetAuthInfoAdminJson() + "/" + param;
                    var response = await client.GetAsync(URL);
                    var json = await response.Content.ReadAsStringAsync();
                    var ErrorHandling = JsonConvert.DeserializeObject<ErrorHandling>(json);
                    if (ErrorHandling.faultstring is null)
                    {
                        //_Global.Vistas.PageHome.ActualizarInformacionCuenta();
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                catch
                {
                    return false;
                }

            }
        }

        #endregion

        public async Task<account_info> GetAccountInfo()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    string param;
                    if (this.i_account == 0)
                        param = JsonConvert.SerializeObject(new { this.id });
                    else
                        param = JsonConvert.SerializeObject(new { this.i_account });

                    var URL = _Global.BaseUrlAdmin + _Global.Servicio.Account + "/" + _Global.Metodo.get_account_info + "/" + await _Global.GetAuthInfoAdminJson() + "/" + param;
                    var response = await client.GetAsync(URL);
                    var Result = await response.Content.ReadAsStringAsync();
                    var account = JsonConvert.DeserializeObject<AccountObject>(Result).account_info;
                    if (account != null)
                        if (account.id != "RecMovilConfig")
                            _Global.CurrentAccount = account;
                    return account;

                }
                catch (Exception ex)
                {

                }
                return this;
            }
        }

        public async Task<GetAccountXDRListResponse> GetAccountXDR(GetAccountXDRListRequest GetAccountXDRListRequest)
        {
            GetAccountXDRListRequest.i_account = this.i_account.ToString();

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var param = JsonConvert.SerializeObject(GetAccountXDRListRequest);
                    var URL = _Global.BaseUrlAdmin + _Global.Servicio.Account + "/" + _Global.Metodo.get_xdr_list + "/" + await _Global.GetAuthInfoAdminJson() + "/" + param;
                    var response = await client.GetAsync(URL);
                    var Result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<GetAccountXDRListResponse>(Result);

                }
                catch
                {

                }
                return new GetAccountXDRListResponse();
            }
        }

        public async Task<string> MakeTransaction_AuthorizationOnly(decimal monto)
        {
            var AccountTransactionRequest = new MakeAccountTransactionRequest();

            AccountTransactionRequest.i_account = this.i_account;
            AccountTransactionRequest.amount = Convert.ToDecimal(monto);
            AccountTransactionRequest.action = "Authorization only";
            AccountTransactionRequest.visible_comment = "Authorization only";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var param = JsonConvert.SerializeObject(AccountTransactionRequest);
                    var URL = _Global.BaseUrlAdmin + _Global.Servicio.Account + "/" + _Global.Metodo.make_transaction + "/" + await _Global.GetAuthInfoAdminJson() + "/" + param;

                    var response = await client.GetAsync(URL);
                    var Result = await response.Content.ReadAsStringAsync();
                    _Global.TransactionResponse = JsonConvert.DeserializeObject<MakeAccountTransactionResponse>(Result);
                    return Result;

                }
                catch
                {
                    return new MakeAccountTransactionResponse().AsJson();
                }
            }

        }

        public async Task<string> MakeTransaction_CapturePayment(decimal monto, MakeAccountTransactionResponse TransactionResponse)
        {
            var MakeAccountTransactionRequest = new MakeAccountTransactionRequest();
            MakeAccountTransactionRequest.i_account = this.i_account;
            MakeAccountTransactionRequest.amount = Convert.ToDecimal(monto);
            MakeAccountTransactionRequest.action = "Capture payment";
            MakeAccountTransactionRequest.visible_comment = "Capture payment";
            MakeAccountTransactionRequest.transaction_id = TransactionResponse.transaction_id;
            var MakeAccountTransactionResponse = new MakeAccountTransactionResponse();

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var param = JsonConvert.SerializeObject(MakeAccountTransactionRequest);
                    var URL = _Global.BaseUrlAdmin + _Global.Servicio.Account + "/" + _Global.Metodo.make_transaction + "/" + await _Global.GetAuthInfoAdminJson() + "/" + param;

                    var response = await client.GetAsync(URL);
                    var Result = await response.Content.ReadAsStringAsync();
                    _Global.TransactionResponse = JsonConvert.DeserializeObject<MakeAccountTransactionResponse>(Result);
                    return Result;

                }
                catch
                {
                    return new MakeAccountTransactionResponse().AsJson();
                }
            }

        }

        public async Task<string> MakeTransaction_EcommercePayment(double monto, string visible_comment)
        {
            var MakeAccountTransactionRequest = new MakeAccountTransactionRequest();

            MakeAccountTransactionRequest.i_account = this.i_account;
            MakeAccountTransactionRequest.amount = Convert.ToDecimal(monto);
            MakeAccountTransactionRequest.action = "E-commerce payment";
            MakeAccountTransactionRequest.visible_comment = visible_comment;

            var MakeAccountTransactionResponse = new MakeAccountTransactionResponse();

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {
                    var param = JsonConvert.SerializeObject(MakeAccountTransactionRequest);
                    var URL = _Global.BaseUrlAdmin + _Global.Servicio.Account + "/" + _Global.Metodo.make_transaction + "/" + await _Global.GetAuthInfoAdminJson() + "/" + param;

                    var response = await client.GetAsync(URL);
                    var Result = await response.Content.ReadAsStringAsync();
                    _Global.TransactionResponse = JsonConvert.DeserializeObject<MakeAccountTransactionResponse>(Result);
                    return Result;

                }
                catch
                {
                    return new MakeAccountTransactionResponse().AsJson();
                }
            }

        }

        public async Task<string> MakeTransaction_Manualcharge(decimal monto, string visible_comment)
        {

            var MakeAccountTransactionRequest = new MakeAccountTransactionRequest();
            MakeAccountTransactionRequest.i_account = this.i_account;
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
                    var param = JsonConvert.SerializeObject(MakeAccountTransactionRequest);
                    var URL = _Global.BaseUrlAdmin + _Global.Servicio.Account + "/" + _Global.Metodo.make_transaction + "/" + await _Global.GetAuthInfoAdminJson() + "/" + param;

                    var response = await client.GetAsync(URL);
                    var Result = await response.Content.ReadAsStringAsync();
                    _Global.TransactionResponse = JsonConvert.DeserializeObject<MakeAccountTransactionResponse>(Result);
                    return Result;

                }
                catch
                {
                    return new MakeAccountTransactionResponse().AsJson();
                }
            }

        }

        public async Task<bool> getAccountByLogin(string user)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

                try
                {
                    var param = JsonConvert.SerializeObject(new { login = user });
                    var URL = _Global.BaseUrlAdmin + _Global.Servicio.Account + "/" + _Global.Metodo.get_account_info + "/" + await _Global.GetAuthInfoAdminJson() + "/" + param;
                    var response = await client.GetStringAsync(URL);
                    var objet = JsonConvert.DeserializeObject<AccountObject>(response);
                    if (objet.account_info != null)
                    {
                        _Global.CurrentAccount = objet.account_info;
                        return true;
                    }
                    else
                        return false;

                }
                catch
                {
                    return false;
                }

            }
        }


        public class GetAccountXDRListRequest
        {

            [DataMember]
            public string i_account { get; set; }
            [DataMember]
            public int i_service { get; set; }
            [DataMember]
            public string from_date { get; set; }
            [DataMember]
            public string to_date { get; set; }

        }

        public class GetAccountXDRListResponse
        {
            [DataMember]
            public XDRInfo[] xdr_list { get; set; }
            [DataMember]
            public int total { get; set; }
        }

        public class XDRInfo
        {
            [DataMember]
            public string i_service { get; set; }
            [DataMember]
            public string subdivision { get; set; }
            [DataMember]
            public string disconnect_reason { get; set; }
            [DataMember]
            public string i_xdr { get; set; }
            [DataMember]
            public string CLD { get; set; }
            [DataMember]
            public string call_recording_server_url { get; set; }
            [DataMember]
            public string connect_time { get; set; }
            [DataMember]
            public string CLI { get; set; }
            [DataMember]
            public float charged_amount { get; set; }
            [DataMember]
            public decimal charged_amount2
            {
                get
                {
                    var value = decimal.Round(Convert.ToDecimal(charged_amount), 2);
                    if (value < 0)
                        return (value * -1);
                    return value;
                }
            }
            [DataMember]
            public string bill_time { get; set; }
            [DataMember]
            public string bit_flags { get; set; }
            [DataMember]
            public string unix_connect_time { get; set; }
            [DataMember]
            public string country { get; set; }
            [DataMember]
            public string bill_status { get; set; }
            [DataMember]
            public string description { get; set; }
            [DataMember]
            public string account_id { get; set; }
            [DataMember]
            public string unix_disconnect_time { get; set; }
            [DataMember]
            public string disconnect_cause { get; set; }
            [DataMember]
            public string charged_quantity { get; set; }
            [DataMember]
            public string call_recording_url { get; set; }
            [DataMember]
            public string disconnect_time { get; set; }

        }


        public class MakeAccountTransactionRequest
        {
            [DataMember]
            public int i_account { get; set; }
            [DataMember]
            public string action { get; set; }
            [DataMember]
            public decimal amount { get; set; }
            [DataMember]
            public string visible_comment { get; set; }
            [DataMember]
            public string transaction_id { get; set; }

        }


        public class MakeAccountTransactionResponse
        {

            [DataMember]
            public string i_payment_transaction { get; set; }
            [DataMember]
            public float balance { get; set; }
            [DataMember]
            public string transaction_id { get; set; }

            [DataMember]
            public string authorization { get; set; }
            [DataMember]
            public string result_code { get; set; }
            [DataMember]
            public string i_xdr { get; set; }

        }


    }

}
