using Blazorise.LoadingIndicator;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Shared_Static_Class.Converters;
using Shared_Static_Class.ErrorModels;
using Shared_Static_Class.Model_DTO;
using Shared_Razor_Components.FundamentalModels;
using System.Net;
using System.Text;
using static Shared_Static_Class.Data.DEMANDA_RELACAO_CHAMADO;
using KGySoft.CoreLibraries;
using Radzen;
using Shared_Razor_Components.Services;
using Newtonsoft.Json.Linq;
using Azure;
using Blazorise;
using System.Net.Http;


namespace Shared_Razor_Components.Shared;

public partial class DemandasFila : ComponentBase
{
    //protected IEnumerable<Fila_SubFila> Data { get; set; } = [];
    protected List<Fila_SubFila_Agrupado> DataToShow { get; set; } = [];

    public List<DEMANDA_TIPO_FILA_DTO> DataToShowDTO { get; set; } = [];

    public IEnumerable<OptionFilas> Filas { get; set; } = [];

    public IEnumerable<OptionFilas> Sub_Filas { get; set; } = [];

    [Inject] IJSRuntime JSRuntime { get; set; } = null;
    [Inject] NavigationManager NavigationManager { get; set; } = null;
    [Inject] ILoadingIndicatorService ApplicationLoadingIndicatorService { get; set; } = null;
    [Inject] IWebHostEnvironment Env { get; set; } = null;
    [Inject] IConfiguration Config { get; set; } = null;
    [Inject] DialogService RadzenDialog { get; set; } = null;

    [Inject] UserService Userservice { get; set; } 

    bool IsBusy { get; set; }
    string Search { get; set; } = string.Empty;

    protected List<Fila_SubFila_Agrupado> DataToShowFiltered
    {
        get
        {
            var saida = DataToShow;
            if (!string.IsNullOrEmpty(Search))
            {
                //saida = saida.Where(x =>
                //x.Fila.Contains(Search, StringComparison.InvariantCultureIgnoreCase)
                //|| x.subFilas.Select(x => x.ID_SUB_FILA).Any(y => y.Contains(Search, StringComparison.InvariantCultureIgnoreCase)));

                //saida.ToList().ForEach(c => c.IsOpened = true);
                //saida = saida.Where(x => x.TemaAgrupado.Select(x => x.Value.sub_Tema).Contains(Search))
                //    .ForEach(x => x.IsOpened = true);
            }

            return saida;
        }
    }

    private string BaseUrl
    {
        get
        {
            if (Env.IsDevelopment())
            {
                var api_link = Config.GetConnectionString("api_host_link");
                return $"{api_link}";
            }
            else
            {
                return "http://brtdtbgs0090sl:9090/";
            }
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {   

        if (firstRender)
        {
            var resultRegional = Userservice.User.REGIONAL;

            //var resultRegional = "NE";

            await ApplicationLoadingIndicatorService.Show();
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(BaseUrl + $"api/demandas/GetDadosFilaSubFila?regional={resultRegional}");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress);

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();

            DataToShowDTO = JsonConvert.DeserializeObject<List<DEMANDA_TIPO_FILA_DTO>>(jsonResponse) ?? [] ;

            foreach(var item in DataToShowDTO)
            {
                var agrupado = new Fila_SubFila_Agrupado(item);    
                DataToShow.TryAdd(agrupado);
            }

            await ApplicationLoadingIndicatorService.Hide();
            StateHasChanged();
        }  

        await base.OnAfterRenderAsync(firstRender);
        StateHasChanged();
    }


    //protected class Fila_SubFila
    //{
    //    public int Id_Fila { get; set; }
    //    public string Fila { get; set; } = string.Empty;
    //    public int Id_sub_Fila { get; set; }
    //    public string sub_Fila { get; set; } = string.Empty;
    //}



    protected class Fila_SubFila_Agrupado
    {
        public Fila_SubFila_Agrupado(DEMANDA_TIPO_FILA_DTO data, bool isOpened = false)
        {
            Id_Fila = data.ID_TIPO_FILA;
            Fila = data.NOME_TIPO_FILA;
            subFilas= data.DEMANDA_SUB_FILAs;
            IsOpened = isOpened;
        }

        public int Id_Fila { get; set; }
        public string Fila { get; set; } = string.Empty;
        public List<DEMANDA_SUB_FILA_DTO> subFilas { get; set; } = [];
        public bool IsOpened { get; set; }
    }
}
