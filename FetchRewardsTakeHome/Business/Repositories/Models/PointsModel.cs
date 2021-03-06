using FetchRewardsTakeHome.Data.Models;

namespace FetchRewardsTakeHome.Business.Repositories.Models;

public class PointsModel : BaseBusinessModel<PointTransactionDataModel>
{
    public string Payer { get; set; }
    public int Points { get; set; }
    public DateTime TimeStamp { get; set; }
    
    public PointsModel(string payer, int points, DateTime timeStamp)
    {
        Payer = payer;
        Points = points;
        TimeStamp = timeStamp;
    }

    public PointsModel(PointTransactionDataModel model) : base(model)
    {
        Id = model.Id;
        Payer = model.Payer;
        Points = model.Points;
        TimeStamp = model.TimeStamp;
        LastUpdated = model.LastUpdated;
        Created = model.Created;
    }
    
    public override PointTransactionDataModel ToDataModel()
    {
        return new PointTransactionDataModel
        {
            Id = Id,
            Payer = Payer,
            Points = Points,
            TimeStamp = TimeStamp,
            Created = Created,
            LastUpdated = DateTime.Now
        };
    }
}