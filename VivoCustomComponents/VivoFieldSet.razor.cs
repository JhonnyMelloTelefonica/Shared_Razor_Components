using Blazorise.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.JSInterop;
using Shared_Static_Class.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;


namespace Shared_Razor_Components.VivoCustomComponents
{
    public partial class VivoFieldSet<T> : ComponentBase
    {
        [Parameter]
        public RenderFragment Body { get; set; }
        [Parameter]
        public RenderFragment? Header { get; set; } = null;
        [Parameter]
        public RenderFragment? ClosedBody { get; set; } = null;
        [Parameter]
        public bool IsOpened { get; set; } = false;
        [Parameter]
        public T? Model { get; set; }
        [Inject]
        public IJSRuntime JSRuntime { get; set; }
        public IJSObjectReference _jsmodule { get; set; }
        public bool? FormValidated { get; set; } = null;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _jsmodule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Shared_Razor_Components/VivoCustomComponents.js");
                FormValidated = await _jsmodule.InvokeAsync<bool>("areInputsValid","form");                
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        public async void IsValid()
        {
            FormValidated = await _jsmodule.InvokeAsync<bool>("areInputsValid", "form");
        }
    }
}
