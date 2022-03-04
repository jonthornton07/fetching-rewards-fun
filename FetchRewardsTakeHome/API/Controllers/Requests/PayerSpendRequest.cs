using System.Text.Json.Serialization;

namespace FetchRewardsTakeHome.API.Controllers.Requests;

public class PayerSpendRequest
{
    [JsonPropertyName("points")] public int Points { get; set; }
}