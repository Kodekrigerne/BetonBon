using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BetonBon.Shared.Models
{
    public class JournalResponseDTO(int number, int nextVoucherNumber)
    {
        [JsonPropertyName("number")]
        public int Number { get; set; } = number;
        [JsonPropertyName("nextVoucherNumber")]
        public int NextVoucherNumber { get; set; } = nextVoucherNumber;
    }
}
