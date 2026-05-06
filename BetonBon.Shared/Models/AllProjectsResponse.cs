using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace BetonBon.Shared.Models
{
    public class AllProjectsResponse
    {
        [JsonPropertyName("items")]
        public List<ProjectDTO> Projects { get; set; }

        public AllProjectsResponse(List<ProjectDTO> projects)
        {
            Projects = projects;
        }
    }
}
