using System;
using System.Collections.Generic;
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

public class PointsControllerTests
{
    [Fact]
    public async void TestGetTransactionSuccess()
    {
        var mockService = A.Fake<ITransactionService>();
        var guid = Guid.NewGuid();
        A.CallTo(() => mockService.LookupTransaction(guid)).Returns(new PointsModel("test", 500, DateTime.Now){Id = guid});

        var controller = new TransactionsController(mockService);
        var result = await controller.GetTransaction(guid);
        Assert.IsType<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        var value = okResult?.Value as TransactionResponse;
        Assert.Equal(guid, value!.Id);
        Assert.Equal(200, okResult!.StatusCode);
    }
    
    [Fact]
    public async void TestGetTransactionDoesNotExist()
    {
        var mockService = A.Fake<ITransactionService>();
        var controller = new TransactionsController(mockService);

        var guid = Guid.NewGuid();
        A.CallTo(() => mockService.LookupTransaction(guid)).Throws(new TransactionNotFound(guid));

        var result = await controller.GetTransaction(guid);
        Assert.IsType<BadRequestObjectResult>(result);
        var badResult = result as BadRequestObjectResult;
        Assert.IsType<StandardBadResponse>(badResult?.Value);
        Assert.Equal(400, badResult!.StatusCode);
        var responseObject = badResult?.Value as StandardBadResponse;
        Assert.Equal(new TransactionNotFound(guid).Message, responseObject!.Message);
    }

    [Fact]
    public async void TestAddTransaction()
    {
        var mockService = A.Fake<ITransactionService>();
        var controller = new TransactionsController(mockService);
        var transactionRequest = new AddTransactionRequest
        {
            Payer = "test",
            Points = 500,
            TimeStamp = DateTime.Now
        };
        A.CallTo(() => mockService.AddTransaction(A<PointsModel>.Ignored))
            .Returns(new PointsModel("test", 500, DateTime.Now));

        var result = await controller.AddTransaction(transactionRequest);
        Assert.IsType<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        var value = okResult?.Value as TransactionResponse;
        Assert.Equal(500, value!.Points);
        Assert.Equal("test", value!.Payer);
        Assert.Equal(201, okResult!.StatusCode);
    }
    
    [Fact]
    public async void TestSpendWithEnoughPoints()
    {
        var mockService = A.Fake<ITransactionService>();
        A.CallTo(() => mockService.Spend(500)).Returns(new List<PointsModel>()
        {
            new PointsModel("test", -500, DateTime.Now)
        });
            
        var controller = new TransactionsController(mockService);
        var result = await controller.Spend(new SpendRequest()
        {
            Points = 500
        });
        Assert.IsType<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        var value = okResult?.Value as List<SpendResponse>;
        Assert.Equal("test", value![0].Payer);
        Assert.Equal(-500, value![0].Points);
        Assert.Equal(200, okResult!.StatusCode);
    }
    
    [Fact]
    public async void TestSpendWithoutEnoughPoints()
    {
        var mockService = A.Fake<ITransactionService>();
        A.CallTo(() => mockService.Spend(500)).Throws(new NotEnoughPoints(500));
            
        var controller = new TransactionsController(mockService);
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