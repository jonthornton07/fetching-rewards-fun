using System.Collections.Generic;
using FakeItEasy;
using FetchRewardsTakeHome.Business.Repositories.Models;
using FetchRewardsTakeHome.Business.Repositories.Payer;
using FetchRewardsTakeHome.Business.Services;
using Xunit;

namespace FetchRewardsTakeHomeTests.Business.Repositories.Services;

public class PayerServiceTests
{

    [Fact]
    public async void TestGetAllPayerBalances()
    {
        var mockPayerRepo = A.Fake<IPayerRepository>();
        var payer1 = new PayerModel("test1");
        var payer2 = new PayerModel("test2");
        var returnList = new List<PayerModel>();
        returnList.Add(payer1);
        returnList.Add(payer2);
        A.CallTo(() => mockPayerRepo.GetAll()).Returns(returnList);

        var service = new PayerService(mockPayerRepo);
        var resultList = await service.GetPayerBalances();
        Assert.Equal(returnList.Count, resultList.Count);
    }
}