using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Shared_Static_Class.Data;
using Shared_Static_Class.ErrorModels;
using Shared_Static_Class.Model_DTO;
using Shared_Razor_Components.FundamentalModels;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using static Shared_Static_Class.Data.DEMANDA_RELACAO_CHAMADO;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Shared_Static_Class.Model_DTO.FilterModels;
using Azure;
using Shared_Static_Class.Model_ForumRTCZ_Context;
using Blazorise;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;
using Shared_Static_Class.Model;

namespace Shared_Razor_Components.Services
{
    public interface IForumRTCZService
    {
        Task<MainResponse> SearchByFilters(GenericPaginationModel<PainelForumRTCZ> filter);
        Task<MainResponse> SearchByFilters(GenericPaginationModel<PainelForumRTCZ> filter, int matricula);
        Task<MainResponse> SearchByAnalista(GenericPaginationModel<PainelForumRTCZ> filter, int matricula);
        Task<MainResponse> GetTemas();
        Task<MainResponse> GetTemas(int matricula);
        Task<MainResponse> PostPublicacao(PublicacaoModel data);
        Task<MainResponse> PostRespostaPublicacao(RespostaPublicacaoModel data);
        Task<MainResponse> PostAvaliacaoToPublicacao(AvaliacaoPublicacaoModel avaliacao);

    }

    public class ForumRTCZService : IForumRTCZService
    {
        private IHostEnvironment Environment;
        private readonly IConfiguration Config;

        public ForumRTCZService(IHostEnvironment environment, IConfiguration _Config)
        {
            Environment = environment;
            Config = _Config;
        }

        private string BaseUrl
        {
            get
            {
                if (Environment.IsDevelopment())
                {
                    var api_link = Config.GetConnectionString("api_host_link");
                    return $"{api_link}api/ForumRTCZ";
                }
                else
                {
                    return "http://brtdtbgs0090sl:9090/api/ForumRTCZ";
                }
            }
        }

        public async Task<MainResponse> GetTemas()
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/GetTemas");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress);
                return await MakeRequestAsync(request, client);
            }
            catch (Exception ex)
            {
                return new MainResponse
                {
                    Content = ex,
                    IsSuccess = false,
                    ErrorMessage = "algum erro ocorreu"
                };
            }
        }
        public async Task<MainResponse> GetTemas(int matricula)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/GetTemas/{matricula}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress);
                return await MakeRequestAsync(request, client);
            }
            catch (Exception ex)
            {
                return new MainResponse
                {
                    Content = ex,
                    IsSuccess = false,
                    ErrorMessage = "algum erro ocorreu"
                };
            }
        }
        public async Task<MainResponse> SearchByAnalista(GenericPaginationModel<PainelForumRTCZ> filter, int matricula)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/GetPublicacoesPendentes/{matricula}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress);

                var body = JsonConvert.SerializeObject(filter);
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                request.Content = content;

                return await MakeRequestAsync(request, client);
            }
            catch (Exception ex)
            {
                return new MainResponse
                {
                    Content = ex,
                    IsSuccess = false,
                    ErrorMessage = "algum erro ocorreu"
                };
            }
        }
        public async Task<MainResponse> SearchByFilters(GenericPaginationModel<PainelForumRTCZ> filter, int matricula)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/Get/{matricula}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress);

                var body = JsonConvert.SerializeObject(filter);
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                request.Content = content;

                return await MakeRequestAsync(request, client);
            }
            catch (Exception ex)
            {
                return new MainResponse
                {
                    Content = ex,
                    IsSuccess = false,
                    ErrorMessage = "algum erro ocorreu"
                };
            }
        }
        public async Task<MainResponse> SearchByFilters(GenericPaginationModel<PainelForumRTCZ> filter)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/Get");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress);

                var body = JsonConvert.SerializeObject(filter);
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                request.Content = content;

                return await MakeRequestAsync(request, client);
            }
            catch (Exception ex)
            {
                return new MainResponse
                {
                    Content = ex,
                    IsSuccess = false,
                    ErrorMessage = "algum erro ocorreu"
                };
            }
        }
        public async Task<MainResponse> PostPublicacao(PublicacaoModel data)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/publicacao/post");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress);

                var body = JsonConvert.SerializeObject(data);
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                request.Content = content;

                return await MakeRequestAsync(request, client);
            }
            catch (Exception ex)
            {
                return new MainResponse
                {
                    Content = ex,
                    IsSuccess = false,
                    ErrorMessage = "algum erro ocorreu"
                };
            }
        }
        public async Task<MainResponse> PostRespostaPublicacao(RespostaPublicacaoModel data)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/publicacao/resposta/post");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress);

                var body = JsonConvert.SerializeObject(data);
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                request.Content = content;

                return await MakeRequestAsync(request, client);
            }
            catch (Exception ex)
            {
                return new MainResponse
                {
                    Content = ex,
                    IsSuccess = false,
                    ErrorMessage = "algum erro ocorreu"
                };
            }
        }
        public async Task<MainResponse> PostAvaliacaoToPublicacao(AvaliacaoPublicacaoModel avaliacao)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/Post/Avaliacao/Publicacao");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress);

                var body = JsonConvert.SerializeObject(avaliacao);
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
            client.Timeout = Timeout.InfiniteTimeSpan;
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
                if (!string.IsNullOrEmpty(responseString))
                {
                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseString);
                    var Erros = string.Join('\n', errorResponse.Errors.SelectMany(x => x.Value).Select((x, y) => $"{y + 1}°. {x}"));
                    return new MainResponse
                    {
                        Content = errorResponse,
                        IsSuccess = false,
                        ErrorMessage = Erros
                    };
                }
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
