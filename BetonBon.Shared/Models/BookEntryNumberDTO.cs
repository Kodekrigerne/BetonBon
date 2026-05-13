using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BetonBon.Shared.Models
{
    public class BookEntryNumberDTO(int[] entryNumbers)
    {
        [JsonPropertyName("entryNumbers")]
        public int[] EntryNumbers { get; set; } = entryNumbers;
    }
}
