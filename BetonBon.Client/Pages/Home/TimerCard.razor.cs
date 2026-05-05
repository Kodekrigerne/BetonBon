using System.Diagnostics;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BetonBon.Client.Pages.Home
{
    public partial class TimerCard : IAsyncDisposable
    {
        [Inject]
        private IJSRuntime JS { get; set; } = default!;

        private enum TimerState { NotStarted, Paused, Running }
        private TimerState _timerState = TimerState.NotStarted;
        private Stopwatch? _stopwatch;
        private TimeSpan _offset = TimeSpan.Zero;

        private PeriodicTimer? _periodicTimer;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var saved = await JS.InvokeAsync<string?>("storage.load", "timer_start");
                if (saved != null && DateTime.TryParse(saved, null, System.Globalization.DateTimeStyles.RoundtripKind, out var startTime))
                {
                    _offset = DateTime.UtcNow - startTime;
                    await StartTimer();
                }
            }
        }

        private async Task StartTimer()
        {
            _timerState = TimerState.Running;
            _stopwatch = new();
            _stopwatch.Start();
            await JS.InvokeVoidAsync("storage.save", "timer_start", DateTime.UtcNow.ToString("o"));
            _ = StartTicking();
        }

        private async Task StopTimer()
        {
            _timerState = TimerState.NotStarted;
            _stopwatch?.Stop();
            _offset = TimeSpan.Zero;
            await JS.InvokeVoidAsync("storage.remove", "timer_start");
            _periodicTimer?.Dispose();
        }

        private void PauseTimer()
        {
            _timerState = TimerState.Paused;
            _stopwatch?.Stop();
        }

        private void UnpauseTimer()
        {
            _timerState = TimerState.Running;
            _stopwatch?.Start();
        }

        private async Task StartTicking()
        {
            _periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(1));

            while (await _periodicTimer.WaitForNextTickAsync())
            {
                StateHasChanged();
            }
        }

        private string FormatElapsed()
        {
            if (_stopwatch == null) return "0s";

            var ts = _stopwatch.Elapsed + _offset;
            if (ts.TotalHours >= 1)
                return $"{(int)ts.TotalHours}t {ts.Minutes}m {ts.Seconds}s";
            if (ts.TotalMinutes >= 1)
                return $"{ts.Minutes}m {ts.Seconds}s";
            return $"{ts.Seconds}s";
        }

        public async ValueTask DisposeAsync()
        {
            _periodicTimer?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
