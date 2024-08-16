using Microsoft.AspNetCore.Components;
using System.Globalization;
using Microsoft.JSInterop;
using Shared_Static_Class.Model_DTO;


namespace Shared_Razor_Components.Shared
{
    public partial class UserCard : ComponentBase, IDisposable
    {
        //: ComponentBase, IDisposable
        public TextInfo textInfo = new CultureInfo("pt-br", false).TextInfo;
        public bool Opened = false;
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Parameter] public ACESSOS_MOBILE_DTO User { get; set; } = new();

        public event Action OnChange;

        protected override void OnInitialized()
        {
            Update();
            base.OnInitialized();
        }

        public void Update()
        {
            //if (MainLayout is not null)
            //{
            //    OnChange += MainLayout.Update;
            //    MainLayout.Update();
            //}
            OnChange?.Invoke();
            StateHasChanged();
        }


        [JSInvokable("SetUserCard")]
        public void StepClicked(ACESSOS_MOBILE_DTO user)
        {
            User = user;
            Opened = true;
            Update();
        }

        [JSInvokable("CloseUserCard")]
        public void CloseCard()
        {
            User = null;
            Opened = false;
            Update();
        }

        public void Dispose()
        {
            //if (MainLayout is not null)
            //{
            //    OnChange -= MainLayout.Update;
            //}
            OnChange -= Update;
        }

    }
}

