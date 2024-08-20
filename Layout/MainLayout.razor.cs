using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using System.Globalization;
using Microsoft.AspNetCore.SignalR.Client;
using Shared_Class_Vivo_Apps.Services;
using Shared_Static_Class.Models;
using Blazorise.LoadingIndicator;
using Shared_Razor_Components.Shared;
using Microsoft.JSInterop;
using BlazorBootstrap;
using Shared_Static_Class.Data;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;

namespace Shared_Razor_Components.Layout
{
    public partial class MainLayout : LayoutComponentBase
    {
        [Inject] HubConnection HubConnection { get; set; }
        [Inject] IRegisterService _contextservice { get; set; }
        [Inject] GetUser_REDECORP getUser_REDECORP { get; set; }
        [Inject] ILoadingIndicatorService ApplicationLoadingIndicatorService { get; set; }
        [Inject] ViewOptionService ViewOption { get; set; }
        [Inject] UserService Userservice { get; set; }
        [Inject] UserCard User { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] Blazorise.IMessageService MessageService { get; set; }
        [Inject] NavigationManager navigationManager { get; set; }

        RenderFragment headerContent
        {
            get
            {
                return setHeader?.ChildContent;
            }
        }
        RenderFragment footerContent
        {
            get
            {
                return setFooter?.ChildContent;
            }
        }
        SetFooter setFooter;
        SetHeader setHeader;
        public bool isBusy = true;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                if (isBusy == value) return;
                if (value == true)
                {
                    ApplicationLoadingIndicatorService.Show();
                }
                else
                {
                    ApplicationLoadingIndicatorService.Hide();
                }
                isBusy = value;

                StateHasChanged();
            }
        }

        List<ToastMessage> messagesToast = new List<ToastMessage>();
        public int CountUsersOnline = 0;
        public TextInfo textInfo = new CultureInfo("pt-br", false).TextInfo;
        public SOLICITAR_USUARIO_MODEL user { get; set; } = new();
        public ACESSOS_MOBILE_PENDENTE? userSolicitado { get; set; } = new();
        public bool AlreadySolicitated { get; set; } = false;


        protected override async Task OnInitializedAsync()
        {
            if (Userservice.User is null)
            {
                Userservice.User = new AcessoModel
                {
                    MATRICULA = user.MATRICULA
                };
            }
            await VerifyCurrentUserExists();
            //ViewOption.PropertyChanged += OnStateChanged;
        }

        private void OnStateChanged(object? sender, PropertyChangedEventArgs e) => InvokeAsync(Update);

        public void Dispose()
        {
            ViewOption.PropertyChanged -= OnStateChanged;
        }

        public void SetHeader(SetHeader setHeader)
        {
            this.setHeader = setHeader;
            if (setHeader is not null)
            {
                setHeader.OnChange += Update;
            }
        }

        public void SetFooter(SetFooter setFooter)
        {
            this.setFooter = setFooter;
            if (setFooter is not null)
            {
                setFooter.OnChange += Update;
            }
        }

        public void Update()
        {
            StateHasChanged();
        }

        public async Task VerifyCurrentUserExists()
        {
            IsBusy = true;
            try
            {
                var result = await _contextservice.VerifyCurrentUserExists(getUser_REDECORP.GetMatricula());
                if (result.IsSuccess)
                {
                    Response<object> saida = JsonConvert.DeserializeObject<Response<object>>(result.Content.ToString());
                    if (saida.Succeeded)
                    {
                        var saidaSolicitado = JsonConvert.DeserializeObject<ACESSOS_MOBILE_PENDENTE>(saida.Data.ToString());
                        if (saidaSolicitado is not null)
                        {
                            if (saidaSolicitado.ID != 0)
                            {
                                userSolicitado = saidaSolicitado;
                                AlreadySolicitated = true;
                                await MessageService.Warning(saida.Message, "Solicitação em andamento");
                            }
                        }
                    }
                    else
                    {
                        Response<string> Resultado = JsonConvert.DeserializeObject<Response<string>>(result.Content.ToString());
                        await ErrorModel(Resultado);
                    }
                }
            }
            catch
            {
                await MessageService.Error("Ocorreu algum erro ao processar sua solicitação", "Erro!");
            }

            IsBusy = false;

            return;
        }

        public async Task ErrorModel(Response<string> data)
        {
            await MessageService.Error(data.Message, "Erro!");
            await JSRuntime.InvokeVoidAsync("console.log", data.Errors);
        }
    }
}
