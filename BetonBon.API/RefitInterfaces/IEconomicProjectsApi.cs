using BetonBon.Shared.Models;
using BetonBon.Shared.Models.TimeEntries;
using Refit;

namespace BetonBon.API.RefitInterfaces
{
    public interface IEconomicProjectsApi
    {
        [Get("/Projects?cursor=0")]
        Task<AllProjectsResponse> GetProjectsAsync();

        [Get("/project-activities?filter=projectNumber$eq:{projectnumber}")]
        Task<AllProjectActivitiesResponse> GetProjectActivitiesAsync(int projectnumber);

        [Get("/Activities/{number}")]
        Task<ActivityDTO> GetActivityByNumberAsync(int number);

        [Get("/CostTypes?filter=isBarred$eq:false")]
        Task<AllMaterialResponse> GetAllMaterialsAsync();

        [Post("/TimeEntries")]
        Task<CreatedTimeEntryResult> CreateTimeEntryAsync(TimeEntry timeEntry, CancellationToken cancellationToken = default);

        [Get("/Employees")]
        Task<CursorResults<Employee>> GetEmployeesAsync([Query] string? cursor = null, CancellationToken cancellationToken = default);
    }
}
