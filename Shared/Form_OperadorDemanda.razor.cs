using Shared_Static_Class.Model_DTO;
using Blazorise;
using Blazorise.LoadingIndicator;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using static Blazorise.Licensing.Constants;

namespace Shared_Razor_Components.Shared
{
    public partial class Form_OperadorDemanda : ComponentBase
    {
        public List<ACESSOS_MOBILE_DTO> NewOperador { get; set; } = new();
        [Parameter]
        public List<ACESSOS_MOBILE_DTO> EnableOperadores { get; set; }
        [Parameter]
        public Radzen.DialogService Dialogservice { get; set; }
    }
}

