using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BetonBon.Shared.Models
{
    public class AllActivitiesResponse(List<ActivityDTO> activities)
    {
        [JsonPropertyName("items")]
        public List<ActivityDTO> Activities { get; set; } = activities;
    }
}
