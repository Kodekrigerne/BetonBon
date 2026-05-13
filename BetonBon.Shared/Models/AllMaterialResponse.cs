using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BetonBon.Shared.Models
{
    public class AllMaterialResponse(List<MaterialDTO> materials)
    {
        [JsonPropertyName("items")]
        public List<MaterialDTO> Materials { get; set; } = materials;
    }
}
