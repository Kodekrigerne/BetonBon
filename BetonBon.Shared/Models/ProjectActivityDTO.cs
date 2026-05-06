using System.Text.Json.Serialization;

namespace BetonBon.Shared.Models
{
    public class ProjectActivityDTO
    {
        [JsonPropertyName("activityNumber")]
        public int ActivityNumber { get; set; }
    }
}