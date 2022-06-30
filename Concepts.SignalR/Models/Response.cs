namespace Concepts.SignalR.Models
{
    public enum RequestType
    {
        POST,
        PUT,
        DELETE
    }

    public class Response
    {
        public RequestType RequestType { get; set; }
        public string? Message { get; set; }
        public int? EntityId { get; set; }
        public UserDto? UserData { get; set; }
        public ProductDto? ProductData { get; set; }
        public bool? IsValid { get; set; }
    }
}
