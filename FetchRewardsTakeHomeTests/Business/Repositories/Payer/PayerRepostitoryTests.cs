using System;
using System.Threading.Tasks;
using FetchRewardsTakeHome.Business.Repositories.Exceptions;
using FetchRewardsTakeHome.Business.Repositories.Models;
using FetchRewardsTakeHome.Business.Repositories.Payer;
using FetchRewardsTakeHome.Data;
using FetchRewardsTakeHome.Data.Models;
using Xunit;

namespace FetchRewardsTakeHomeTests.Business.Repositories.Payer;

public class PayerRepositoryTests
{
    
    [Fact]
    public async void TestFindPayerByNameThrowsExceptionWhenDoesntExist()
    {
        var repo = new PayerRepository(new FakeDatabase());
        Func<Task> action = async () => { await repo.FindPayerByName("test"); };
        await Assert.ThrowsAsync<PayerNotFound>(action);
    }

    [Fact]
    public async void TestFindPayerByNameGetsRightPlayer()
    {
        var db = new FakeDatabase();
        var test2Id = Guid.NewGuid();
        db.Payers.Add(new PayerDataModel
        {
            AvailablePoints = 50,
            Created = DateTime.Now,
            Id = Guid.NewGuid(),
            LastUpdated = DateTime.Now,
            Payer = "Test"
        });
        db.Payers.Add(new PayerDataModel()
        {
            AvailablePoints = 50,
            Created = DateTime.Now,
            Id = test2Id,
            LastUpdated = DateTime.Now,
            Payer = "Test2"
        });
        var repo = new PayerRepository(db);
        var result = await repo.FindPayerByName("Test2");
        Assert.Equal(test2Id, result.Id);
    }
    
    [Fact]
    public async void TestInsertPayerAndGettingAll()
    {
        var db = new FakeDatabase();
        var repo = new PayerRepository(db);
        await repo.InsertNewPayer(new PayerModel("test1"));
        await repo.InsertNewPayer(new PayerModel("test2"));
        var result = await repo.GetAll();
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async void TestAddPointsWhenPayerDoesNotExistThrowsException()
    {
        var db = new FakeDatabase();
        var repo = new PayerRepository(db);
        await repo.InsertNewPayer(new PayerModel("test1"));
        Func<Task> action = async () => { await repo.AddPoints(new PayerModel("test"), 500); };
        await Assert.ThrowsAsync<PayerNotFound>(action);
    }

    [Fact]
    public async void TestAddPointsWhenPayerDoesExist()
    {
        var db = new FakeDatabase();
        var repo = new PayerRepository(db);
        var payer = await repo.InsertNewPayer(new PayerModel("test1"));
        var result = await repo.AddPoints(payer, 500);
        Assert.Equal(500, result.Points);
        result = await repo.AddPoints(result, 250);
        Assert.Equal(750, result.Points);
    }
}