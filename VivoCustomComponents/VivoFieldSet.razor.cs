using Blazorise.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.JSInterop;
using Shared_Razor_Components.FundamentalModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;


namespace Shared_Razor_Components.VivoCustomComponents
{
    public partial class VivoFieldSet<T> : ComponentBase where T : INotifyPropertyChanged
    {
        [Parameter]
        public RenderFragment Body { get; set; }
        [Parameter]
        public RenderFragment? Header { get; set; } = null;
        [Parameter]
        public RenderFragment? ClosedBody { get; set; } = null;
        [Parameter]
        public T? Model { get; set; }
        [Parameter]
        public string? Style { get; set; } = string.Empty;
        [Parameter]
        public string? Class { get; set; } = string.Empty;
        [Parameter]
        public bool Locked { get; set; } = true;
        [Inject]
        public IJSRuntime JSRuntime { get; set; }
        public IJSObjectReference _jsmodule { get; set; }
        public string ClassField => $"{(IsOpened ? "opened" : "closed")} {(FormValidated ? "all-valid" : "check-invalid")}";
        public bool FormValidated { get; set; } = true;
        public bool IsOpened { get => Locked ? false : isOpened; set => isOpened = value; }

        private bool isOpened = true;

        public event Action Render;

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Model.PropertyChanged += Update;
                Render += UpdatePage;

                _jsmodule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Shared_Razor_Components/VivoCustomComponents.js");
                //FormValidated = await _jsmodule.InvokeAsync<bool>("areInputsValid", this.GetHashCode());
            }

            //if (_jsmodule is not null)
            //    FormValidated = await _jsmodule.InvokeAsync<bool>("areInputsValid", "form");

            await base.OnAfterRenderAsync(firstRender);
        }

        private void OpenAction()
        {
            if (!Locked)
                IsOpened = !IsOpened;
        }
        private async void Update(object? sender, PropertyChangedEventArgs e)
        {
            if (_jsmodule is not null)
            {
                var check = FormValidated;
                await Task.Delay(100);
                FormValidated = await _jsmodule.InvokeAsync<bool>("areInputsValid", this.GetHashCode());
                Render.Invoke();
                if (check != FormValidated)
                {
                    await Task.Delay(100);
                    await _jsmodule.InvokeVoidAsync("CheckEveryValidation", FormValidated, this.GetHashCode());
                }
            }
        }

        public async void UpdatePage()
        {
            StateHasChanged();
        }

        public async void ExecuteCheckValidation()
        {
            if (_jsmodule is not null)
            {
                var check = FormValidated;
                await Task.Delay(100);
                FormValidated = await _jsmodule.InvokeAsync<bool>("areInputsValid", this.GetHashCode());
                Render.Invoke();
                if (check != FormValidated)
                {
                    await Task.Delay(100);
                    await _jsmodule.InvokeVoidAsync("CheckEveryValidation", FormValidated, this.GetHashCode());
                }
            }
        }
    }
}
