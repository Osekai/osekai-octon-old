namespace Osekai.Octon.OsuApi;

public class OsuApiTimeThrottler: TimeThrottlerPerSecond
{
    public OsuApiTimeThrottler() : base(20)
    {
    }
}