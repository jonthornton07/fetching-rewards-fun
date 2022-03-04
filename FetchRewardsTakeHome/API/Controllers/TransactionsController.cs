using System.Net.Mime;
using FetchRewardsTakeHome.API.Controllers.Requests;
using FetchRewardsTakeHome.API.Controllers.Responses;
using FetchRewardsTakeHome.Business.Repositories.Exceptions;
using FetchRewardsTakeHome.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace FetchRewardsTakeHome.API.Controllers;

[ApiController]
[Route("transactions")]
public class TransactionsController
{
    private readonly ITransactionService _transactionService;

    public TransactionsController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpGet("{guid}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> GetTransaction([FromRoute] Guid guid)
    {
        try
        {
            var transaction = await _transactionService.LookupTransaction(guid);
            return new OkObjectResult(new TransactionResponse(transaction));
        }
        catch (TransactionNotFound e)
        {
            return new BadRequestObjectResult(new StandardBadResponse(e));
        }
    }

    [HttpPost("add")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TransactionResponse))]
    public async Task<IActionResult> AddTransaction([FromBody] AddTransactionRequest request)
    {
            var transaction = await _transactionService.AddTransaction(request.ToBusinessModel());
            return new OkObjectResult(new TransactionResponse(transaction))
            {
                StatusCode = StatusCodes.Status201Created
            };
    }
    
    [HttpPost("spend")]
    public async Task<IActionResult> Spend([FromBody] SpendRequest request)
    {
        try
        {
            var transactions = await _transactionService.Spend(request.Points);
            var responseList = transactions.Select(s => new SpendResponse(s)).ToList();
            return new OkObjectResult(responseList);
        }
        catch (NotEnoughPoints error)
        {
            return new BadRequestObjectResult(new StandardBadResponse(error));
        }
    }
}