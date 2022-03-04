using FetchRewardsTakeHome.Data.Models;

namespace FetchRewardsTakeHome.Business.Repositories.Models;

public abstract class BaseBusinessModel<TDataModel> where TDataModel : BaseDataModel
{
    protected BaseBusinessModel()
    {
        Id = Guid.NewGuid();
        LastUpdated = DateTime.Now;
        Created = DateTime.Now;
    }

    protected BaseBusinessModel(TDataModel model)
    {
    }

    public Guid Id { get; set; }
    public DateTime LastUpdated { get; set; }
    public DateTime Created { get; set; }

    public abstract TDataModel ToDataModel();
}