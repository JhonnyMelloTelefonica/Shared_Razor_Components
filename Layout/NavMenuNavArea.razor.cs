using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using BlazorBootstrap;
using Microsoft.Extensions.Hosting;
using Microsoft.JSInterop;
using Radzen;
using Shared_Razor_Components.Shared;
using Shared_Razor_Components.FundamentalModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Authorization;
using Shared_Static_Class.Model;
using Newtonsoft.Json;
using System.Reflection;


namespace Shared_Razor_Components.Layout
{
    public partial class NavMenuNavArea : ComponentBase
    {
        [Inject] private NavigationManager Nav { get; set; }
        [Inject] IHostingEnvironment Env { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        IJSObjectReference _jsmodule { get; set; }
        bool IsDevelopment { get; set; } = false;
        public IEnumerable<NavMenuData> NavData = [];

        protected override void OnInitialized()
        {
            IsDevelopment = Env.EnvironmentName.ToLower() == "development" ? true : false;
            base.OnInitialized();
        }

        [JSInvokable("FetchData")]
        public void FetchData(string dados)
        {
            NavData = JsonConvert.DeserializeObject<IEnumerable<NavMenuData>>(dados);
            StateHasChanged();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _jsmodule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Shared_Razor_Components/Js/ReadFilesFromRCL.js");
                var dados = await _jsmodule.InvokeAsync<string>("readTextFile", "./_content/Shared_Razor_Components/Files/NavMenuData.json", DotNetObjectReference.Create(this));
            }

            await base.OnAfterRenderAsync(firstRender);
        }

    }
}
