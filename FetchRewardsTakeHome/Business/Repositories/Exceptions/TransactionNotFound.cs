namespace FetchRewardsTakeHome.Business.Repositories.Exceptions;

public class TransactionNotFound : Exception
{
    /// <summary>
    ///     Create an exception
    /// </summary>
    /// <param name="guid">The ID of the transaction</param>
    public TransactionNotFound(Guid guid) : base($"Transaction {guid} is not found")
    {
    }
}