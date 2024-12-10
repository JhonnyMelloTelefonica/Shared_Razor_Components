using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Radzen;
using Shared_Razor_Components.FundamentalModels;
using Shared_Razor_Components.Layout;
using System.ComponentModel;
using System.Globalization;

namespace Shared_Razor_Components.Shared.BasicForApplication
{
    public partial class Register
    {
        private CriarUsuarioTemplate ModalComponent;
        public SOLICITAR_USUARIO_MODEL Criaruser { get; set; } = new();
        public TextInfo textInfo = new CultureInfo("pt-br", false).TextInfo;
        string witdhpage => registerservice.AlreadySolicitated ? "width: auto; margin: 40px auto;" : "width: 700px; margin: 40px auto;";
        public IBrowserFile file { get; set; }
        public EditContext editContext { get; set; }
        IJSObjectReference _reference;

        private async Task ClickFileInput()
        {
            await JSRuntime.InvokeVoidAsync("fileInputInterop.clickFileInput");
        }

        protected override async Task OnInitializedAsync()
        {
            service.PropertyChanged += OnStateChanged;
            service.isBusy = true;
            await registerservice.VerifyCurrentUserExists();
            service.isBusy = false;
            editContext = new EditContext(Criaruser);
            await base.OnInitializedAsync();
        }


        public async Task ChangeAvatar(InputFileChangeEventArgs args)
        {
            Criaruser.UserAvatar = null;
            service.isBusy = true;
            file = args.File;
            if (file != null)
            {
                using (var stream = file.OpenReadStream())
                {
                    Criaruser.UserAvatar = new byte[stream.Length];
                    await stream.ReadAsync(Criaruser.UserAvatar, 0, (int)stream.Length);
                }
            }
            service.isBusy = false;

            await InvokeAsync(StateHasChanged);
        }

        private async Task CriarUser(SOLICITAR_USUARIO_MODEL user)
        {
            var OBS = await service.Swal.FireAsync(new SweetAlertOptions
            {
                Title = "Quase lá...",
                Text = "Por favor nos dê mais detalhes sobre quem você é e sua motivação para solicitação de acesso a ferramenta",
                Icon = SweetAlertIcon.Warning,
                InputAutoFocus = true,
                ShowCancelButton = false,
                Input = SweetAlertInputType.Textarea,
                InputValidator = new InputValidatorCallback((string input) => input.Length == 0 ? "Por favor informe algum valor" : null, this),
                ValidationMessage = "Por favor informe algum valor",
                ConfirmButtonText = "Finalizar",
                ShowCloseButton = true,
                ShowDenyButton = false,
                AllowEscapeKey = false,
                AllowEnterKey = true,
                ShowLoaderOnConfirm = true,
                AllowOutsideClick = false,
            });

            if (!string.IsNullOrEmpty(OBS.Value))
            {
                await service.CriarUsuario(user, OBS.Value);
                this.StateHasChanged();
                return;
            }
        }

        private void OnStateChanged(object? sender, PropertyChangedEventArgs e) => StateHasChanged();

        public void Dispose()
        {
            if (ModalComponent is not null)
                ModalComponent.Dispose();
            service.PropertyChanged -= OnStateChanged;
        }

    }
}