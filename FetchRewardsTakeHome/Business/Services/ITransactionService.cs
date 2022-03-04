using FetchRewardsTakeHome.Business.Repositories.Exceptions;
using FetchRewardsTakeHome.Business.Repositories.Models;

namespace FetchRewardsTakeHome.Business.Services;

public interface ITransactionService
{
    /// <summary>
    ///     Add a given transaction
    /// </summary>
    /// <param name="points">Transaction to add</param>
    /// <returns>The newly added transaction</returns>
    public Task<PointsModel> AddTransaction(PointsModel points);

    /// <summary>
    ///     Lookup a given transaction
    /// </summary>
    /// <param name="guid">Guid of the transaction to look up</param>
    /// <returns>The given transaction</returns>
    /// <exception cref=""></exception>
    public Task<PointsModel> LookupTransaction(Guid guid);

    /// <summary>
    /// Spend a number of points
    /// </summary>
    /// <param name="points">Number of points to spend</param>
    /// <returns>The list of transactions</returns>
    /// <exception cref="NotEnoughPoints"></exception>
    public Task<List<PointsModel>> Spend(int points);
}