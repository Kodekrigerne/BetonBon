using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BetonBon.Shared.Models
{
    public class MaterialDTO(int id, string name, string unit)
    {
        [JsonPropertyName("number")]
        public int Id { get; set; } = id;
        [JsonPropertyName("name")]
        public string Name { get; set; } = name;
        public string Unit { get; set; } = unit;
    }
}
