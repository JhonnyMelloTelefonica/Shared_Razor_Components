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
    public partial class VivoInputText : InputBase<string>
    {
        [Parameter]
        public required string LabelText { get; set; }
        [Parameter] public string? Id { get; set; }
        [Parameter] public string? Style { get; set; }
        [Parameter] public bool Disable { get; set; } = false;
        [Parameter, EditorRequired] public Expression<Func<string>> ValidationFor { get; set; } = default!;
        [Inject]public IJSRuntime JSRuntime { get; set; }
        /** Este parametro serve apenas para validar se o componente está dentro de um EDITFORM
         * caso sim ele executa funcionalidades de validação de propriedade **/
        [CascadingParameter] public EditContext Context { get; set; } = null;

        protected override bool TryParseValueFromString(string value, out string result, out string validationErrorMessage)
        {
            result = value;
            validationErrorMessage = null;
            return true;
        }

    }
}
