using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FetchRewardsTakeHome.API.Controllers.Requests;

public class SpendRequest
{
    [JsonPropertyName("points")]
    [Required]
    public int Points { get; set; }
}