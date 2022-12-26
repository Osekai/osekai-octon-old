namespace Osekai.Octon.Applications.OsuApi;

public class OsuApiTimeThrottler: TimeThrottlerPerSecond
{
    public OsuApiTimeThrottler() : base(20)
    {
    }
}