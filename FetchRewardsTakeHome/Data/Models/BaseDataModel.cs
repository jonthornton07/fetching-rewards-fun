namespace FetchRewardsTakeHome.Data.Models;

public abstract class BaseDataModel
{
    public Guid Id { get; set; }
    public DateTime LastUpdated { get; set; }
    public DateTime Created { get; set; }
}