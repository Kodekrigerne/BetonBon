using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BetonBon.Shared.Models.DraftEntries
{
    public sealed record NewDraftEntryDTO
    {
        public required string Date { get; init; }
        public int EntryTypeNumber { get; init; } = 5;
        public int JournalNumber { get; init; } = 1; 
        public required double Amount { get; init; } 
        public required int ProjectNumber { get; init; }
        public required int CostTypeNumber { get; init; }
        public int AccountNumber { get; init; } = 1025;
        public string Currency { get; init; } = "DKK";
        public string? Text { get; init; } 
    }
}
