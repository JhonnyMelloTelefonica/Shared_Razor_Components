using System.Globalization;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Shared_Razor_Components.Shared;

public partial class VivoSlaDateFormat : InputBase<int>
{
    [Parameter] public bool HourFormat { get; set; }
    [Parameter] public string Label { get; set; } = "Sla";
    [Parameter, EditorRequired] public Expression<Func<int>> ValidationFor { get; set; } = default!;
    public EventCallback<int> FormatChangedEvent = new();
    protected override bool TryParseValueFromString(string value, out int result, out string validationErrorMessage)
    {
        validationErrorMessage = null;
        try
        {
            result = int.TryParse(value, CultureInfo.InvariantCulture, out int saida) ? saida : 0;
            return true;
        }
        catch
        {
            result = default;
            validationErrorMessage = $"O {Label} não é um campo válido.";
            return false;
        }
    }
    public void ValueChangedEvent(ChangeEventArgs value)
    {
        int args = int.Parse(value.Value.ToString());

        if (!HourFormat)
        {
            args = args * 24;
        }

        Value = args;
        
        ValueChanged.InvokeAsync(args);
    }

    public void HouFormatChanged()
    {
        HourFormat = !HourFormat;
        FormatChangedEvent.InvokeAsync(Value);
        //StateHasChanged();
    }
}
