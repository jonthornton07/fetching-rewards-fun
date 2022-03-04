using FetchRewardsTakeHome.API.Controllers.Requests;
using FetchRewardsTakeHome.API.Controllers.Responses;
using FetchRewardsTakeHome.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace FetchRewardsTakeHome.API.Controllers;

[ApiController]
[Route("payers")]
public class PayerController
{
    private readonly IPayerService _payerService;
    
    public PayerController(IPayerService payerService)
    {
        _payerService = payerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var payers = await _payerService.GetPayerBalances();
        var responseList = payers.Select(p => new PayerResponse(p)).ToList();
        return new OkObjectResult(responseList);
    }
}