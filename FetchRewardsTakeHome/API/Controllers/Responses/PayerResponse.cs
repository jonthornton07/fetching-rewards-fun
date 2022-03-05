using System.Text.Json.Serialization;
using FetchRewardsTakeHome.Business.Repositories.Models;

namespace FetchRewardsTakeHome.API.Controllers.Responses;

public class PayerResponse
{
    [JsonPropertyName("id")] 
    public Guid Id { get; set; }
    
    [JsonPropertyName("payer")] 
    public string Payer { get; set; }

    [JsonPropertyName("points")] 
    public int Points { get; set; }

    public PayerResponse(PayerModel model)
    {
        Payer = model.Payer;
        Points = model.Points;
        Id = model.Id;
    }
}