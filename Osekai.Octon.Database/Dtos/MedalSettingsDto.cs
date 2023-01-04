namespace Osekai.Octon.Database.Dtos;

public class MedalSettingsDto
{
    public MedalSettingsDto(bool locked)
    {
        Locked = locked;
    }
    public bool Locked { get; }
}