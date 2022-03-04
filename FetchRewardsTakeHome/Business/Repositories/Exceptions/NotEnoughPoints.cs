namespace FetchRewardsTakeHome.Business.Repositories.Exceptions;

public class NotEnoughPoints : Exception
{
    public NotEnoughPoints(int points) : base($"There are not enough points ({points}) to spend")
    {
    }
}