using BetonBon.Shared.Models;
using Refit;

namespace BetonBon.API.RefitInterfaces
{
    public interface IEconomicJournalsRelayApi
    {

        [Post("/draft-entries")]
        Task<EntryNumberResponseDTO> PostNewEntryAsync(NewDraftEntryDTO newDraftEntry);

        [Post("/journals/1/bookdraftentries")]
        Task<HttpResponseMessage> BookDraftEntryAsync(BookEntryNumberDTO entryNumberDTO);
    }
}
