using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Shared_Static_Class.Converters;
using Shared_Static_Class.ErrorModels;
using Shared_Static_Class.Model_DTO;
using Shared_Razor_Components.FundamentalModels;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using static Shared_Static_Class.Data.DEMANDA_RELACAO_CHAMADO;

namespace Shared_Razor_Components.Services
{
    public interface IConsultarDemandasService
    {
        Task<MainResponse> AddOperadoresSuporte(IEnumerable<DEMANDA_BD_OPERADORES_DTO> newoperador);
        Task<MainResponse> AtivarOperador(int id);
        Task<MainResponse> DesativarOperador(int id);
        Task<MainResponse> GetAllEnableOperadores(string regional);
        Task<MainResponse> GetDataDash(string regional, IEnumerable<int> Perfis, int matricula,
            //Nullable Parameters
            List<ACESSOS_MOBILE_DTO>? matricula_analista = null, string? status = null, IEnumerable<DateTime> Periodo = null, int? Tipo_fila = null, int? Sub_fila = null, bool Comprioridade = false, bool Semprioridade = false);
        Task<MainResponse> GetDemandaById(string idDemanda);
        Task<MainResponse> GetAcessoById(string idDemanda);
        Task<MainResponse> GetDesligamentoById(string idDemanda);
        Task<MainResponse> GetDemandas(GenericPaginationModel<PaginationDemandasModel> data);
        Task<MainResponse> GetFiltersDemandas(string regional);
        Task<MainResponse> GetOperadoresSuporte(string regional);
        Task<MainResponse> InsertResposta(RespostaDemandaModel newresposta, Tabela_Demanda tabela);
        Task<MainResponse> InsertRespostaChangeStatus(RespostaDemandaModel newresposta, Tabela_Demanda tabela);
        Task<MainResponse> GetAnalistaObservacao(Guid idChamado, int matricula, bool takeall = false);
        Task<MainResponse> PostAnalistaObservacao(Guid idChamado, int matricula, string observacao);
        Task<MainResponse> GetAllIndexChamado(int MATRICULA, Controle_Demanda_role role, string regional);
    }
    public class ConsultarDemandasService : IConsultarDemandasService
    {
        private IWebHostEnvironment Environment;
        private readonly IConfiguration Config;

        public ConsultarDemandasService(IWebHostEnvironment environment, IConfiguration _Config)
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
                    return $"{api_link}api/Demandas";
                }
                else
                {
                    return "http://brtdtbgs0090sl:9090/api/Demandas";
                }
            }
        }

        public async Task<MainResponse> GetDemandaById(string idDemanda)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/GetDemandaById?idDemanda={idDemanda}");
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
        public async Task<MainResponse> GetAcessoById(string idDemanda)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/GetAcessoById?idAcesso={idDemanda}");
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
        public async Task<MainResponse> GetDesligamentoById(string idDemanda)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/GetDesligamentoById?idDesligamento={idDemanda}");
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
        public async Task<MainResponse> GetDemandas(GenericPaginationModel<PaginationDemandasModel> data)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/GetDemandasList");
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
        public async Task<MainResponse> GetFiltersDemandas(string regional)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/GetFiltersDemandas?regional={regional}");
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
        public async Task<MainResponse> GetAllEnableOperadores(string regional)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/GetAllEnableOperadores?regional={regional}");
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
        public async Task<MainResponse> AtivarOperador(int id)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/AtivarOperadorSuporte?id={id}");
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
        public async Task<MainResponse> DesativarOperador(int id)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/DesativarOperadorSuporte?id={id}");
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
        public async Task<MainResponse> GetOperadoresSuporte(string regional)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/GetOperadoresSuporte?regional={regional}");
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
        public async Task<MainResponse> GetDataDash(string regional, IEnumerable<int> Perfis, int matricula,
            //Nullable Parameters
            List<ACESSOS_MOBILE_DTO>? matricula_analista, string? status, IEnumerable<DateTime> Periodo, int? Tipo_fila, int? Sub_fila, bool Comprioridade = false, bool Semprioridade = false)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();

                StringBuilder uriBuilder = new StringBuilder($"{BaseUrl}/GetDataDash?regional={regional}&matricula={matricula}&");
                if (Comprioridade || Semprioridade)
                {
                    uriBuilder.Append($"Comprioridade={Comprioridade}&");
                    uriBuilder.Append($"Semprioridade={Semprioridade}&");
                }

                if (!string.IsNullOrEmpty(status))
                    uriBuilder.Append($"status={status}&");

                string uri = uriBuilder.ToString().TrimEnd('&'); // Remove the trailing '&'
                client.BaseAddress = new Uri(uri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress);

                var body = JsonConvert.SerializeObject(new Tuple<IEnumerable<int>, IEnumerable<DateTime>, List<ACESSOS_MOBILE_DTO>?, int?, int?>(Perfis, Periodo, matricula_analista, Tipo_fila, Sub_fila));
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
        public async Task<MainResponse> AddOperadoresSuporte(IEnumerable<DEMANDA_BD_OPERADORES_DTO> newoperador)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/AddOperadoresSuporte");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress);

                var body = JsonConvert.SerializeObject(newoperador);
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
        public async Task<MainResponse> InsertResposta(RespostaDemandaModel newresposta, Tabela_Demanda tabela)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/InsertResposta?tabela={(int)tabela}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress);

                var body = JsonConvert.SerializeObject(newresposta);
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
        public async Task<MainResponse> InsertRespostaChangeStatus(RespostaDemandaModel newresposta, Tabela_Demanda tabela)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/InsertRespostaChangeStatus?tabela={(int)tabela}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress);

                var body = JsonConvert.SerializeObject(newresposta);
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
        public async Task<MainResponse> GetAnalistaObservacao(Guid idChamado, int matricula, bool takeall)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/GetAnalistaObservacao?idChamado={idChamado}&matricula={matricula}&takeall={takeall}");
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
        public async Task<MainResponse> PostAnalistaObservacao(Guid idChamado, int matricula, string observacao)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/PostAnalistaObservacao?idChamado={idChamado}&matricula={matricula}&observacao={observacao}");
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
        public async Task<MainResponse> GetAllIndexChamado(int MATRICULA, Controle_Demanda_role role, string regional)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/GetAllIndexChamado?MATRICULA={MATRICULA}&role={role}&regional={regional}");
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

        private async Task<MainResponse> MakeRequestAsync(HttpRequestMessage getRequest, HttpClient client)
        {
            client.Timeout = Timeout.InfiniteTimeSpan;
            var response = await client.SendAsync(getRequest).ConfigureAwait(false);
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
