using FetchRewardsTakeHome.Business.Repositories.Exceptions;
using FetchRewardsTakeHome.Business.Repositories.Models;

namespace FetchRewardsTakeHome.Business.Repositories.Payer;

public interface IPayerRepository
{

    /// <summary>
    /// Get all the payers
    /// </summary>
    /// <returns>All the payers</returns>
    public Task<List<PayerModel>> GetAll();

    /// <summary>
    ///     Find a payer by name
    /// </summary>
    /// <param name="name">name of the payer to find</param>
    /// <returns>payer if exists</returns>
    /// <exception cref="PayerNotFound">throws exception if payer does not not exist</exception>
    public Task<PayerModel> FindPayerByName(string name);

    /// <summary>
    ///     Insert a new payer
    /// </summary>
    /// <param name="payer">payer to insert</param>
    /// <returns>the newly created payer which should be used going forward</returns>
    public Task<PayerModel> InsertNewPayer(PayerModel payer);

    /// <summary>
    ///     Add a transaction to a payer
    /// </summary>
    /// <param name="payer">payer to add the transaction</param>
    /// <param name="points">points to add to the payer</param>
    /// <returns>the updated payer</returns>
    /// <exception cref="PayerNotFound"></exception>
    public Task<PayerModel> AddPoints(PayerModel payer, int points);
}