using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Shared_Razor_Components.FundamentalModels;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Shared_Razor_Components.Services
{
    public interface IEditQuestionService
    {
        Task<MainResponse> DeleteQuestion(int ID_QUESTION, int matricula);
        Task<MainResponse> EnableQuestion(int ID_QUESTION, int matricula);
        Task<MainResponse> GetEditQuestion(GenericPaginationModel<FilterEditQuestionModel> filter);
    }
    public class EditQuestionService : IEditQuestionService
    {
        private IWebHostEnvironment Environment;
        private IConfiguration Config;

        public EditQuestionService(IWebHostEnvironment environment, IConfiguration _Config)
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
        public async Task<MainResponse> GetEditQuestion(GenericPaginationModel<FilterEditQuestionModel> filter)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/GetEditQuestion");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress);

                var body = JsonConvert.SerializeObject(filter);
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
        public async Task<MainResponse> EnableQuestion(int ID_QUESTION, int matricula)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/EnableQuestion?ID_QUESTION={ID_QUESTION}&matricula={matricula}");
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
        public async Task<MainResponse> DeleteQuestion(int ID_QUESTION, int matricula)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/DeleteQuestion?ID_QUESTION={ID_QUESTION}&matricula={matricula}");
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
