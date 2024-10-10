using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Shared_Static_Class.Data;
using Shared_Static_Class.ErrorModels;
using Shared_Razor_Components.FundamentalModels;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Shared_Razor_Components.Services
{
    public interface IDesligamentosService
    {
        Task<MainResponse> Create(DEMANDA_DESLIGAMENTOS data, string MENSAGEM);
    }
    public class DesligamentosService : IDesligamentosService
    {
        private IHostEnvironment Environment;
        private IConfiguration Config;

        public DesligamentosService(IHostEnvironment environment, IConfiguration _Config)
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

                    return $"{api_link}api/Desligamentos";
                }
                else
                {
                    return "http://brtdtbgs0090sl:9090/api/Desligamentos";
                }
            }
        }
        public async Task<MainResponse> Create(DEMANDA_DESLIGAMENTOS data, string MENSAGEM)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/create?MENSAGEM={MENSAGEM}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress);

                var body = JsonConvert.SerializeObject(data);
                var content = new StringContent(body, Encoding.UTF8, "application/json");
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
                responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseString);
                return new MainResponse
                {
                    Content = errorResponse,
                    IsSuccess = false,
                    ErrorMessage = errorResponse.Title
                };
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
