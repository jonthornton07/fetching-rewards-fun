using FetchRewardsTakeHome.Data.Models;

namespace FetchRewardsTakeHome.Business.Repositories.Models;

public class PayerModel : BaseBusinessModel<PayerDataModel>
{
    public PayerModel(string payer)
    {
        Payer = payer;
        Points = 0;
    }

    public PayerModel(PayerDataModel model) : base(model)
    {
        Id = model.Id;
        Payer = model.Payer;
        Points = model.AvailablePoints;
    }

    public string Payer { get; set; }
    public int Points { get; set; }

    public override PayerDataModel ToDataModel()
    {
        return new PayerDataModel
        {
            Id = Id,
            Payer = Payer,
            AvailablePoints = Points,
            Created = Created,
            LastUpdated = DateTime.Now
        };
    }
}