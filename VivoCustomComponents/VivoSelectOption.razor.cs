using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shared_Razor_Components.VivoCustomComponents
{
    public partial class VivoSelectOption<T> : ComponentBase
    {
        [Parameter]
        public required string Text { get; set; }
        [Parameter] public string? Id { get; set; }
        [Parameter] public string? Style { get; set; }
        [Parameter] public T Option { get; set; }
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        //protected override bool TryParseValueFromString(string value, out T result, out string validationErrorMessage)
        //{
        //    result = value;
        //    validationErrorMessage = null;
        //    return true;
        //}

    }
}
