using System.Diagnostics;

namespace BetonBon.Client.Pages.Home
{
    public partial class TimerCard : IAsyncDisposable
    {
        private enum TimerState { NotStarted, Paused, Running }
        private TimerState _timerState = TimerState.NotStarted;
        private Stopwatch? _stopwatch;

        private PeriodicTimer? _periodicTimer;

        private void StartTimer()
        {
            _timerState = TimerState.Running;
            _stopwatch = new();
            _stopwatch.Start();
            _ = StartTicking();
        }

        private void StopTimer()
        {
            _timerState = TimerState.NotStarted;
            _stopwatch?.Stop();
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

            var ts = _stopwatch.Elapsed;
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
