using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Types;
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
    public partial class VivoInputNumber : InputBase<int>
    {
        [Parameter] public required string LabelText { get; set; }
        [Parameter] public string? Id { get; set; }
        [Parameter] public string? Style { get; set; }
        [Parameter] public bool Disable { get; set; } = false;
        [Parameter, EditorRequired] public Expression<Func<int>> ValidationFor { get; set; } = default!;
        [Inject] public IJSRuntime JSRuntime { get; set; }

        protected override bool TryParseValueFromString(string value, out int result, out string validationErrorMessage)
        {
            result = int.TryParse(value, out int numbervalue) ? numbervalue : 0;
            validationErrorMessage = null;
            return true;
        }

    }
}
