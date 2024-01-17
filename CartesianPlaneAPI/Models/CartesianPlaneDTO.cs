using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace CartesianPlaneAPI.Models
{
    public class CartesianPlaneDTO
    {
        [JsonRequired]
        [JsonPropertyName("x")]
        public int x { get; set; }

        [JsonPropertyName("y")]
        [JsonRequired]
        public int y { get; set; }
    }
}
