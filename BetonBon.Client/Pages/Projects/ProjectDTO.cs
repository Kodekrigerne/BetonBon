using System.Text.Json.Serialization;

namespace BetonBon.Client.Pages.Projects
{
    public class ProjectDTO(int number, string name)
    {
        [JsonPropertyName("number")]
        public int Number { get; set; } = number;

        [JsonPropertyName("name")]
        public string Name { get; set; } = name;
    }
}
