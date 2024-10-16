using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Shared_Static_Class.Converters;
using Shared_Razor_Components.FundamentalModels;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using static Shared_Static_Class.Model_DTO.JORNADA_DTO;

namespace Shared_Razor_Components.Services
{
    public interface ICreateFormService
    {
        Task<MainResponse> CriarFormulario(IEnumerable<TEMAS_QTD>? TEMAS_QUANTIDADE,
            string TIPO_PROVA,
            int CARGO,
            bool FIXA,
            string REGIONAL,
            int MATRICULA,
            DateTime DT_INIT,
            DateTime? DT_FINAL,
            int QTD_TOTAL_SOLICITADA,
            bool ELEGIVEL);
        Task<MainResponse> GetDataCriarFormulario();
        Task<MainResponse> GetQuestionsBySubTema(int subtema);
        Task<MainResponse> GetTemasCriarFormulario(string TIPO_PROVA, string REGIONAL, int CARGO, bool FIXA);
        Task<MainResponse> GetTemasCriarFormularioDetalhado(string Tipo_prova, Cargos Cargo, string regional, bool? Elegiveis, bool? Fixa, bool? Banco_Completo);
        Task<MainResponse> Get_Qtd_SubTema(string TIPO_PROVA, string REGIONAL, int CARGO, bool FIXA, int SUB_TEMA);
        Task<MainResponse> Get_Qtd_Tema(string TIPO_PROVA, string REGIONAL, int CARGO, bool FIXA, int TEMA);
        Task<MainResponse> CriarFormularioDetalhado(string regional, int matricula, string Tipo_prova, Cargos Cargo
            , bool? Elegiveis, DateTime? Dt_inicio, DateTime? Dt_fim, bool? Fixa, IEnumerable<JORNADA_QUESTION_DTO?> data);
    }
    public class CreateFormService : ICreateFormService
    {
        private IHostEnvironment Environment;
        private IConfiguration Config;

        public CreateFormService(IHostEnvironment environment, IConfiguration _Config)
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
        public async Task<MainResponse> GetDataCriarFormulario()
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/GetDataCriarFormulario");
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

        public async Task<MainResponse> GetTemasCriarFormulario(
            string TIPO_PROVA,
            string REGIONAL,
            int CARGO,
            bool FIXA
            )
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/GetTemasCriarFormulario?TIPO_PROVA={TIPO_PROVA}&CARGO={CARGO}&FIXA={FIXA}&REGIONAL={REGIONAL}");
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
        public async Task<MainResponse> CriarFormularioDetalhado(
            string regional,
            int matricula,
            string Tipo_prova,
            Cargos Cargo,
            bool? Elegiveis,
            DateTime? Dt_inicio,
            DateTime? Dt_fim,
            bool? Fixa,
            IEnumerable<JORNADA_QUESTION_DTO?> data
                )
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();

                StringBuilder uriBuilder = new StringBuilder($"{BaseUrl}/CriarFormularioDetalhado?regional={regional}&matricula={matricula}&Tipo_prova={Tipo_prova}&Cargo={(int)Cargo}&");
                if (Elegiveis.HasValue)
                    uriBuilder.Append($"Elegiveis={Elegiveis}&");

                if (Fixa.HasValue)
                    uriBuilder.Append($"Fixa={Fixa}&");

                if (Dt_inicio.HasValue)
                    uriBuilder.Append($"Dt_inicio={Dt_inicio.Value.ToString("yyyy-MM-ddTHH:mm:ss")}&");

                if (Dt_fim.HasValue)
                    uriBuilder.Append($"Dt_fim={Dt_fim.Value.ToString("yyyy-MM-ddTHH:mm:ss")}&");

                string uri = uriBuilder.ToString().TrimEnd('&'); // Remove the trailing '&'

                client.BaseAddress = new Uri(uri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress);

                var body = JsonConvert.SerializeObject(data);
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                request.Content = content;
                client.Timeout = Timeout.InfiniteTimeSpan;

                return await MakeRequestAsync(request, client);
            }
            catch (Exception)
            {
                return new MainResponse
                {
                    Content = "",
                    IsSuccess = false,
                    ErrorMessage = "Algum erro ocorreu"
                };
            }
        }
        public async Task<MainResponse> GetTemasCriarFormularioDetalhado(
                string Tipo_prova,
                Cargos Cargo,
                string regional,
                bool? Elegiveis,
                bool? Fixa,
                bool? Banco_Completo
                )
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();

                StringBuilder uriBuilder = new StringBuilder($"{BaseUrl}/GetTemasCriarFormulario?Tipo_prova={Tipo_prova}&Cargo={(int)Cargo}&regional={regional}&");
                if (Elegiveis.HasValue)
                    uriBuilder.Append($"Elegiveis={Elegiveis}&");

                if (Fixa.HasValue)
                    uriBuilder.Append($"Fixa={Fixa}&");

                if (Banco_Completo.HasValue)
                    uriBuilder.Append($"Banco_Completo={Banco_Completo}&");

                string uri = uriBuilder.ToString().TrimEnd('&'); // Remove the trailing '&'

                client.BaseAddress = new Uri(uri);
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
                    ErrorMessage = "Algum erro ocorreu"
                };
            }
        }
        public async Task<MainResponse> GetQuestionsBySubTema(int subtema)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/GetQuestionsBySubTema?subtema={subtema}");
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
                    ErrorMessage = "Algum erro ocorreu"
                };
            }
        }

        public async Task<MainResponse> Get_Qtd_Tema(
            string TIPO_PROVA,
            string REGIONAL,
            int CARGO,
            bool FIXA,
            int TEMA
            )
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/Get_Qtd_Tema?TIPO_PROVA={TIPO_PROVA}&CARGO={CARGO}&FIXA={FIXA}&TEMA={TEMA}&REGIONAL={REGIONAL}");
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
        public async Task<MainResponse> Get_Qtd_SubTema(
           string TIPO_PROVA,
           string REGIONAL,
           int CARGO,
           bool FIXA,
           int SUB_TEMA
           )
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/Get_Qtd_Tema?TIPO_PROVA={TIPO_PROVA}&CARGO={CARGO}&FIXA={FIXA}&SUB_TEMA={SUB_TEMA}&REGIONAL={REGIONAL}");
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

        public async Task<MainResponse> CriarFormulario(
            IEnumerable<TEMAS_QTD>? TEMAS_QUANTIDADE,
            string TIPO_PROVA,
            int CARGO,
            bool FIXA,
            string REGIONAL,
            int MATRICULA,
            DateTime DT_INIT,
            DateTime? DT_FINAL,
            int QTD_TOTAL_SOLICITADA,
            bool ELEGIVEL
            )
        {
            try
            {
                CultureInfo provider = new CultureInfo("en-IE");
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();

                string uri = $"{BaseUrl}/CreateFormulario?" +
                $"TIPO_PROVA={TIPO_PROVA}&CARGO={CARGO}&FIXA={FIXA}&REGIONAL={REGIONAL}" +
                $"&MATRICULA={MATRICULA}&DT_INIT={DT_INIT.ToString("dd/MM/yyyy")}&QTD_TOTAL_SOLICITADA={QTD_TOTAL_SOLICITADA}" +
                $"&ELEGIVEL={ELEGIVEL}";

                if (DT_FINAL is not null)
                {
                    uri += $"&DT_FINAL={DT_FINAL.Value.ToString("dd/MM/yyyy")}";
                }

                client.BaseAddress = new Uri(uri);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress);
                var body = JsonConvert.SerializeObject(TEMAS_QUANTIDADE);
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                request.Content = content;
                client.Timeout = Timeout.InfiniteTimeSpan;
                var saida = await MakeRequestWaitAsync(request, client);
                return saida;
            }
            catch (Exception ex)
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
            var response = await client.SendAsync(getRequest).WaitAsync(new TimeSpan(0, 20, 0)).ConfigureAwait(true);
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
        private async Task<MainResponse> MakeRequestWaitAsync(HttpRequestMessage getRequest, HttpClient client)
        {
            var responseString = string.Empty;
            try
            {
                var response = await client.SendAsync(getRequest).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                return new MainResponse
                {
                    Content = responseString,
                    IsSuccess = true
                };
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine(ex);

                return new MainResponse
                {
                    Content = null,
                    IsSuccess = false,
                    ErrorMessage = "An error occurred"
                };
            }
        }

    }
}
