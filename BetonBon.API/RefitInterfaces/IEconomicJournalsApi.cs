using BetonBon.Shared.Models.DraftEntries;
using Refit;

namespace BetonBon.API.RefitInterfaces
{
    public interface IEconomicJournalsApi
    {

        [Post("/draft-entries")]
        Task<EntryNumberResponseDTO> PostNewEntryAsync(NewDraftEntryDTO newDraftEntry, CancellationToken cancellationToken = default);

        [Post("/journals/1/bookdraftentries")]
        Task<HttpResponseMessage> BookDraftEntryAsync(BookEntryNumberDTO entryNumberDTO, CancellationToken cancellationToken = default);
    }
}
