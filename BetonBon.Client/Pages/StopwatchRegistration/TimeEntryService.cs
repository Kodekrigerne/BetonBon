using BetonBon.Client.Pages.Home;
using BetonBon.Client.RefitInterfaces;
using BetonBon.Shared.Models.TimeEntries;
using Refit;

namespace BetonBon.Client.Pages.StopwatchRegistration
{
    public class TimeEntryService
    {
        private readonly IEconomicRelayApi _economicApi;

        public TimeEntryService(IEconomicRelayApi economicApi)
        {
            ArgumentNullException.ThrowIfNull(economicApi);
            _economicApi = economicApi;
        }

        public async Task<IEnumerable<IApiResponse<CreatedTimeEntryResult>>> CreateTimeEntries(StopwatchSession session, IEnumerable<AllocationDraft> drafts)
        {
            //var text = session.GetSessionString();
            var date = new DateTimeOffset(session.StartTime);

            var responses = new List<IApiResponse<CreatedTimeEntryResult>>();
            foreach (var draft in drafts)
            {
                var timeEntry = new TimeEntry()
                {
                    ProjectNumber = draft.ProjectDTO!.Number,
                    ActivityNumber = draft.ActivityDTO!.Number,
                    EmployeeNumber = 1, //TODO: Get employeenumber from user profile
                    //Text = text,
                    NumberOfHours = draft.Minutes / 60d,
                    Date = date
                };
                responses.Add(await _economicApi.CreateTimeEntryAsync(timeEntry));
            }

            return responses;
        }
    }
}
