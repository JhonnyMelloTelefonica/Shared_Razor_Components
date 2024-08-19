using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using Radzen;
using Shared_Razor_Components.Shared;
using Shared_Static_Class.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Shared_Razor_Components.Layout
{
    public partial class NavMenu : ComponentBase, IDisposable
    {
        [Inject] IHostingEnvironment Env { get; set; }
        [Inject] private Radzen.DialogService DialogService { get; set; }
        [Inject] private IJSRuntime JSRuntime { get; set; }
        [Inject] private UserService user { get; set; }
        [Inject] private ViewOptionService ViewOption { get; set; }
        [Inject] private UserCard User { get; set; }
        bool IsDevelopment { get; set; } = false;

        List<ToastMessage> messagesToast = new List<ToastMessage>();
        public int CountUsersOnline = 0;
        public TextInfo textInfo = new CultureInfo("pt-br", false).TextInfo;

        private void ShowMessage(ToastType toastType) => messagesToast.Add(new ToastMessage
        {
            Type = toastType,
            Title = "Blazor Bootstrap",
            HelpText = $"{DateTime.Now}",
            Message = $"Hello, world! This is a toast message. DateTime: {DateTime.Now}",
        });

        private ToastMessage CreateToastMessage(ToastType toastType, string titulo, string mensagem)
        => new ToastMessage
        {
            Type = toastType,
            Title = titulo,
            HelpText = DateTime.Now.ToShortTimeString(),
            IconName = BlazorBootstrap.IconName.BellFill,
            Message = mensagem
        };

        public List<Notification> messages = new();
        public bool IsConnected => HubConnection?.State == HubConnectionState.Connected;
        [Inject] HubConnection HubConnection { get; set; }

        protected override async Task OnInitializedAsync()
        {
            ViewOption.PropertyChanged += OnStateChanged;
            IsDevelopment = Env.EnvironmentName.ToLower() == "development" ? true : false;
            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                try
                {
                    HubConnection.On<string, string, string, string>("NewNotification", (nome, titulo, mensagem, link) =>
                    {
                        messagesToast.Add(CreateToastMessage(ToastType.Info, titulo, mensagem));

                        messages.Add(new Notification
                        {
                            Hora = DateTime.Now,
                            message = mensagem,
                            SenderName = nome,
                            Title = titulo,
                            link = link
                        });

                        InvokeAsync(StateHasChanged);
                    });

                    HubConnection.On<string>("UsersOnlineCount", (count) =>
                    {
                        CountUsersOnline = int.Parse(count);
                        InvokeAsync(StateHasChanged);
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public async Task OpenNotificationModal()
        {
            await DialogService.OpenAsync<NotificationsModal>($"Notificações",
                   new Dictionary<string, object>() { { "notifications", messages } },
                   new DialogOptions() { Width = "700px", Height = "512px", Resizable = true, Draggable = true });
        }

        private void OnStateChanged(object? sender, PropertyChangedEventArgs e) => InvokeAsync(UpdatePage);

        private void UpdatePage()
        {
            InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            if (HubConnection is not null)
            {
                HubConnection.DisposeAsync();
            }
            ViewOption.PropertyChanged -= OnStateChanged;
        }

        private bool collapseNavMenu = true;

        private string? NavMenuCssClass => collapseNavMenu ? null : null;

        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }

        // private string CalculatePadding(int nameLength)
        // {
        //     if (nameLength > 20)
        //     {
        //         return "ps-1";
        //     }
        //     else

        //         return "ps-4";
        // }


        private string NameUser(string nomeCompleto)
        {
            string[] partesNome = nomeCompleto.Split();

            if (partesNome.Length > 0)
            {
                string primeiroNome = partesNome[0];
                string ultimoNome = partesNome[partesNome.Length - 1];

                return textInfo.ToUpper(primeiroNome) + " " + textInfo.ToUpper(ultimoNome);
            }

            return string.Empty; // Se não houver partes no nome, retorna uma string vazia ou outra coisa apropriada.
        }



    }
}
