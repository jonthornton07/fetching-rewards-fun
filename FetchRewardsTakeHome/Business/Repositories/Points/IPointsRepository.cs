using FetchRewardsTakeHome.Business.Repositories.Exceptions;
using FetchRewardsTakeHome.Business.Repositories.Models;

namespace FetchRewardsTakeHome.Business.Repositories.Points;

public interface IPointsRepository
{
    
    /// <summary>
    /// Insert a transaction
    /// </summary>
    /// <param name="points">Transaction to insert</param>
    /// <returns>The inserted transaction to be used going forward</returns>
    public Task<PointsModel> InsertTransaction(PointsModel points);

    /// <summary>
    /// Find a given transaction
    /// </summary>
    /// <param name="id">ID of the transaction to look up</param>
    /// <returns>The given transaction or null if the transaction does not exsit</returns>
    public Task<PointsModel?> FindTransaction(Guid id);
    
    /// <summary>
    /// Returns a list of sorted transactions
    ///     - The sorting starts with negative transactions first
    ///     - Each negative and positive set of transactions is sorted by date
    /// </summary>
    /// <returns>The sorted transactions</returns>
    public Task<List<PointsModel>> GetSortedTransactions();
}