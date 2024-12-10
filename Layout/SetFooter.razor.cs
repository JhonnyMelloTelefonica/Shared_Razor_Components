using Microsoft.AspNetCore.Components;

namespace Shared_Razor_Components.Layout
{
    public class SetFooter : ComponentBase, IDisposable
    {
        [CascadingParameter] public MainLayout? MainLayout { get; set; }
        [CascadingParameter] public UnauthLayout? UnauthLayout { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }

        public event Action OnChange;
        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                if (UnauthLayout is not null)
                {
                    UnauthLayout.SetFooter(this);
                }

                if (MainLayout is not null)
                {
                    MainLayout.SetFooter(this);
                }
            }
            base.OnAfterRender(firstRender);
        }
        public void Update()
        {
            OnChange?.Invoke();
            StateHasChanged();
        }

        public void UpdatePage()
        {
            StateHasChanged();
        }

        public void Dispose()
        {
            UnauthLayout?.SetHeader(null);
            MainLayout?.SetHeader(null);
            OnChange -= Update;
        }

    }
}
