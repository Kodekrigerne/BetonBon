using BetonBon.Shared.Models;
using Refit;

namespace BetonBon.API.RefitInterfaces
{
    public interface IEconomicRelayApi
    {
        [Get("/Projects?cursor=0")]
        Task<AllProjectsResponse> GetProjectsAsync();
        
        [Get("/project-activities?filter=projectNumber$eq:{projectnumber}")]
        Task<AllProjectActivitiesResponse> GetProjectActivitiesAsync(int projectnumber);
        
        [Get("/Activities/{number}")]
        Task<ActivityDTO> GetActivityByNumberAsync(int number);
    }
}
