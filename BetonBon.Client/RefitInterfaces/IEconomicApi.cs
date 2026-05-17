using BetonBon.Shared.Models;
using BetonBon.Shared.Models.TimeEntries;
using Refit;

namespace BetonBon.Client.RefitInterfaces
{
    public interface IEconomicApi
    {
        [Get("/api/projects")]
        Task<List<ProjectDTO>> GetAllProjectsAsync();

        [Get("/api/activitiesByProjectNumber")]
        Task<List<ActivityDTO>> GetAllActivitiesByProjectAsync(int projectNumber);

        [Get("/api/materials")]
        Task<List<MaterialDTO>> GetAllMaterialsAsync();

        [Post("/api/newDraftEntry")]
        Task<HttpResponseMessage> CreateNewDraftEntry(NewDraftEntryDTO newDraftEntry);

        [Post("/api/newTimeEntry")]
        Task<IApiResponse<CreatedTimeEntryResult>> CreateTimeEntryAsync([Body] TimeEntry timeEntry, CancellationToken cancellationToken = default);
    }
}
