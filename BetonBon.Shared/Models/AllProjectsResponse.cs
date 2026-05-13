using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BetonBon.Shared.Models
{
    public class AllProjectsResponse(List<ProjectDTO> projects)
    {
        [JsonPropertyName("items")]
        public List<ProjectDTO> Projects { get; set; } = projects;
    }
}
