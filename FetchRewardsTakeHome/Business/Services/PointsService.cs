using FetchRewardsTakeHome.Business.Repositories.Exceptions;
using FetchRewardsTakeHome.Business.Repositories.Models;
using FetchRewardsTakeHome.Business.Repositories.Payer;
using FetchRewardsTakeHome.Business.Repositories.Points;

namespace FetchRewardsTakeHome.Business.Services;

public class PointsService : IPointsService
{
    private readonly IPayerRepository _payerRepository;
    private readonly IPointsRepository _pointsRepository;

    public PointsService(IPointsRepository pointsRepository, IPayerRepository payerRepository)
    {
        _pointsRepository = pointsRepository;
        _payerRepository = payerRepository;
    }

    public async Task<PointsModel?> AddPoints(PointsModel points)
    {
        var payer = await _payerRepository.FindPayerByName(points.Payer);
        if (payer == null)
        {
            payer = await _payerRepository.InsertNewPayer(new PayerModel(points.Payer));
        }
        var returnedTransaction = await _pointsRepository.InsertTransaction(points);
        await _payerRepository.AddPoints(payer, returnedTransaction.Points);
        return returnedTransaction;
    }

    public async Task<PointsModel?> LookupPointsInformation(Guid guid)
    {
        return await _pointsRepository.FindTransaction(guid);
    }

    public async Task<List<PointsModel>> Spend(int points)
    {
        var transactions = await _pointsRepository.GetSortedTransactions();
        var remainingPoints = Math.Abs(points);
        var payeePointsMap = new Dictionary<string, int>();

        for (int i = 0; i < transactions.Count; i++)
        {
            var transaction = transactions[i];
            var payer = transaction.Payer;
            if (!payeePointsMap.ContainsKey(transaction.Payer))
            {
                payeePointsMap[payer] = 0;
            }

            var pointsToAdd = transaction.Points;
            if (transaction.Points >= 0)
            {
                pointsToAdd = Math.Min(remainingPoints, transaction.Points);
            }
            payeePointsMap[payer] += pointsToAdd;
            remainingPoints -= pointsToAdd;
            if (remainingPoints == 0)
            {
                break;
            }
        }
        
        if (remainingPoints != 0)
        {
            throw new NotEnoughPoints(points);
        }

        var transactionToRedeem = new List<PointsModel>();
        foreach (var key in payeePointsMap.Keys)
        {
            if (payeePointsMap[key] != 0)
            {
                transactionToRedeem.Add(new PointsModel(key, payeePointsMap[key] * -1, DateTime.Now));
            }
        }

        return await AddTransactions(transactionToRedeem);
    }

    private async Task<List<PointsModel>> AddTransactions(List<PointsModel> transactions)
    {
        var returnList = new List<PointsModel>();
        foreach (var transaction in transactions)
        {
            returnList.Add(await AddPoints(transaction));
        }
        return returnList;
    }
}