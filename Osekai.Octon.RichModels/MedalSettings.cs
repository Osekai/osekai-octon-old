namespace Osekai.Octon.RichModels;

public class MedalSettings
{
    protected internal MedalSettings(bool locked)
    {
        Locked = locked;
    }
    public bool Locked { get; }
}