using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Shared_Static_Class.ErrorModels;
using Shared_Razor_Components.FundamentalModels;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Shared_Razor_Components.Services
{
    public interface IAcessoPendenteByIdService
    {
        Task<MainResponse> AnswerAcessoChange_Status(int matricula, int id, string resposta, string status);
        Task<MainResponse> AnswerAcessoDevolver_Analista(DETALHADO_ACESSO_PENDENTE_MODEL? usuario, int matricula, int id, string resposta, string status);
        Task<MainResponse> AnswerAcessoInsert_Novo_Usuario(DETALHADO_ACESSO_PENDENTE_MODEL? usuario, int matricula, int id, string resposta, string status);
        Task<MainResponse> AnswerAcessoUpdate_usuario(DETALHADO_ACESSO_PENDENTE_MODEL? usuario, int matricula, int id, string resposta, string status);
        Task<MainResponse> GetAcessoPendenteById(int idAcesso);

    }
    public class AcessoPendenteByIdService : IAcessoPendenteByIdService
    {
        private IHostEnvironment Environment;
        private IConfiguration Config;

        public AcessoPendenteByIdService(IHostEnvironment environment, IConfiguration _Config)
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
                    return $"{api_link}api/ControleADM";
                }
                else
                {
                    return "http://brtdtbgs0090sl:9090/api/ControleADM";
                }
            }
        }
        public async Task<MainResponse> GetAcessoPendenteById(int idAcesso)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/GetAcessoPendenteById?idAcesso={idAcesso}");
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

        public async Task<MainResponse> AnswerAcessoChange_Status(
              int matricula,
              int id,
              string resposta,
              string status)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/AnswerAcesso/change-status?matricula={matricula}&id={id}&resposta={resposta}&status={status}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress);
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
        public async Task<MainResponse> AnswerAcessoInsert_Novo_Usuario(
            DETALHADO_ACESSO_PENDENTE_MODEL? usuario,
              int matricula,
              int id,
              string resposta,
              string status)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/AnswerAcesso/insert-novo-usuario?matricula={matricula}&id={id}&resposta={resposta}&status={status}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress);
                var body = JsonConvert.SerializeObject(usuario);
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

        public async Task<MainResponse> AnswerAcessoDevolver_Analista(
            DETALHADO_ACESSO_PENDENTE_MODEL? usuario,
              int matricula,
              int id,
              string resposta,
              string status)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/AnswerAcesso/devolver-analista?matricula={matricula}&id={id}&resposta={resposta}&status={status}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress);
                var body = JsonConvert.SerializeObject(usuario);
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
        public async Task<MainResponse> AnswerAcessoUpdate_usuario(
            DETALHADO_ACESSO_PENDENTE_MODEL? usuario,
              int matricula,
              int id,
              string resposta,
              string status)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/AnswerAcesso/update-usuario?matricula={matricula}&id={id}&resposta={resposta}&status={status}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress);
                var body = JsonConvert.SerializeObject(usuario);
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
