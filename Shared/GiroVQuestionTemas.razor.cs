using Blazorise.LoadingIndicator;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Hosting;
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
using Radzen;

namespace Shared_Razor_Components.Shared;

public partial class GiroVQuestionTemas : ComponentBase
{
    protected IEnumerable<Tema_SubTema> Data { get; set; } = [];
    protected IEnumerable<Tema_SubTema_Agrupado> DataToShow { get; set; } = [];
    [Inject] IJSRuntime JSRuntime { get; set; } = null;
    [Inject] NavigationManager NavigationManager { get; set; } = null;
    [Inject] ILoadingIndicatorService ApplicationLoadingIndicatorService { get; set; } = null;
    [Inject] IHostEnvironment Env { get; set; } = null;
    [Inject] IConfiguration Config { get; set; } = null;
    [Inject] DialogService RadzenDialog { get; set; } = null;
    bool IsBusy { get; set; }
    string Search { get; set; } = string.Empty;
    protected IEnumerable<Tema_SubTema_Agrupado> DataToShowFiltered
    {
        get
        {
            var saida = DataToShow;
            if (!string.IsNullOrEmpty(Search))
            {
                saida = saida.Where(x =>
                x.Tema.Contains(Search, StringComparison.InvariantCultureIgnoreCase)
                || x.TemaAgrupado.Select(x => x.Value.sub_Tema).Any(y => y.Contains(Search, StringComparison.InvariantCultureIgnoreCase)));

                saida.ToList().ForEach(c => c.IsOpened = true);
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
            await ApplicationLoadingIndicatorService.Show();
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(BaseUrl + "api/Jornada/QuestionTemas");
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress);

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string saida = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            Data = JsonConvert.DeserializeObject<IEnumerable<Tema_SubTema>>(saida) ?? [];

            foreach (var item in Data.GroupBy(x => x.Id_Tema))
            {
                DataToShow = DataToShow.Append(new(item.AsEnumerable()));
            }

            await ApplicationLoadingIndicatorService.Hide();
            StateHasChanged();
        }

        await base.OnAfterRenderAsync(firstRender);
        StateHasChanged();
    }


    protected class Tema_SubTema
    {
        public int Id_Tema { get; set; }
        public string Tema { get; set; } = string.Empty;
        public int Id_sub_Tema { get; set; }
        public string sub_Tema { get; set; } = string.Empty;
    }

    protected class Tema_SubTema_Agrupado
    {
        public Tema_SubTema_Agrupado(IEnumerable<Tema_SubTema> data, bool isOpened = false)
        {
            Tema = data.First().Tema;
            id_Tema = data.First().Id_Tema;
            foreach (var tema in data)
            {
                TemaAgrupado.Add(tema.Id_sub_Tema, tema);
            }

            IsOpened = isOpened;
        }

        public int id_Tema { get; set; }
        public string Tema { get; set; } = string.Empty;
        public Dictionary<int, Tema_SubTema> TemaAgrupado { get; set; } = [];
        public bool IsOpened { get; set; }
    }
}
