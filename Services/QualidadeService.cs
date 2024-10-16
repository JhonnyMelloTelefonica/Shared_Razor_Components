using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Shared_Static_Class.Data;
using Shared_Static_Class.Model_DTO;
using System.Net.Http.Json;
using System.Text;


public interface IQualidadeService
{
    Task<List<QUALIDADE_BD_MANUAL_AUDIT>> GetAuditDataAsync();
    Task<bool> EnviarLinhaFidelizadaAsync(FidelizacaoDTO dadosFidelizacao);

}

public class QualidadeService : IQualidadeService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public QualidadeService(HttpClient httpClient, IHostEnvironment environment)
    {
        _httpClient = httpClient;


        if (environment.IsDevelopment())
        {
            _baseUrl = "https://s3q65465-5159.brs.devtunnels.ms/";
        }
        else
        {
            _baseUrl = "http://brtdtbgs0090sl:9090/";
        }

        _httpClient.BaseAddress = new Uri(_baseUrl);
    }

    public async Task<List<QUALIDADE_BD_MANUAL_AUDIT>> GetAuditDataAsync()
    {
        try
        {
            HttpResponseMessage response = await _httpClient.GetAsync("api/Qualidade/Qualidade");


            if (response.IsSuccessStatusCode)
            {

                return await response.Content.ReadFromJsonAsync<List<QUALIDADE_BD_MANUAL_AUDIT>>();
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu uma exceção durante a solicitação HTTP: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> EnviarLinhaFidelizadaAsync(FidelizacaoDTO dadosFidelizacao)
    {
        try
        {
            string jsonContent = JsonConvert.SerializeObject(dadosFidelizacao);

            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await _httpClient.PostAsync("api/Qualidade/EnviarDadosFidelizacao", content);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ocorreu uma exceção durante a solicitação HTTP: {ex.Message}");
            return false;
        }

    }

}