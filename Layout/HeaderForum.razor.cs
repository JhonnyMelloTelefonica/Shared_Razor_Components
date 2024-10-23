using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using System.Globalization;
using Shared_Razor_Components.Services;
using Shared_Razor_Components.FundamentalModels;
using Blazorise.LoadingIndicator;
using Shared_Razor_Components.Shared;
using Microsoft.JSInterop;
using BlazorBootstrap;
using Shared_Static_Class.Data;
using Newtonsoft.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Components.Routing;

namespace Shared_Razor_Components.Layout
{
    public partial class HeaderForum 
    {
        [Parameter] public string title { get; set; } = string.Empty;

    }
}
