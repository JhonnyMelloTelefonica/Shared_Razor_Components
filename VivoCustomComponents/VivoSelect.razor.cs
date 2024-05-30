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
    public partial class VivoSelect<T> : InputSelect<T>
    {
        [Parameter]
        public required string LabelText { get; set; }
        [Parameter] public string? Id { get; set; }
        [Parameter] public string? Style { get; set; }
        [Parameter] public bool Multiple { get; set; } = false;
        [Parameter] public RenderFragment? Icon { get;  set; }
        [Parameter] public IDictionary<string,T> Data { get; set; }
        [Parameter, EditorRequired] public Expression<Func<T>> ValidationFor { get; set; } = default!;
        /** Este parametro serve apenas para validar se o componente está dentro de um EDITFORM
         * caso sim ele executa funcionalidades de validação de propriedade **/
        [CascadingParameter] public EditContext Context { get; set; } = null; 

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        //protected override bool TryParseValueFromString(string value, out T result, out string validationErrorMessage)
        //{
        //    result = value;
        //    validationErrorMessage = null;
        //    return true;
        //}

        protected override bool TryParseValueFromString(string value, out T result, out string validationErrorMessage)
        {
            if (typeof(T) == typeof(string))
            {
                result = (T)(object)value;
                validationErrorMessage = null;
                return true;
            }
            else if (typeof(T).IsEnum)
            {
                var success = BindConverter.TryConvertTo<T>(value, CultureInfo.CurrentCulture, out var parsedValue);
                if (success)
                {
                    result = parsedValue;
                    validationErrorMessage = null;
                    return true;
                }
                else
                {
                    result = default;
                    validationErrorMessage = null;
                    return false;
                }
            }

            throw new InvalidOperationException($"não suporta o tipo '{typeof(T)}'.");
        }

    }
}
