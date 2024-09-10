using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Shared_Razor_Components.FundamentalModels;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;
using Shared_Static_Class.Model_DTO;

namespace Shared_Razor_Components.Services
{
    public interface IPWService
    {
        Task<MainResponse> Get_List_Resultado_Prova(string regional);
        Task<MainResponse> SendEmail(SendEmailModel model);
        Task<MainResponse> SendTeams(SendEmailModel model);
        Task SendEmailTeams(SendEmailModel model);
    }

    public class PWService : IPWService
    {
        public const string GetIndexImagesRoute = "https://prod-77.westeurope.logic.azure.com:443/workflows/c304a18fa3f040dcbb169724c6e01447/triggers/manual/paths/invoke?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=FDG4RO3QKBJjzDsXfoUBsO0PVGnfEMVLHXQgJ_3ipNE";
        public const string SendEmailRoute = "https://prod-80.westeurope.logic.azure.com:443/workflows/92aae95061bf439ab7b320421ae910f2/triggers/manual/paths/invoke?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=JhePuoyIHbSYac3gYJIuEcTueVEQ_Iw0f6sh0hs-tQQ";
        public const string SendTeamsRoute = "https://prod-252.westeurope.logic.azure.com:443/workflows/5761c68de5d0444e95ff392a17e089bc/triggers/manual/paths/invoke?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=PIMLa6p98WqdJrO_WifnEhCz1872tzLHmWprC-HuDx8";
        private IWebHostEnvironment Environment;
        private IConfiguration Config;

        public PWService(IWebHostEnvironment environment, IConfiguration _Config)
        {
            Config = _Config;
            Environment = environment;
        }

        public async Task<MainResponse> Get_List_Resultado_Prova(string regional)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{GetIndexImagesRoute}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress);

                var content = new StringContent($"{{regional:{regional}}}", Encoding.UTF8, "application/json");
                request.Content = content;

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
        public async Task<MainResponse> SendEmail(SendEmailModel model)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(SendEmailRoute);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress);

            var stringcontent = JsonConvert.SerializeObject(model);
            var content = new StringContent(stringcontent, Encoding.UTF8, "application/json");
            request.Content = content;

            var response = await client.SendAsync(request).ConfigureAwait(true);
            var responseString = string.Empty;

            try
            {
                response.EnsureSuccessStatusCode();
                responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<MainResponse>(responseString);
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine(ex);

                return new MainResponse
                {
                    Content = "Não foi possível enviar o e-mail, por favor sinalize ne_automacao.br@telefonica.com",
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task SendEmailTeams(SendEmailModel model)
        {
            await Task.WhenAll(SendEmail(model),SendTeams(model));
        }

        public async Task<MainResponse> SendTeams(SendEmailModel model)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(SendTeamsRoute);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress);

            var stringcontent = JsonConvert.SerializeObject(model);
            var content = new StringContent(stringcontent, Encoding.UTF8, "application/json");
            request.Content = content;

            var response = await client.SendAsync(request).ConfigureAwait(true);
            var responseString = string.Empty;

            try
            {
                response.EnsureSuccessStatusCode();
                responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonConvert.DeserializeObject<MainResponse>(responseString);
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine(ex);

                return new MainResponse
                {
                    Content = "Não foi possível enviar o e-mail, por favor sinalize ne_automacao.br@telefonica.com",
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
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
