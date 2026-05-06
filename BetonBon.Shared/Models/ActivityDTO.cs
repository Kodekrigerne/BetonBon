using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BetonBon.Shared.Models
{
    public class ActivityDTO(int number, string name)
    {
        [JsonPropertyName("number")]
        public int Number { get; set; } = number;

        [JsonPropertyName("name")]
        public string Name { get; set; } = name;
    }
}
