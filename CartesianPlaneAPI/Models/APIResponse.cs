namespace CartesianPlaneAPI.Models
{
    public class APIResponse
    {
        public string msg { get; set; } //message describing the output
        public object? data { get; set; } //object containing the requested data
    }
}
