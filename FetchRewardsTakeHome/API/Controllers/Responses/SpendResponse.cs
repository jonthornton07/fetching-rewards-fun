using System.Text.Json.Serialization;
using FetchRewardsTakeHome.Business.Repositories.Models;

namespace FetchRewardsTakeHome.API.Controllers.Responses;

public class SpendResponse
{
    [JsonPropertyName("payer")]
    public string Payer { get; set; }
    
    [JsonPropertyName("points")]
    public int Points { get; set; }

    public SpendResponse(PointsModel model)
    {
        Payer = model.Payer;
        Points = model.Points;
    }
}