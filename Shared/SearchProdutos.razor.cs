using Blazorise;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Shared_Static_Class.Converters;
using Shared_Static_Class.Data;
using Timer = System.Timers.Timer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using static System.Net.WebRequestMethods;
using System.Timers;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Components.Web;
using Shared_Razor_Components.Services;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Shared_Razor_Components.Shared
{
    public partial class SearchProdutos : ComponentBase, IDisposable
    {
        [Parameter] public string Search { get; set; } = string.Empty;
        public IEnumerable<PRODUTOS_CARDAPIO> Produtos { get; set; } = [];
        Timer _timer { get; set; } = default!;
        public bool EmptySearch { get; set; } = false;
        public bool Searching { get; set; } = false;
        public HttpClient _client { get; set; } = default!;
        public static CancellationTokenSource cancellationTokenSource { get; set; } = new CancellationTokenSource();
        [Inject] static IConfiguration _configuration { get; set; } = default!;
        [Inject] ICardapioDigitalService _service { get; set; } = default!;
        [Inject] IDialogService FluentDialog { get; set; } = default!;
        protected override void OnInitialized()
        {
            _timer = new Timer(700);
            _timer.Elapsed += OnUserFinish;
            _timer.AutoReset = false;
            _client = new HttpClient();
            base.OnInitialized();
        }

        void CleanSearch()
        {
            cancellationTokenSource.Cancel();
            Produtos = [];
            Search = string.Empty;
        }

        void ResetTimer(KeyboardEventArgs e)
        {
            _timer.Stop();
            _timer.Start();
        }

        private async void OnUserFinish(object? source, ElapsedEventArgs e)
        {
            if (Searching)
            {
                cancellationTokenSource.Cancel();
            }
            else
            {
                cancellationTokenSource = new CancellationTokenSource();
            }

            var _canceltoken = cancellationTokenSource.Token;
            Searching = true;

            try
            {
                var connection_string = "http://localhost:5159/";
                var response = await _client.GetAsync($"{connection_string}api/CardapioDigital/Search?search={Search ?? "-"}", _canceltoken);
                response.EnsureSuccessStatusCode();

                _canceltoken.ThrowIfCancellationRequested();

                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    var saida = await JsonSerializer.DeserializeAsync<IEnumerable<PRODUTOS_CARDAPIO>>(stream, cancellationToken: _canceltoken);
                    if (!saida.Any())
                    {
                        EmptySearch = true;
                    }
                    else
                    {
                        Produtos = saida;
                        EmptySearch = false;
                    }
                }

            }
            catch (TaskCanceledException) when (_canceltoken.IsCancellationRequested)
            {
                Console.WriteLine("User canceled request");
                Produtos = [];
                EmptySearch = false;
            }
            catch (TaskCanceledException)
            {
                Console.WriteLine("Request timed out");
                Produtos = [];
                EmptySearch = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                Produtos = [];
                EmptySearch = false;
            }

            Searching = false;
            await InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            _timer.Elapsed -= OnUserFinish;
        }
    }
}
