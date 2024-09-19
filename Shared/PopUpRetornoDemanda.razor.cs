using Blazorise.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Shared_Razor_Components.FundamentalModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared_Static_Class.Model_DTO;
using Shared_Static_Class.Converters;
using Radzen.Blazor;
using Microsoft.FluentUI.AspNetCore.Components;
using Shared_Static_Class.Data;


namespace Shared_Razor_Components.Shared
{
    public partial class PopUpRetornoDemanda : ComponentBase
    {
        [Inject] UserCard Card { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] NavigationManager NavManager { get; set; }
        [Parameter] public ACESSOS_MOBILE_DTO Responsavel { get; set; } = new();
        [Parameter] public Radzen.Orientation Orientation { get; set; } = Radzen.Orientation.Horizontal;
        [Parameter] public DEMANDAS_CHAMADO_DTO chamado { get; set; } = new();
        [Parameter] public DEMANDA_RELACAO_CHAMADO relacao_chamado { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
    }
}
