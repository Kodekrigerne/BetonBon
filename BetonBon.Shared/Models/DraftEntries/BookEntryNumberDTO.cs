using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BetonBon.Shared.Models.DraftEntries
{
    public sealed record BookEntryNumberDTO
    {
        public required int[] EntryNumbers { get; init; }
    }
}
