using Microsoft.AspNetCore.Components;
using System.Globalization;
using Microsoft.JSInterop;
using Shared_Static_Class.Model_DTO;
using Microsoft.Extensions.Hosting;


namespace Shared_Razor_Components.Shared
{
    public partial class UserCard : ComponentBase, IDisposable
    {
        //: ComponentBase, IDisposable
        public TextInfo textInfo = new CultureInfo("pt-br", false).TextInfo;
        public bool Opened = false;
        public bool Context = false;
        [Inject] IHostEnvironment Env { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Parameter] public ACESSOS_MOBILE_DTO User { get; set; } = new();

        [Parameter]
        public RenderFragment? ImageUser { get; set; } 

        [Parameter]
        public RenderFragment? senhaUser { get; set; }
        bool IsDevelopment { get; set; } = false;

        public event Action OnChange;

        protected override void OnInitialized()
        {
            Update();
            IsDevelopment = Env.EnvironmentName.ToLower() == "development" ? true : false;
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
        public void StepClicked(ACESSOS_MOBILE_DTO user, bool context)
        {
            User = user;
            Opened = true;

            if(context == true)
            {
                Context = true;
            }
            else
            {
                Context = false;
            }

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

