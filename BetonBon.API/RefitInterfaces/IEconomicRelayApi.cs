using BetonBon.Shared.Models;
using Refit;

namespace BetonBon.API.RefitInterfaces
{
    public interface IEconomicRelayApi
    {
        [Get("/Projects?cursor=0")]
        Task<AllProjectsResponse> GetProjectsAsync();
    }
}
