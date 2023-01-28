namespace Osekai.Octon.Persistence.Dtos;

public class MedalSettingsDto
{
    public MedalSettingsDto(bool locked)
    {
        Locked = locked;
    }
    public bool Locked { get; }
}