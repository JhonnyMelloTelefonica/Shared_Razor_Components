using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared_Razor_Components.Shared
{
    public partial class VivoSlaDateFormat : ComponentBase
    {
        [Parameter] public int Bind { get; set; }
        [Parameter] public bool HourFormat { get; set; }
        [Parameter] public string Label { get; set; } = "Sla";


        public EventCallback<int> FormatChangedEvent = new();
        public void ValueChanged(ChangeEventArgs value)
        {
            int args = int.Parse(value.Value.ToString());

            if (HourFormat)
            {
                Bind = args;
            }
            else
            {
                Bind = args * 24;
            }
        }

        public void HouFormatChanged()
        {
            HourFormat = !HourFormat;
            FormatChangedEvent.InvokeAsync(Bind);
            StateHasChanged();
        }
    }
}
