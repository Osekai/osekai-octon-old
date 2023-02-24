namespace Osekai.Octon.Domain.ValueObjects;

public readonly struct MedalSettings
{
    public MedalSettings(bool locked)
    {
        Locked = locked;
    }
    
    public bool Locked { get; }
}