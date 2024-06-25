using Azure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Shared_Static_Class.ErrorModels;
using Shared_Static_Class.Model_DTO;
using Shared_Static_Class.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Shared_Static_Class.Converters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;


namespace Shared_Razor_Components.Shared
{
    public partial class SelectAnalistaSuporte : ComponentBase
    {
        [Inject] public Radzen.DialogService _DialogService { get; set; }
        [Inject] public IConfiguration _config { get; set; }
        [Inject] public IWebHostEnvironment _Environment { get; set; }
        public HttpClient _client { get; set; } = new HttpClient();
        private List<AnalistaOption> Analistas { get; set; } = [];
        public bool IsBusy { get; private set; } = true;
        public bool IsOpened { get; set; } = false;
        public bool IsSelected => Analistas.Any(x => x.Item2 == true);
        [Parameter] public bool IsSuporte { get; set; } = false;
        [Parameter] public bool IsAcessoLogico { get; set; } = false;
        [Parameter] public int SubFila { get; set; } = 0;
        [Parameter] public AcessoModel _user { get; set; } = null;

        protected class AnalistaOption
        {
            public AnalistaOption(ACESSOS_MOBILE_DTO item1, bool item2)
            {
                Item1 = item1;
                Item2 = item2;
            }

            public ACESSOS_MOBILE_DTO Item1 { get; set; } = new();
            public bool Item2 { get; set; } = false;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var host = _Environment.IsDevelopment() ? _config.GetConnectionString("api_host_link") : "http://brtdtbgs0090sl:9090/";
                var response = await _client.GetAsync($"{host}api/Demandas/AnalistaSuporte/Get/{IsSuporte}/{IsAcessoLogico}/{SubFila}/{_user.REGIONAL}/{_user.MATRICULA}");
                string responseString;

                try
                {
                    response.EnsureSuccessStatusCode();
                    responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var listAnalistas = JsonConvert.DeserializeObject<IEnumerable<ACESSOS_MOBILE_DTO>>(responseString);
                    if (listAnalistas != null)
                    {
                        Analistas = listAnalistas
                            .Select(x => {return new AnalistaOption(x, false);} ).ToList();
                    }
                }
                catch (HttpRequestException ex)
                {
                    responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseString);
                    //await _DialogService.Alert(string.Join(" ; ", errorResponse.Errors.Select(x => x.Value)), errorResponse.Title);
                    //_DialogService.Close(new
                    //{
                    //    Analista = 0
                    //});
                }

                IsBusy = false;
                StateHasChanged();
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private async void SelectAnalista()
        {
            _DialogService.Close(Analistas.First(x => x.Item2 == true).Item1.MATRICULA);
        }

        private void ActionSelectAnalista(AnalistaOption item) 
        {
            if (IsSelected)
            {
                Analistas.ForEach(x=> x.Item2 = false);
            }

            item.Item2 = true;
        }

    }
}
