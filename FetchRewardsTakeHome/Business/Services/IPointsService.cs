using FetchRewardsTakeHome.Business.Repositories.Exceptions;
using FetchRewardsTakeHome.Business.Repositories.Models;

namespace FetchRewardsTakeHome.Business.Services;

public interface IPointsService
{
    /// <summary>
    ///     Add a given points transaction
    /// </summary>
    /// <param name="points">Points to add</param>
    /// <returns>The newly added points transaction</returns>
    public Task<PointsModel?> AddPoints(PointsModel points);

    /// <summary>
    ///     Lookup a given points transaction
    /// </summary>
    /// <param name="guid">Guid of the transaction to look up</param>
    /// <returns>The given points transaction or null if it does not exist</returns>
    public Task<PointsModel?> LookupPointsInformation(Guid guid);

    /// <summary>
    ///     Spend a number of points
    /// </summary>
    /// <param name="points">Number of points to spend</param>
    /// <returns>The list of transactions</returns>
    /// <exception cref="NotEnoughPoints"></exception>
    public Task<List<PointsModel>> Spend(int points);
}