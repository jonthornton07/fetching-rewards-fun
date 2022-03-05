using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using FetchRewardsTakeHome.Business.Repositories.Exceptions;
using FetchRewardsTakeHome.Business.Repositories.Models;
using FetchRewardsTakeHome.Business.Repositories.Payer;
using FetchRewardsTakeHome.Business.Repositories.Points;
using FetchRewardsTakeHome.Business.Services;
using Microsoft.Extensions.Logging;
using Xunit;

namespace FetchRewardsTakeHomeTests.Business.Repositories.Services;

public class PointsServiceTests
{

    [Fact]
    public void TestAddTransactionWithExistingPayer()
    {
        var mockPayerRepo = A.Fake<IPayerRepository>();
        var mockPointsRepo = A.Fake<IPointsRepository>();
        var payerModel = new PayerModel("test");

        A.CallTo(() => mockPayerRepo.FindPayerByName("test")).Returns(Task.FromResult<PayerModel?>(null));
        A.CallTo(() => mockPayerRepo.InsertNewPayer(A<PayerModel>.Ignored)).Returns(payerModel);
        A.CallTo(() => mockPointsRepo.InsertTransaction(A<PointsModel>.Ignored)).Returns(new PointsModel("test", 500, DateTime.Now));
        A.CallTo(() => mockPayerRepo.AddPoints(payerModel, 500)).Returns(payerModel);

        var service = new PointsService(mockPointsRepo, mockPayerRepo);
        service.AddPoints(new PointsModel("test", 500, DateTime.Now));

        A.CallTo(() => mockPayerRepo.FindPayerByName("test")).MustHaveHappened();
        A.CallTo(() => mockPayerRepo.InsertNewPayer(A<PayerModel>.Ignored)).MustHaveHappened();
        A.CallTo(() => mockPointsRepo.InsertTransaction(A<PointsModel>.Ignored)).MustHaveHappened();
        A.CallTo(() => mockPayerRepo.AddPoints(payerModel, 500)).MustHaveHappened();
    }
    
    [Fact]
    public void TestAddTransactionWithNonExistingPayer()
    {
        var mockPayerRepo = A.Fake<IPayerRepository>();
        var mockPointsRepo = A.Fake<IPointsRepository>();
        var payerModel = new PayerModel("test");

        A.CallTo(() => mockPayerRepo.FindPayerByName("test")).Returns(payerModel);
        A.CallTo(() => mockPayerRepo.InsertNewPayer(A<PayerModel>.Ignored)).Returns(payerModel);
        A.CallTo(() => mockPointsRepo.InsertTransaction(A<PointsModel>.Ignored)).Returns(new PointsModel("test", 500, DateTime.Now));
        A.CallTo(() => mockPayerRepo.AddPoints(payerModel, 500)).Returns(payerModel);

        var service = new PointsService(mockPointsRepo, mockPayerRepo);
        service.AddPoints(new PointsModel("test", 500, DateTime.Now));

        A.CallTo(() => mockPayerRepo.FindPayerByName("test")).MustHaveHappened();
        A.CallTo(() => mockPayerRepo.InsertNewPayer(A<PayerModel>.Ignored)).MustNotHaveHappened();
        A.CallTo(() => mockPointsRepo.InsertTransaction(A<PointsModel>.Ignored)).MustHaveHappened();
        A.CallTo(() => mockPayerRepo.AddPoints(payerModel, 500)).MustHaveHappened();
    }

    [Fact] 
    public async void TestLookUpTransactionNotFoundThrowsException()
    {
        var mockPayerRepo = A.Fake<IPayerRepository>();
        var mockPointsRepo = A.Fake<IPointsRepository>();

        var id = Guid.NewGuid();
        A.CallTo(() => mockPointsRepo.FindTransaction(id)).Returns(Task.FromResult<PointsModel?>(null));
        
        var service = new PointsService(mockPointsRepo, mockPayerRepo);
        Assert.Null(await service.LookupPointsInformation(id));
    }
    
    [Fact] 
    public async void TestLookUpTransactionFindsTransaction()
    {
        var mockPayerRepo = A.Fake<IPayerRepository>();
        var mockPointsRepo = A.Fake<IPointsRepository>();

        var id = Guid.NewGuid();
        var returnModel = new PointsModel("test", 50, DateTime.Now)
        {
            Id = id
        };
        A.CallTo(() => mockPointsRepo.FindTransaction(id)).Returns(returnModel);
        
        var service = new PointsService(mockPointsRepo, mockPayerRepo);
        var returnObject = await service.LookupPointsInformation(id);
        Assert.Equal(id, returnObject.Id);
    }

    [Fact]
    public async void TestSpend()
    {
        var first = new PointsModel("DANNON", -200, DateTime.Parse("2020-10-31T15:00:00Z"));
        var second = new PointsModel("DANNON", 300, DateTime.Parse("2020-10-31T10:00:00Z"));
        var third = new PointsModel("UNILEVER", 200, DateTime.Parse("2020-10-31T11:00:00Z"));
        var fourth = new PointsModel("MILLER COORS", 10000, DateTime.Parse("2020-11-01T14:00:00Z"));
        var fifth =new PointsModel("DANNON", 1000, DateTime.Parse("2020-11-02T14:00:00Z"));
        var list = new List<PointsModel>();
        list.Add(first);
        list.Add(second);
        list.Add(third);
        list.Add(fourth);
        list.Add(fifth);
        
        var mockPayerRepo = A.Fake<IPayerRepository>();
        var mockPointsRepo = A.Fake<IPointsRepository>();

        A.CallTo(() => mockPointsRepo.GetSortedTransactions()).Returns(list);
        A.CallTo(() => mockPayerRepo.FindPayerByName(A<string>.Ignored)).Returns(new PayerModel("test"));
        A.CallTo(() => mockPointsRepo.InsertTransaction(A<PointsModel>.Ignored)).Returns(new PointsModel("test", 500, DateTime.Now));
        A.CallTo(() => mockPayerRepo.AddPoints(A<PayerModel>.Ignored, A<int>.Ignored)).Returns(new PayerModel("test"));
        
        var service = new PointsService(mockPointsRepo, mockPayerRepo);
        await service.Spend(5000);

        A.CallTo(() => mockPointsRepo.InsertTransaction(A<PointsModel>.Ignored))
            .WhenArgumentsMatch((PointsModel model) => model.Points == -100 && model.Payer == "DANNON")
            .MustHaveHappened();
        
        A.CallTo(() => mockPointsRepo.InsertTransaction(A<PointsModel>.Ignored))
            .WhenArgumentsMatch((PointsModel model) => model.Points == -200 && model.Payer == "UNILEVER")
            .MustHaveHappened();
        
        A.CallTo(() => mockPointsRepo.InsertTransaction(A<PointsModel>.Ignored))
            .WhenArgumentsMatch((PointsModel model) => model.Points == -4700 && model.Payer == "MILLER COORS")
            .MustHaveHappened();
        
        A.CallTo(() => mockPointsRepo.InsertTransaction(A<PointsModel>.Ignored))
            .MustHaveHappenedANumberOfTimesMatching(n => n == 3);
    }
    
    [Fact]
    public async void TestSpendWithoutEnoughPoints()
    {
        var first = new PointsModel("DANNON", -200, DateTime.Parse("2020-10-31T15:00:00Z"));
        var second = new PointsModel("DANNON", 300, DateTime.Parse("2020-10-31T10:00:00Z"));
        var list = new List<PointsModel>();
        list.Add(first);
        list.Add(second);
        
        var mockPayerRepo = A.Fake<IPayerRepository>();
        var mockPointsRepo = A.Fake<IPointsRepository>();

        A.CallTo(() => mockPointsRepo.GetSortedTransactions()).Returns(list);
        var service = new PointsService(mockPointsRepo, mockPayerRepo);
        
        Func<Task> action = async () => { await service.Spend(5000000);};
        await Assert.ThrowsAsync<NotEnoughPoints>(action);
    }
    
        [Fact]
    public async void TestSpendDoesNotReturnPayerWithNoTransactions()
    {
        
        var first = new PointsModel("UNILEVER", -500, DateTime.Parse("2020-10-29T15:00:00Z"));
        var second = new PointsModel("UNILEVER", 500, DateTime.Parse("2020-10-31T15:00:00Z"));
        var third = new PointsModel("DANNON", 300, DateTime.Parse("2020-10-30T10:00:00Z"));
        var list = new List<PointsModel>();
        list.Add(first);
        list.Add(second);
        list.Add(third);
        
        var mockPayerRepo = A.Fake<IPayerRepository>();
        var mockPointsRepo = A.Fake<IPointsRepository>();

        A.CallTo(() => mockPointsRepo.GetSortedTransactions()).Returns(list);
        A.CallTo(() => mockPayerRepo.FindPayerByName(A<string>.Ignored)).Returns(new PayerModel("test"));
        A.CallTo(() => mockPointsRepo.InsertTransaction(A<PointsModel>.Ignored)).Returns(new PointsModel("test", 300, DateTime.Now));
        A.CallTo(() => mockPayerRepo.AddPoints(A<PayerModel>.Ignored, A<int>.Ignored)).Returns(new PayerModel("test"));
        
        var service = new PointsService(mockPointsRepo, mockPayerRepo);
        await service.Spend(300);

        A.CallTo(() => mockPointsRepo.InsertTransaction(A<PointsModel>.Ignored))
            .WhenArgumentsMatch((PointsModel model) => model.Payer == "UNILEVER")
            .MustNotHaveHappened();
        
        A.CallTo(() => mockPointsRepo.InsertTransaction(A<PointsModel>.Ignored))
            .WhenArgumentsMatch((PointsModel model) => model.Points == -300 && model.Payer == "DANNON")
            .MustHaveHappened();
        
        A.CallTo(() => mockPointsRepo.InsertTransaction(A<PointsModel>.Ignored))
            .MustHaveHappenedANumberOfTimesMatching(n => n == 1);
    }
}