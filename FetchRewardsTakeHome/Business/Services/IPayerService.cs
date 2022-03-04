using FetchRewardsTakeHome.Business.Repositories.Exceptions;
using FetchRewardsTakeHome.Business.Repositories.Models;

namespace FetchRewardsTakeHome.Business.Services;

public interface IPayerService
{
    /// <summary>
    /// Get the balances of all the payers
    /// </summary>
    /// <returns>The payers and their balances</returns>
    public Task<List<PayerModel>> GetPayerBalances();
}