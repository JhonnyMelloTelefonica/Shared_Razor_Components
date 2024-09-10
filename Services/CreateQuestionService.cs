using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Shared_Static_Class.Data;
using Shared_Razor_Components.FundamentalModels;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using static Shared_Static_Class.Model_DTO.JORNADA_DTO;

namespace Shared_Razor_Components.Services
{
    public interface ICreateQuestionService
    {
        Task<MainResponse> CreateQuestion(int TEMA, int SUB_TEMA, IEnumerable<string> TP_FORMS, string TP_QUESTAO, string PERGUNTA, IEnumerable<int> CARGO, bool? FIXA, List<JORNADA_BD_ANSWER_ALTERNATIVA> ALTERNATIVAS, int? matricula, string regional);
        Task<MainResponse> CreateQuestionMassivo(List<JORNADA_QUESTION_DTO> data, int matricula);
        Task<MainResponse> GetDataCriarQuestion();
    }
    public class CreateQuestionService : ICreateQuestionService
    {
        private IWebHostEnvironment Environment;
        private IConfiguration Config;

        public CreateQuestionService(IWebHostEnvironment environment, IConfiguration _Config)
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
                    return $"{api_link}api/ControleQuestionsJornada";
                }
                else
                {
                    return "http://brtdtbgs0090sl:9090/api/ControleQuestionsJornada";
                }
            }
        }
        public async Task<MainResponse> GetDataCriarQuestion()
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/GetDataCriarQuestion");
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
        public async Task<MainResponse> CreateQuestion(
            int TEMA,
            int SUB_TEMA,
            IEnumerable<string> TP_FORMS,
            string TP_QUESTAO,
            string PERGUNTA,
            IEnumerable<int> CARGO,
            bool? FIXA,
            List<JORNADA_BD_ANSWER_ALTERNATIVA> ALTERNATIVAS,
            int? matricula,
            string regional
            )
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/CreateQuestion");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress);
                var body = JsonConvert.SerializeObject(new CreateQuestionModel
                {
                    TEMA = TEMA,
                    SUB_TEMA = SUB_TEMA,
                    TP_FORMS = TP_FORMS.ToList(),
                    TP_QUESTAO = TP_QUESTAO,
                    PERGUNTA = PERGUNTA,
                    CARGO = CARGO.ToList(),
                    FIXA = FIXA,
                    ALTERNATIVAS = ALTERNATIVAS,
                    matricula = matricula,
                    regional = regional
                });
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
        public async Task<MainResponse> CreateQuestionMassivo(List<JORNADA_QUESTION_DTO> data, int matricula)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/CreateQuestionMassivo?matricula={matricula}");
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


