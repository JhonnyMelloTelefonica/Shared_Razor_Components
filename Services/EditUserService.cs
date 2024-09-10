using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Shared_Razor_Components.FundamentalModels;
using System.IO.Compression;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Shared_Razor_Components.Services
{
    public interface IEditUserService
    {
        Task<MainResponse> UpdateAvatarImage(byte[] bytes, int matricula);
        Task<MainResponse> UpdateSenhaUser(string old, string newone, string confirmnewone, int matricula);
    }
    public class EditUserService : IEditUserService
    {
        private IWebHostEnvironment Environment;
        private IConfiguration Config;
        public EditUserService(IWebHostEnvironment environment, IConfiguration _Config)
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
        private string BaseUrlJornada
        {
            get
            {
                if (Environment.IsDevelopment())
                {
                    var api_link = Config.GetConnectionString("api_host_link");
                    return $"{api_link}api/Jornada";
                }
                else
                {
                    return "http://brtdtbgs0090sl:9090/api/Jornada";
                }
            }
        }
        public async Task<MainResponse> UpdateSenhaUser(string old, string newone, string confirmnewone, int matricula)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrl}/UpdateSenhaUser?old={old}&newone={newone}&confirmnewone={confirmnewone}&matricula={matricula}");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress);
                var response = await MakeRequestAsync(request, client);
                return new MainResponse
                {
                    Content = response,
                    IsSuccess = true,
                    ErrorMessage = null
                };
            }
            catch (Exception ex)
            {
                return new MainResponse
                {
                    Content = ex.Message,
                    IsSuccess = false,
                    ErrorMessage = "algum erro ocorreu"
                };
            }
        }
        public async Task<MainResponse> UpdateAvatarImage(byte[] bytes, int matricula)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri($"{BaseUrlJornada}/UpdateAvatarImage");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress);

                var body = JsonConvert.SerializeObject(new { bytes = bytes, matricula = matricula });
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                request.Content = content;

                var response = await MakeRequestAsync(request, client);
                return new MainResponse
                {
                    Content = response,
                    IsSuccess = true,
                    ErrorMessage = null
                };
            }
            catch (Exception ex)
            {
                return new MainResponse
                {
                    Content = ex.Message,
                    IsSuccess = false,
                    ErrorMessage = "algum erro ocorreu"
                };
            }
        }

        private async Task<string> MakeRequestAsync(HttpRequestMessage getRequest, HttpClient client)
        {
            var response = await client.SendAsync(getRequest).WaitAsync(new TimeSpan(0, 5, 0)).ConfigureAwait(false);
            var responseString = string.Empty;
            try
            {
                response.EnsureSuccessStatusCode();
                responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            catch (HttpRequestException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return responseString;
        }
    }
}

