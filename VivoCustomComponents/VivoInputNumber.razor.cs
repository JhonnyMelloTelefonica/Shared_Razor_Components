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
    public partial class VivoInputNumber<T> : InputNumber<T> where T : struct
    {
        static VivoInputNumber()
        {
            if (!IsNumericType())
            {
                throw new InvalidOperationException("T deve ser um tipo númerico.");
            }
        }
        [Parameter] public required string LabelText { get; set; }
        [Parameter] public string? Id { get; set; }
        [Parameter] public string? Style { get; set; }
        [Parameter] public bool Disable { get; set; } = false;
        [Parameter, EditorRequired] public Expression<Func<T>> ValidationFor { get; set; } = default!;
        [Inject] public IJSRuntime JSRuntime { get; set; }

        private static bool IsNumericType()
        {
            var type = typeof(T);
            return type == typeof(int) ||
                   type == typeof(long) ||
                   type == typeof(short) ||
                   type == typeof(float) ||
                   type == typeof(double) ||
                   type == typeof(decimal);
        }

        protected override bool TryParseValueFromString(string value, out T result, out string validationErrorMessage)
        {
            validationErrorMessage = null;
            try
            {
                result = (T)Convert.ChangeType(value, typeof(T));
                return true;
            }
            catch
            {
                result = default;
                validationErrorMessage = $"The {LabelText} field is not valid.";
                return false;
            }
        }

    }
}
