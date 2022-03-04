using FetchRewardsTakeHome.Business.Repositories.Models;
using FetchRewardsTakeHome.Business.Repositories.Payer;

namespace FetchRewardsTakeHome.Business.Services;

public class PayerService: IPayerService
{
    private readonly IPayerRepository _payerRepository;
    
    public PayerService(IPayerRepository payerRepository)
    {
        _payerRepository = payerRepository;
    }

    public async Task<List<PayerModel>> GetPayerBalances()
    {
        return await _payerRepository.GetAll();
    }
}