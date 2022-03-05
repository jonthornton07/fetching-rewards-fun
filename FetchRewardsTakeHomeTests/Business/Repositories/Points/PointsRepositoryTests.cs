using System;
using System.Threading.Tasks;
using FetchRewardsTakeHome.Business.Repositories.Exceptions;
using FetchRewardsTakeHome.Business.Repositories.Models;
using FetchRewardsTakeHome.Business.Repositories.Points;
using FetchRewardsTakeHome.Data;
using Xunit;

namespace FetchRewardsTakeHomeTests.Business.Repositories.Points;

public class PointsRepositoryTests
{

    [Fact]
    public async void TestInsertAndFindTransaction()
    {
        var repo = new PointsRepository(new FakeDatabase());
        var first = await repo.InsertTransaction(new PointsModel("test", 250, DateTime.Now));
        var second = await repo.InsertTransaction(new PointsModel("test", 300, DateTime.Now));

        var firstResult = await repo.FindTransaction(first.Id);
        var secondResult = await repo.FindTransaction(second.Id);
        
        Assert.Equal(first.Points, firstResult.Points);
        Assert.Equal(first.Id, firstResult.Id);
        Assert.Equal(second.Points, secondResult.Points);
        Assert.Equal(second.Id, secondResult.Id);
    }

    [Fact]
    public async void TestPointsTransactionDoesNotReturnsNull()
    {
        var repo = new PointsRepository(new FakeDatabase());
        Assert.Null(await repo.FindTransaction(Guid.NewGuid()));
    }
    
    [Fact]
    public async void TestGetSortedTransactionsByDateAndAmounts()
    {
        var repo = new PointsRepository(new FakeDatabase());
        var first = await repo.InsertTransaction(new PointsModel("DANNON", 1000, DateTime.Parse("2020-11-02T14:00:00Z")));
        var second = await repo.InsertTransaction(new PointsModel("UNILEVER", 200, DateTime.Parse("2020-10-31T11:00:00Z")));
        var third = await repo.InsertTransaction(new PointsModel("DANNON", -200, DateTime.Parse("2020-10-31T15:00:00Z")));
        var fourth = await repo.InsertTransaction(new PointsModel("MILLER COORS", 10000, DateTime.Parse("2020-11-01T14:00:00Z")));
        var fifth = await repo.InsertTransaction(new PointsModel("DANNON", 300, DateTime.Parse("2020-10-31T10:00:00Z")));

        var result = await repo.GetSortedTransactions();
        Assert.Equal(third.Id, result[0].Id);
        Assert.Equal(fifth.Id, result[1].Id);
        Assert.Equal(second.Id, result[2].Id);
        Assert.Equal(fourth.Id, result[3].Id);
        Assert.Equal(first.Id, result[4].Id);
    }
}