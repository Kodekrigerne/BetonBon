using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BetonBon.Shared.Models
{
    public class AllProjectActivitiesResponse
    {
        public AllProjectActivitiesResponse(List<ProjectActivityDTO> projectActivities)
        {
            ProjectActivities = projectActivities;
        }

        [JsonPropertyName("items")]
        public List<ProjectActivityDTO> ProjectActivities{ get; set; }
    }
}
