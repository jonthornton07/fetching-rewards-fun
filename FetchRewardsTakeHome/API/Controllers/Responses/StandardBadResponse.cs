using System.Text.Json.Serialization;

namespace FetchRewardsTakeHome.API.Controllers.Responses;

public class StandardBadResponse
{

    [JsonPropertyName("message")] public string Message { get; set; }
    
    public StandardBadResponse(Exception e)
    {
        Message = e.Message;
    }
    
    public StandardBadResponse(string message)
    {
        Message = message;
    }
}