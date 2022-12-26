namespace Osekai.Octon.Applications.OsuApiV2;

public class OsuApiTimeThrottler: TimeThrottlerPerSecond
{
    public OsuApiTimeThrottler() : base(20)
    {
    }
}