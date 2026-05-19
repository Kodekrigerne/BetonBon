using BetonBon.Shared.Models;
using BetonBon.Shared.Models.TimeEntries;
using Refit;

namespace BetonBon.Client.RefitInterfaces
{
    public interface IEconomicRelayApi
    {
        [Get("/api/projects")]
        Task<List<ProjectDTO>> GetAllProjectsAsync(CancellationToken cancellation = default);

        [Get("/api/activitiesByProjectNumber")]
        Task<List<ActivityDTO>> GetAllActivitiesByProjectAsync(int projectNumber);

        [Get("/api/materials")]
        Task<List<MaterialDTO>> GetAllMaterialsAsync();

        [Post("/api/newDraftEntry")]
        Task<HttpResponseMessage> CreateNewDraftEntry(NewDraftEntryDTO newDraftEntry);

        [Post("/api/newTimeEntry")]
        Task<IApiResponse<CreatedTimeEntryResult>> CreateTimeEntryAsync([Body] TimeEntry timeEntry, CancellationToken cancellationToken = default);

        [Get("/api/employees")]
        Task<CursorResults<Employee>> GetEmployeesAsync([Query] string? cursor = null, CancellationToken cancellationToken = default);

        [Get("/api/timeEntries")]
        Task<List<TimeEntry>> GetTimeEntriesAsync([Query] int projectNumber, int activityNumber, int employeeNumber, CancellationToken cancellation = default);
    }
}
