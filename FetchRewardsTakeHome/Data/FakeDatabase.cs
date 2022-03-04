using System.Collections.Concurrent;
using FetchRewardsTakeHome.Data.Models;

namespace FetchRewardsTakeHome.Data;

public class FakeDatabase
{
    public FakeDatabase()
    {
        Payers = new ConcurrentBag<PayerDataModel>();
        Transactions = new ConcurrentBag<TransactionDataModel>();
    }

    public ConcurrentBag<PayerDataModel> Payers { get; set; }
    public ConcurrentBag<TransactionDataModel> Transactions { get; set; }
}