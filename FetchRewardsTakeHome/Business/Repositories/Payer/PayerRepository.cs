using FetchRewardsTakeHome.Business.Repositories.Exceptions;
using FetchRewardsTakeHome.Business.Repositories.Models;
using FetchRewardsTakeHome.Data;

namespace FetchRewardsTakeHome.Business.Repositories.Payer;

public class PayerRepository : IPayerRepository
{
    private readonly FakeDatabase _database;

    public PayerRepository(FakeDatabase database)
    {
        _database = database;
    }

    public Task<PayerModel> FindPayerByName(string name)
    {
        var foundPayer = _database.Payers.FirstOrDefault(p => p.Payer == name);
        if (foundPayer == null)
        {
            throw new PayerNotFound(name);
        }
        return Task.FromResult(new PayerModel(foundPayer));
    }

    public Task<PayerModel> InsertNewPayer(PayerModel model)
    {
        var dbModel = model.ToDataModel();
        _database.Payers.Add(dbModel);
        return Task.FromResult(new PayerModel(dbModel));
    }

    public Task<List<PayerModel>> GetAll()
    {
        return Task.FromResult(_database.Payers.Select(p => new PayerModel(p)).ToList());
    }

    public Task<PayerModel> AddPoints(PayerModel payer, int points)
    {
        var model = _database.Payers.FirstOrDefault(p => p.Payer == payer.Payer);
        if (model == null) throw new PayerNotFound(payer.Payer);
        model.AvailablePoints += points;
        return Task.FromResult(new PayerModel(model));
    }
}