using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using TiendaRecargas.Models;
using TiendaRecargas.Models.Enums;

namespace TiendaRecargas.Provedores
{
    public class Ding
    {
        public static bool simulate { get; set; } = false;

        public class CountryIso
        {
            [DataMember]
            public string countryIso { get; set; }
        }

        public static async Task<GetProductsResponse> GetProductsBycountryIso(string iso, string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("api_key", token);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var url2 = "https://api.dingconnect.com/api/V1/GetProducts" + "?countryIsos=" + iso;
                    var response = await client.GetAsync(url2);
                    var result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<GetProductsResponse>(result);
                }
                catch (Exception ex)
                {
                    return default(GetProductsResponse);
                }
            }
        }

        public static async Task<SendTransferResponse> SendTransfer(Recarga recarga, string token)
        {

            SendTransferRequest entity = new SendTransferRequest();

            if (recarga.tipoRecarga == TipoRecarga.movil)
            {
                entity.SkuCode = "CU_CU_TopUp";
            }
            else if (recarga.tipoRecarga == TipoRecarga.nauta)
            {
                entity.SkuCode = "CU_NU_TopUp";
            }

            entity.AccountNumber = recarga.numero;
            entity.SendValue = (float)recarga.valor;
            entity.ValidateOnly = Ding.simulate;
            entity.DistributorRef = $"cuenta_{recarga.idCuenta}";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("api_key", token);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var url2 = "https://api.dingconnect.com/api/V1/SendTransfer";
                    var response = await client.PostAsJsonAsync(url2, entity);
                    var result = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<SendTransferResponse>(result);
                }
                catch (Exception ex)
                {
                    return default(SendTransferResponse);
                }
            }
        }

        public class agent_balance
        {
            [DataMember]
            public float Balance { get; set; }
            [DataMember]
            public string CurrencyIso { get; set; }
            [DataMember]
            public int ResultCode { get; set; }
            [DataMember]
            public error[] ErrorCodes { get; set; }
        }

        public class error
        {
            [DataMember]
            public string Code { get; set; }
            [DataMember]
            public string Context { get; set; }

        }

        #region GetProducts

        public class GetProductsRequest
        {
            public string countryIsos { get; set; }
        }

        public class GetProductsResponse
        {
            public string ResultCode { get; set; }
            public error[] ErrorCodes { get; set; }
            public Item[] Items { get; set; }
        }

        public class SettingDefinition
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public bool IsMandatory { get; set; }
        }

        public class MinMax
        {
            public float CustomerFee { get; set; }
            public float DistributorFee { get; set; }
            public float ReceiveValue { get; set; }
            public string ReceiveCurrencyIso { get; set; }
            public float ReceiveValueExcludingTax { get; set; }
            public float TaxRate { get; set; }
            public string TaxName { get; set; }
            public string TaxCalculation { get; set; }
            public float TaxSendValueName { get; set; }
            public string SendCurrencyIso { get; set; }

        }

        public class Item
        {
            public string ProviderCode { get; set; }
            public string SkuCode { get; set; }
            public string LocalizationKey { get; set; }
            public SettingDefinition[] SettingDefinitions { get; set; }
            public MinMax Maximum { get; set; }
            public MinMax Minimum { get; set; }
            public float CommissionRate { get; set; }
            public string ProcessingMode { get; set; }
            public string RedemptionMechanism { get; set; }
            public string[] Benefits { get; set; }
            public string ValidityPeriodIso { get; set; }
            public string UatNumber { get; set; }
            public string AdditionalInformation { get; set; }
            public string DefaultDisplayText { get; set; }
            public string RegionCode { get; set; }
        }

        #endregion

        #region SendTransfer

        public class SendTransferRequest
        {

            public string SkuCode { get; set; }
            public float SendValue { get; set; }
            public string AccountNumber { get; set; }
            public string DistributorRef { get; set; }
            public bool ValidateOnly { get; set; }

        }

        public class SendTransferResponse
        {
            public string ResultCode { get; set; }
            public error[] ErrorCodes { get; set; }
            public TransferRecord TransferRecord { get; set; }
        }

        public class TransferRecord
        {
            public string SkuCode { get; set; }
            public TransferId TransferId { get; set; }
            public Price Price { get; set; }

            public float CommissionApplied { get; set; }
            public string StartedUtc { get; set; }
            public string CompletedUtc { get; set; }
            public string ProcessingState { get; set; }
            public string ReceiptText { get; set; }
            public string AccountNumber { get; set; }
        }

        public class TransferId
        {
            public string TransferRef { get; set; }
            public string DistributorRef { get; set; }
        }

        public class Price
        {
            public float CustomerFee { get; set; }
            public float DistributorFee { get; set; }
            public float ReceiveValue { get; set; }
            public string ReceiveCurrencyIso { get; set; }
            public float ReceiveValueExcludingTax { get; set; }
            public string TaxRate { get; set; }
            public string TaxCalculation { get; set; }
            public float SendValue { get; set; }
            public string SendCurrencyIso { get; set; }

        }

        #endregion

    }
}
