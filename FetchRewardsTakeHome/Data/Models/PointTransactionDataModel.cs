namespace FetchRewardsTakeHome.Data.Models;

public class PointTransactionDataModel : BaseDataModel
{
    public string Payer { get; set; }
    public int Points { get; set; }
    public DateTime TimeStamp { get; set; }
}