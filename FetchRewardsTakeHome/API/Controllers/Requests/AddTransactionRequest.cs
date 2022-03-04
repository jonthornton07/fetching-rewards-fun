using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using FetchRewardsTakeHome.Business.Repositories.Models;
using FetchRewardsTakeHome.Data.Models;

namespace FetchRewardsTakeHome.API.Controllers.Requests;

public class AddTransactionRequest : RequestDto<PointsModel, TransactionDataModel>
{
    [JsonPropertyName("payer")] 
    [Required] public string Payer { get; set; }

    [JsonPropertyName("points")]
    [Required]
    public int Points { get; set; }

    [JsonPropertyName("timestamp")]
    [Required]
    public DateTime TimeStamp { get; set; }

    public override PointsModel ToBusinessModel()
    {
        return new PointsModel(Payer, Points, TimeStamp);
    }
}