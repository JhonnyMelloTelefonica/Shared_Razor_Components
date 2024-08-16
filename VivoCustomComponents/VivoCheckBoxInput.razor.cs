using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace Shared_Razor_Components.VivoCustomComponents
{
    public partial class VivoCheckBoxInput : InputCheckbox
    {
        [Parameter] public string TrueString { get; set; } = "ON";
        [Parameter] public string FalseString { get; set; } = "OFF";
        [Parameter] public string CssClass { get; set; } = string.Empty;
        [Parameter] public string Id { get; set; } = string.Empty;
        [Parameter] public string LabelText { get; set; } = string.Empty;
        [Inject] public IJSRuntime JSRuntime { get; set; }
        public IJSObjectReference _jsmodule { get; set; }
        private bool value;
        protected override void OnParametersSet()
        {
            value = CurrentValue;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _jsmodule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Shared_Razor_Components/VivoCustomComponents.js");
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        private void ChangeValueOnClick()
        {
            value = !value;
            ValueChanged.InvokeAsync(value);
        }

    }
}
