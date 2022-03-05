using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using FetchRewardsTakeHome.API.Controllers;
using FetchRewardsTakeHome.API.Controllers.Requests;
using FetchRewardsTakeHome.API.Controllers.Responses;
using FetchRewardsTakeHome.Business.Repositories.Exceptions;
using FetchRewardsTakeHome.Business.Repositories.Models;
using FetchRewardsTakeHome.Business.Services;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace FetchRewardsTakeHomeTests.API;

public class PayerControllerTests 
{
    [Fact]
    public async void TestPayerControllerGetAll()
    {
        var mockPayerService = A.Fake<IPayerService>();
        var mockPointsService = A.Fake<IPointsService>();
        A.CallTo(() => mockPayerService.GetPayerBalances()).Returns(new List<PayerModel>
        {
            new PayerModel("test"),
            new PayerModel("test2")
        });
        var controller = new PayerController(mockPayerService, mockPointsService);
        var result = await controller.GetAll();
        Assert.IsType<OkObjectResult>(result);

        var okResult = result as OkObjectResult;
        var value = okResult?.Value as List<PayerResponse>;
        Assert.Equal(2, value!.Count);
        Assert.Equal(200, okResult!.StatusCode);
    }
    [Fact]
    public async void TestGetPointsInformation()
    {
        var mockService = A.Fake<IPointsService>();
        var mockPayerService = A.Fake<IPayerService>();
        var guid = Guid.NewGuid();
        A.CallTo(() => mockService.LookupPointsInformation(guid)).Returns(new PointsModel("test", 500, DateTime.Now){Id = guid});

        var controller = new PayerController(mockPayerService, mockService);
        var result = await controller.GetPointsInformation(guid);
        Assert.IsType<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        var value = okResult?.Value as PointsResponse;
        Assert.Equal(guid, value!.Id);
        Assert.Equal(200, okResult!.StatusCode);
    }
    
    [Fact]
    public async void TestGetPointsInformationThatDoesntExist()
    {
        var mockService = A.Fake<IPointsService>();
        var mockPayerService = A.Fake<IPayerService>();
        var controller = new PayerController(mockPayerService, mockService);

        var guid = Guid.NewGuid();
        A.CallTo(() => mockService.LookupPointsInformation(guid)).Returns(Task.FromResult<PointsModel?>(null));

        var result = await controller.GetPointsInformation(guid);
        Assert.IsType<BadRequestObjectResult>(result);
        var badResult = result as BadRequestObjectResult;
        Assert.IsType<StandardBadResponse>(badResult?.Value);
        Assert.Equal(400, badResult!.StatusCode);
    }

    [Fact]
    public async void TestAddPoints()
    {
        var mockService = A.Fake<IPointsService>();
        var mockPayerService = A.Fake<IPayerService>();
        var controller = new PayerController(mockPayerService, mockService);
        var transactionRequest = new AddPointsRequest
        {
            Payer = "test",
            Points = 500,
            TimeStamp = DateTime.Now
        };
        A.CallTo(() => mockService.AddPoints(A<PointsModel>.Ignored))
            .Returns(new PointsModel("test", 500, DateTime.Now));

        var result = await controller.AddPoints(transactionRequest);
        Assert.IsType<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        var value = okResult?.Value as PointsResponse;
        Assert.Equal(500, value!.Points);
        Assert.Equal("test", value!.Payer);
        Assert.Equal(201, okResult!.StatusCode);
    }
    
    [Fact]
    public async void TestSpendWithEnoughPoints()
    {
        var mockService = A.Fake<IPointsService>();
        var mockPayerService = A.Fake<IPayerService>();
        A.CallTo(() => mockService.Spend(500)).Returns(new List<PointsModel>()
        {
            new PointsModel("test", -500, DateTime.Now)
        });
            
        var controller = new PayerController(mockPayerService, mockService);
        var result = await controller.Spend(new SpendRequest()
        {
            Points = 500
        });
        Assert.IsType<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        var value = okResult?.Value as List<SpendResponse>;
        Assert.Equal("test", value![0].Payer);
        Assert.Equal(-500, value![0].Points);
        Assert.Equal(201, okResult!.StatusCode);
    }
    
    [Fact]
    public async void TestSpendWithoutEnoughPoints()
    {
        var mockService = A.Fake<IPointsService>();
        var mockPayerService = A.Fake<IPayerService>();
        A.CallTo(() => mockService.Spend(500)).Throws(new NotEnoughPoints(500));
            
        var controller = new PayerController(mockPayerService, mockService);
        var result = await controller.Spend(new SpendRequest()
        {
            Points = 500
        });
        var badResult = result as BadRequestObjectResult;
        Assert.IsType<StandardBadResponse>(badResult?.Value);
        Assert.Equal(400, badResult!.StatusCode);
        var responseObject = badResult?.Value as StandardBadResponse;
        Assert.Equal(new NotEnoughPoints(500).Message, responseObject!.Message);
    }
}