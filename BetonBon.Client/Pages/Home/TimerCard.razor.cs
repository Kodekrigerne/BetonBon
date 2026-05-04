namespace BetonBon.Client.Pages.Home
{
    public partial class TimerCard
    {
        private enum TimerState { NotStarted, Running }
        private TimerState _timerState = TimerState.NotStarted;
        private DateTime? _startedAt = null;
        private Timer? _timer;
    }
}
