using System.Text.Json.Serialization;

namespace FetchRewardsTakeHome.API.Controllers.Responses;

public class StandardBadResponse
{
    public StandardBadResponse(Exception e)
    {
        Message = e.Message;
    }

    [JsonPropertyName("message")] public string Message { get; set; }
}