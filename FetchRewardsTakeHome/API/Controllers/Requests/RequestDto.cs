using FetchRewardsTakeHome.Business.Repositories.Models;
using FetchRewardsTakeHome.Data.Models;

namespace FetchRewardsTakeHome.API.Controllers.Requests;

public abstract class RequestDto<TBusModel, TDataModel>
    where TDataModel : BaseDataModel
    where TBusModel : BaseBusinessModel<TDataModel>
{
    public abstract TBusModel ToBusinessModel();
}