using Blazorise.LoadingIndicator;
using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.Hosting;
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
//using KGySoft.CoreLibraries;
using Microsoft.Extensions.Hosting;
using Radzen;
using Shared_Razor_Components.Services;
using Newtonsoft.Json.Linq;
using Azure;
using Blazorise;
using System.Net.Http;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;


namespace Shared_Razor_Components.Shared;

public partial class CargoByPerfil : ComponentBase
{
    //protected IEnumerable<Fila_SubFila> Data { get; set; } = [];
    protected List<Perfil_Cargo_Agrupado> DataToShow { get; set; } = [];

    public List<PERFIS> DataToShowDTO { get; set; } = [];

    public IEnumerable<OptionFilas> Filas { get; set; } = [];

    public IEnumerable<OptionFilas> Sub_Filas { get; set; } = [];

    [Inject] IJSRuntime JSRuntime { get; set; } = null;
    [Inject] NavigationManager NavigationManager { get; set; } = null;
    [Inject] ILoadingIndicatorService ApplicationLoadingIndicatorService { get; set; } = null;
    [Inject] IHostEnvironment Env { get; set; } = null;

    [Inject] IConfiguration Config { get; set; } = null;
    [Inject] DialogService RadzenDialog { get; set; } = null;

    [Inject] UserService Userservice { get; set; }

    bool IsBusy { get; set; }
    string Search { get; set; } = string.Empty;

    protected List<Perfil_Cargo_Agrupado> DataToShowFiltered
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

            client.BaseAddress = new Uri(BaseUrl + $"api/controleADM/GetAllPerfilByCargo");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress);

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();

            DataToShowDTO = JsonConvert.DeserializeObject<List<PERFIS>>(jsonResponse) ?? [];

            foreach (var item in DataToShowDTO)
            {
                var agrupado = new Perfil_Cargo_Agrupado(item);
                DataToShow.Add(agrupado);
            }

            await ApplicationLoadingIndicatorService.Hide();
            StateHasChanged();
        }

        await base.OnAfterRenderAsync(firstRender);
        StateHasChanged();
    }


    public class PERFIS
    {
        public int Id_PERFIL { get; set; }
        public string NOME_PERFIL { get; set; } = string.Empty;
        //public int Id_sub_Fila { get; set; }
        public List<string> cargos { get; set; } = [];
    }


    protected class Perfil_Cargo_Agrupado
    {
        public Perfil_Cargo_Agrupado(PERFIS data, bool isOpened = false)
        {
            IdPerfil = data.Id_PERFIL;
            NomePerfil = data.NOME_PERFIL;
            cargos = data.cargos;
            IsOpened = isOpened;
        }

        public int IdPerfil { get; set; }
        public string NomePerfil { get; set; } = string.Empty;
        public List<string> cargos { get; set; } = [];
        public bool IsOpened { get; set; }
    }
}
