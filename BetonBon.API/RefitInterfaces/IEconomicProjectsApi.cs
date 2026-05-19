using BetonBon.Shared.Models;
using BetonBon.Shared.Models.TimeEntries;
using Refit;

namespace BetonBon.API.RefitInterfaces
{
    public interface IEconomicProjectsApi
    {
        [Get("/Projects")]
        Task<CursorResults<ProjectDTO>> GetProjectsAsync(CancellationToken cancellation = default);

        [Get("/project-activities?filter=projectNumber$eq:{projectnumber}")]
        Task<CursorResults<ProjectActivityDTO>> GetProjectActivitiesAsync(int projectnumber, CancellationToken cancellationToken = default);

        [Get("/Activities/{number}")]
        Task<ActivityDTO> GetActivityByNumberAsync(int number, CancellationToken cancellationToken = default);

        [Get("/CostTypes?filter=isBarred$eq:false")]
        Task<CursorResults<MaterialDTO>> GetAllMaterialsAsync(CancellationToken cancellationToken = default);

        [Post("/TimeEntries")]
        Task<CreatedTimeEntryResult> CreateTimeEntryAsync(TimeEntry timeEntry, CancellationToken cancellationToken = default);

        [Get("/Employees")]
        Task<CursorResults<Employee>> GetEmployeesAsync([Query] string? cursor = null, CancellationToken cancellationToken = default);

        [Get("/TimeEntries?filter=projectNumber$eq:{projectNumber}$and:activityNumber$eq:{activityNumber}$and:employeeNumber$eq:{employeeNumber}")]
        Task<CursorResults<TimeEntry>> GetTimeEntriesAsync([Query] int projectNumber, int activityNumber, int employeeNumber, CancellationToken cancellation = default);
    }
}
