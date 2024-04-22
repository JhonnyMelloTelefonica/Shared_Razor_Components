using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Globalization;

namespace Shared_Razor_Components.Shared
{
    public partial class ClosePlataformButton : ComponentBase, IDisposable
    {
        //: ComponentBase, IDisposable
        public TextInfo textInfo = new CultureInfo("pt-br", false).TextInfo;
        public bool Opened = false;
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] ViewOptionService viewoption { get; set; }

        public event Action OnChange;

        protected override void OnInitialized()
        {
            Update();
            base.OnInitialized();
        }

        public void Update()
        {
            OnChange?.Invoke();
            StateHasChanged();
        }

        public void Dispose()
        {
            OnChange -= Update;
        }

    }
}
