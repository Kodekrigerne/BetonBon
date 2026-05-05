using System.Text;
using System.Text.Json.Serialization;

namespace BetonBon.Client.Pages.Home
{
    public record SessionPause(DateTime PausedAt, DateTime StartedAgainAt);

    [method: JsonConstructor]
    public class StopwatchSession(DateTime startTime)
    {
        public DateTime StartTime { get; } = startTime;

        [JsonInclude]
        public List<SessionPause> SessionPauses { get; private set; } = [];

        [JsonInclude]
        public DateTime? StopTime { get; private set; } = null;

        [JsonInclude]
        public DateTime? PausedAt { get; private set; } = null;

        public void PauseSession(DateTime pausedAt)
        {
            if (PausedAt != null) throw new InvalidOperationException("Session is already paused.");
            PausedAt = pausedAt;
        }

        public void UnpauseSession(DateTime startedAgainAt)
        {
            if (PausedAt == null) throw new InvalidOperationException("Session is not paused.");
            SessionPauses.Add(new(PausedAt.Value, startedAgainAt));
            PausedAt = null;
        }

        public void SetStopTime(DateTime stopTime)
        {
            if (StopTime != null) throw new InvalidOperationException("Stop time cannot be overridden.");
            if (PausedAt != null) UnpauseSession(stopTime);
            StopTime = stopTime;
        }

        public TimeSpan GetOffset(DateTime currentTime)
        {
            var end = StopTime ?? currentTime;
            var total = end - StartTime;

            foreach (var pause in SessionPauses)
                total -= pause.StartedAgainAt - pause.PausedAt;

            if (PausedAt != null && StopTime == null)
                total -= currentTime - PausedAt.Value;

            return total;
        }

        public string GetSessionString()
        {
            static string formatDateTime(DateTime dateTime)
            {
                return $"{dateTime.ToLocalTime():dd/MM/yyyy HH:mm}";
            }

            if (StopTime == null) throw new InvalidOperationException("Can only get session data after session is finished.");

            var sb = new StringBuilder();
            sb.AppendLine($"Startet: {formatDateTime(StartTime)}");

            foreach (var pause in SessionPauses)
            {
                sb.AppendLine($"Pauset: {formatDateTime(pause.PausedAt)}");
                sb.AppendLine($"Startet igen: {formatDateTime(pause.StartedAgainAt)}");
            }

            sb.AppendLine($"Stoppet: {formatDateTime(StopTime.Value)}");

            return sb.ToString();
        }
    }
}
