using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Shared_Razor_Components.FundamentalModels;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;

namespace Shared_Razor_Components.Services
{
    public interface IListaFormService
    {
        Task<MainResponse> FinalizarForm(int ID_CRIADOR, string CARGO, int CADERNO, string TP_FORMS, bool FIXA);
        Task<MainResponse> GetFormsRotaByUser(int CARGO, int MATRICULA, bool FIXA, string REGIONAL);
    }
    public class ListaFormService : IListaFormService
    {
        private IHostEnvironment Environment;
        private IConfiguration Config;

        public ListaFormService(IHostEnvironment environment, IConfiguration _Config)
        {
            Config = _Config;
            Environment = environment;
        }
        private string BaseUrl
        {
            get
            {
                if (Environment.IsDevelopment())
                {
                    var api_link = Config.GetConnectionString("api_host_link");
                    return $"{api_link}api/ControleFormJornada";
                }
                else
                {
                    return "http://brtdtbgs0090sl:9090/api/ControleFormJornada";
                }
            }
        }
        public async Task<MainResponse> GetFormsRotaByUser(int CARGO, int MATRICULA, bool FIXA, string REGIONAL)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/GetFormsRotaByUser?CARGO={CARGO}&MATRICULA={MATRICULA}&FIXA={FIXA}&REGIONAL={REGIONAL}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress);
                return await MakeRequestAsync(request, client);
            }
            catch (Exception)
            {
                return new MainResponse
                {
                    Content = "",
                    IsSuccess = false,
                    ErrorMessage = "algum erro ocorreu"
                };
            }
        }
        public async Task<MainResponse> FinalizarForm(
            int ID_CRIADOR
            , string CARGO
            , int CADERNO
            , string TP_FORMS
            , bool FIXA)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(
                    $"{BaseUrl}/FinalizarForm?ID_CRIADOR={ID_CRIADOR}&CARGO={CARGO}&FIXA={FIXA}" +
                    $"&CADERNO={CADERNO}&TP_FORMS={TP_FORMS}"
                    );
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress);
                return await MakeRequestAsync(request, client);
            }
            catch (Exception)
            {
                return new MainResponse
                {
                    Content = "",
                    IsSuccess = false,
                    ErrorMessage = "algum erro ocorreu"
                };
            }
        }

        private async Task<MainResponse> MakeRequestAsync(HttpRequestMessage getRequest, HttpClient client)
        {
            var response = await client.SendAsync(getRequest).WaitAsync(new TimeSpan(0, 5, 0)).ConfigureAwait(true);
            var responseString = string.Empty;
            try
            {
                response.EnsureSuccessStatusCode();
                responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine(ex);
            }
            if (!string.IsNullOrEmpty(responseString))
            {
                return new MainResponse
                {
                    Content = responseString,
                    IsSuccess = true
                };
            }
            else
            {
                return new MainResponse
                {
                    Content = response,
                    IsSuccess = false,
                    ErrorMessage = "algum erro ocorreu"
                };
            }
        }
    }
}
