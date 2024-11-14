using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.FluentUI.AspNetCore.Components;
using Radzen.Blazor;
using Shared_Static_Class.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using System.Timers;
using Timer = System.Timers.Timer;
using Blazorise;
using static Shared_Razor_Components.VivoCustomComponents.VivoTimer;


namespace Shared_Razor_Components.VivoCustomComponents
{
    public partial class VivoTimer : ComponentBase
    {
        [Parameter] public string? Id { get; set; }
        [Parameter] public string? Style { get; set; }
        [Parameter] public string? CssClass { get; set; }
        [Parameter] public string BackGroundColor { get; set; } = "#7719aa";
        [Parameter] public TimerKind timerkind { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
        IJSObjectReference _jsmodule { get; set; } = default!;
        Timer _timer { get; set; }
        Random rnd { get; set; }
        DateTime hour_init { get; set; }
        private string Hash { get; set; } = string.Empty;
        protected override async Task OnInitializedAsync()
        {
            hour_init = DateTime.Now;
            _timer = new Timer(TimeSpan.FromSeconds(1));
            _timer.Elapsed += OnTimerElapsed;
            _timer.AutoReset = true;
            _timer.Start();

            _jsmodule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Shared_Razor_Components/VivoCustomComponents/VivoTimer.razor.js");
            string guidString = Guid.NewGuid().ToString("N").ToUpper();
            string alphabetLetters = new string(guidString.Where(c => char.IsLetter(c)).ToArray());
            Hash = alphabetLetters;

            await base.OnInitializedAsync();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e)
        {
            //Console.WriteLine(Porcentage);
            var value = 0;
            double _timeIntervalSeconds = 0;
            switch (timerkind)
            {
                case TimerKind.Days:
                    _timeIntervalSeconds = TimeSpan.FromDays(1).TotalSeconds;
                    //value = ;
                    break;
                case TimerKind.Hours:
                    _timeIntervalSeconds = TimeSpan.FromHours(1).TotalSeconds;
                    break;
                case TimerKind.Minutes:
                    _timeIntervalSeconds = TimeSpan.FromMinutes(1).TotalSeconds;
                    break;
                case TimerKind.Seconds:
                    _timeIntervalSeconds = TimeSpan.FromSeconds(1).TotalSeconds;
                    break;
                _:
                    break;
            };
            //(int)Math.Round((timeElapsedSinceLastTick / _timeIntervalSeconds) * 100, 0);
            InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public enum TimerKind
        {
            Seconds,
            Minutes,
            Hours,
            Days
        }
    }
}
