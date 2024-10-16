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


namespace Shared_Razor_Components.Layout
{
    public partial class UnauthLayout : LayoutComponentBase, IDisposable
    {
        RenderFragment headerContent => setHeader?.ChildContent;
        RenderFragment footerContent => setFooter?.ChildContent;
        SetFooter setFooter;
        SetHeader setHeader;

        public void SetHeader(SetHeader setHeader)
        {
            this.setHeader = setHeader;
            if (setHeader is not null)
            {
                setHeader.OnChange += Update;
            }
            Update();
        }

        public void SetFooter(SetFooter setHeader)
        {
            this.setFooter = setFooter;
            if (setFooter is not null)
            {
                setFooter.OnChange += Update;
            }
            Update();
        }

        public void Update() => StateHasChanged();            

        public void Dispose()
        {
            //this.Dispose();
        }
    }
}
