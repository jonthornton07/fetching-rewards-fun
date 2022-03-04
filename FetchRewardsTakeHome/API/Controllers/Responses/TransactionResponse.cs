using System.Text.Json.Serialization;
using FetchRewardsTakeHome.Business.Repositories.Models;

namespace FetchRewardsTakeHome.API.Controllers.Responses;

public class TransactionResponse
{
    [JsonPropertyName("id")] 
    public Guid Id { get; set; }

    [JsonPropertyName("payer")] 
    public string Payer { get; set; }

    [JsonPropertyName("points")] 
    public int Points { get; set; }

    [JsonPropertyName("timestamp")] 
    public DateTime TimeStamp { get; set; }
    
    public TransactionResponse(PointsModel model)
    {
        Id = model.Id;
        Payer = model.Payer;
        Points = model.Points;
        TimeStamp = model.TimeStamp;
    }

}