namespace Osekai.Octon.Services.Entities;

public class MedalSettings
{
    protected internal MedalSettings(bool locked)
    {
        Locked = locked;
    }
    public bool Locked { get; }
}