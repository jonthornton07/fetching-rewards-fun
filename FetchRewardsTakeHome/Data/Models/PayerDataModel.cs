using FetchRewardsTakeHome.Business.Repositories.Models;

namespace FetchRewardsTakeHome.Data.Models;

public class PayerDataModel : BaseDataModel
{
    public string Payer { get; set; }
    public int AvailablePoints { get; set; }
}