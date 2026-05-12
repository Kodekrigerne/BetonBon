using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BetonBon.Shared.Models
{
    public class EntryNumberResponseDTO
    {
        [JsonPropertyName("entryNumber")]
        public int CreatedEntryNumber { get; set; }
    }
}
