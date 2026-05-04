using Microsoft.AspNetCore.Components;

namespace BetonBon.Client.Pages
{
    public partial class Home
    {
        [Inject]
        public NavigationManager Navigation { get; set; } = default!;

        private enum TimerState { NotStarted, Running }
        private TimerState _timerState = TimerState.NotStarted;
        private DateTime? _startedAt = null;
        private Timer? _timer;

    }
}
