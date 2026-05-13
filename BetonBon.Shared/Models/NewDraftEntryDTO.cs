using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BetonBon.Shared.Models
{
    public class NewDraftEntryDTO(string date, double amount, int projectNumber, int costTypeNumber, string note)
    {
        [JsonPropertyName("date")]
        public string Date { get; set; } = date;
        [JsonPropertyName("entryTypeNumber")]
        public int EntryTypeNumber { get; set; } = 5;
        [JsonPropertyName("journalNumber")]
        public int JournalNumber { get; set; } = 1;
        [JsonPropertyName("amount")]
        public double Amount { get; set; } = amount;
        [JsonPropertyName("projectNumber")]
        public int ProjectNumber { get; set; } = projectNumber;
        [JsonPropertyName("costTypeNumber")]
        public int CostTypeNumber { get; set; } = costTypeNumber;
        [JsonPropertyName("accountNumber")]
        public int AccountNumber { get; set; } = 1025;
        [JsonPropertyName("currency")]
        public string Currency { get; set; } = "DKK";
        [JsonPropertyName("text")]
        public string Note { get; set; } = note;
    }
}
