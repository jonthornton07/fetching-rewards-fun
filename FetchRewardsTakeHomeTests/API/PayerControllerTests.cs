using System.Collections.Generic;
using FakeItEasy;
using FetchRewardsTakeHome.API.Controllers;
using FetchRewardsTakeHome.API.Controllers.Responses;
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
        var mockService = A.Fake<IPayerService>();
        A.CallTo(() => mockService.GetPayerBalances()).Returns(new List<PayerModel>
        {
            new PayerModel("test"),
            new PayerModel("test2")
        });
        var controller = new PayerController(mockService);
        var result = await controller.GetAll();
        Assert.IsType<OkObjectResult>(result);

        var okResult = result as OkObjectResult;
        var value = okResult?.Value as List<PayerResponse>;
        Assert.Equal(2, value!.Count);
        Assert.Equal(200, okResult!.StatusCode);
    }
}