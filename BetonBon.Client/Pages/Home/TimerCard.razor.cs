using System.Diagnostics;
using System.Text.Json;
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
        private Stopwatch? _stopwatch = null;

        private StopwatchSession? _session = null;
        private TimeSpan _offset = TimeSpan.Zero;

        private PeriodicTimer? _periodicTimer;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var json = await JS.InvokeAsync<string?>("storage.load", "bb_timer");
                if (json != null)
                {
                    var session = JsonSerializer.Deserialize<StopwatchSession>(json)
                        ?? throw new InvalidOperationException("Invalid stopwatch session data.");

                    _session = session;

                    if (_session.PausedAt == null) await StartTimer();
                    else
                    {
                        _timerState = TimerState.Paused;
                        _stopwatch = new();
                        _ = StartTicking();
                    }

                    _offset = session.GetOffset(DateTime.UtcNow);
                }
            }
        }

        private async Task StartTimer()
        {
            _timerState = TimerState.Running;
            _stopwatch = new();
            _stopwatch.Start();
            _offset = TimeSpan.Zero;
            _session ??= new(DateTime.UtcNow);
            await SaveSession();
            _ = StartTicking();
        }

        /// <summary>
        /// Currently clears the timer and session and readies for a new run.
        /// When the time registration flow is integrated this should instead use the session to initiate that flow.
        /// </summary>
        private async Task StopTimer()
        {
            _timerState = TimerState.NotStarted;
            _stopwatch?.Stop();
            _session?.SetStopTime(DateTime.UtcNow);
            _session = null;
            StopTicking();
            await JS.InvokeVoidAsync("storage.remove", "bb_timer");
        }

        private async Task PauseTimer()
        {
            _timerState = TimerState.Paused;
            _stopwatch?.Stop();
            _session?.PauseSession(DateTime.UtcNow);
            StopTicking();
            await SaveSession();
        }

        private async Task UnpauseTimer()
        {
            _timerState = TimerState.Running;
            _stopwatch?.Start();
            _session?.UnpauseSession(DateTime.UtcNow);
            _ = StartTicking();
            await SaveSession();
        }

        private async Task StartTicking()
        {
            _periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(1));

            while (await _periodicTimer.WaitForNextTickAsync())
            {
                StateHasChanged();
            }
        }

        private void StopTicking()
        {
            _periodicTimer?.Dispose();
            _periodicTimer = null;
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

        private async Task SaveSession()
        {
            var json = JsonSerializer.Serialize(_session);
            await JS.InvokeVoidAsync("storage.save", "bb_timer", json);
        }

        public async ValueTask DisposeAsync()
        {
            _periodicTimer?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
