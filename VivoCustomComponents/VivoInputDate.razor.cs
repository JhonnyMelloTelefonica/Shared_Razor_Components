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
    public partial class VivoInputDate : InputBase<DateTime?>
    {
        [Parameter]
        public required string LabelText { get; set; }
        [Parameter] public string? Id { get; set; }
        [Parameter] public string? Style { get; set; }
        [Parameter, EditorRequired] public Expression<Func<DateTime?>> ValidationFor { get; set; } = default!;

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        protected override bool TryParseValueFromString(string value, out DateTime? result, out string validationErrorMessage)
        {
            result = DateTime.TryParse(value, out DateTime saida) ? saida: null;
            validationErrorMessage = null;
            return true;
        }

    }
}
