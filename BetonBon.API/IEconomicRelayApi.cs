using Refit;

namespace BetonBon.API
{
    public interface IEconomicRelayApi
    {
        [Get("/Projects?cursor=0")]
        Task<string> GetRawProjectsAsync();
    }
}
