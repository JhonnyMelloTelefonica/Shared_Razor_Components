using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared_Razor_Components.VivoCustomComponents
{
    public partial class VivoInputText<T> : ComponentBase where T : INotifyPropertyChanged
    {
        [Parameter]
        public string LabelText { get; set; }
        [Parameter]
        public T? Model { get; set; }
        [Inject]
        public IJSRuntime JSRuntime { get; set; }


    }
}
