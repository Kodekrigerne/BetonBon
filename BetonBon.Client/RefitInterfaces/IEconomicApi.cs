using BetonBon.Shared.Models;
using Refit;

namespace BetonBon.Client.RefitInterfaces
{
    public interface IEconomicApi
    {
        [Get("/api/projects")]
        Task<List<ProjectDTO>> GetAllProjectsAsync();

        [Get("/api/activitiesByProjectNumber")]
        Task<List<ActivityDTO>> GetAllActivitiesByProjectAsync(int projectNumber);
    }
}
