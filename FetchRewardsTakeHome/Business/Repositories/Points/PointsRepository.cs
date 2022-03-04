using FetchRewardsTakeHome.Business.Repositories.Exceptions;
using FetchRewardsTakeHome.Business.Repositories.Models;
using FetchRewardsTakeHome.Data;

namespace FetchRewardsTakeHome.Business.Repositories.Points;

public class PointsRepository : IPointsRepository
{
    private readonly FakeDatabase _database;

    public PointsRepository(FakeDatabase database)
    {
        _database = database;
    }

    public Task<PointsModel> InsertTransaction(PointsModel points)
    {
        var dbModel = points.ToDataModel();
        _database.Transactions.Add(dbModel);
        var returnModel = new PointsModel(dbModel);
        return Task.FromResult(returnModel);
    }

    public Task<PointsModel> FindTransaction(Guid id)
    {
        var model = _database.Transactions.FirstOrDefault(t => t.Id == id);
        if (model == null) throw new TransactionNotFound(id);
        return Task.FromResult(new PointsModel(model));
    }
    
    public Task<List<PointsModel>> GetSortedTransactions()
    {
        var transactions = _database.
            Transactions.Select(t => new PointsModel(t))
            .Where(t => t.Points < 0)
            .OrderBy(t => t.TimeStamp)
            .ToList();
        
        var positiveTransactions = _database.
            Transactions.Select(t => new PointsModel(t))
            .Where(t => t.Points >= 0)
            .OrderBy(t => t.TimeStamp)
            .ToList();

        transactions.AddRange(positiveTransactions);
        return Task.FromResult(transactions);
    }
}