using Blazorise.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Shared_Static_Class.Models;
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


namespace Shared_Razor_Components.Shared
{
    public partial class PopUpRetornoDemanda : ComponentBase
    {
        [Inject] NavigationManager NavManager { get; set; }
        [Parameter] public ACESSOS_MOBILE_DTO Responsavel { get; set; } = new();
        [Parameter] public Radzen.Orientation Orientation { get; set; } = Radzen.Orientation.Horizontal;
        [Parameter] public DEMANDAS_CHAMADO_DTO chamado { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }
    }
}
