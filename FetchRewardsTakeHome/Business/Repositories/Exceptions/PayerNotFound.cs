namespace FetchRewardsTakeHome.Business.Repositories.Exceptions;

public class PayerNotFound : Exception
{
    /// <summary>
    ///     Create an exception
    /// </summary>
    /// <param name="payerName">the name of the payer being searched for</param>
    public PayerNotFound(string payerName) : base($"Payer with {payerName} is not found")
    {
    }
}