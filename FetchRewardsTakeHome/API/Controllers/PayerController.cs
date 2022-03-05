using System.Net.Mime;
using FetchRewardsTakeHome.API.Controllers.Requests;
using FetchRewardsTakeHome.API.Controllers.Responses;
using FetchRewardsTakeHome.Business.Repositories.Exceptions;
using FetchRewardsTakeHome.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace FetchRewardsTakeHome.API.Controllers;

[ApiController]
[Route("payers")]
public class PayerController
{
    private readonly IPayerService _payerService;
    private readonly IPointsService _pointsService;

    public PayerController(IPayerService payerService, IPointsService pointsService)
    {
        _payerService = payerService;
        _pointsService = pointsService;
    }

    [HttpGet("points")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PayerResponse>))]
    public async Task<IActionResult> GetAll()
    {
        var payers = await _payerService.GetPayerBalances();
        var responseList = payers.Select(p => new PayerResponse(p)).ToList();
        return new OkObjectResult(responseList);
    }


    [HttpGet("points/{guid}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PointsResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<StandardBadResponse>))]
    public async Task<IActionResult> GetPointsInformation([FromRoute] Guid guid)
    {
        var transaction = await _pointsService.LookupPointsInformation(guid);
        if (transaction == null)
        {
            return new BadRequestObjectResult(new StandardBadResponse("Unable to find points information"));
        }

        return new OkObjectResult(new PointsResponse(transaction));
    }

    [HttpPost("points/add")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PointsResponse))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<StandardBadResponse>))]
    public async Task<IActionResult> AddPoints([FromBody] AddPointsRequest request)
    {
        var transaction = await _pointsService.AddPoints(request.ToBusinessModel());
        if (transaction == null)
        {
            return new BadRequestObjectResult(new StandardBadResponse("Unable to insert request"));
        }
        return new OkObjectResult(new PointsResponse(transaction))
        {
            StatusCode = StatusCodes.Status201Created
        };
    }

    [HttpPost("points/spend")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(List<SpendResponse>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<StandardBadResponse>))]
    public async Task<IActionResult> Spend([FromBody] SpendRequest request)
    {
        try
        {
            var transactions = await _pointsService.Spend(request.Points);
            var responseList = transactions.Select(s => new SpendResponse(s)).ToList();
            return new OkObjectResult(responseList)
            {
                StatusCode = StatusCodes.Status201Created
            };
        }
        catch (NotEnoughPoints error)
        {
            return new BadRequestObjectResult(new StandardBadResponse(error));
        }
    }
}