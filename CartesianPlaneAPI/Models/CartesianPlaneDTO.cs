using System.Text.Json.Serialization;


namespace CartesianPlaneAPI.Models
{
    public class CartesianPlaneDTO
    {
        [JsonPropertyName("x")]
        [JsonRequired]
        public int x { get; set; }

        [JsonPropertyName("y")]
        [JsonRequired]
        public int y { get; set; }
    }
}
