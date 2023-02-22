namespace Osekai.Octon.Domain.Entities;

public class MedalSettings
{
    public MedalSettings(bool locked)
    {
        Locked = locked;
    }
    
    public bool Locked { get; set; }
}