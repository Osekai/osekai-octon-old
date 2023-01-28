using Osekai.Octon.Persistence.Dtos;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Models;

internal class MedalSettings
{
    public int Id { get; set; }
    public int MedalId { get; set; }
    public Medal Medal { get; set; } = null!;
    public bool Locked { get; set; }

    public MedalSettingsDto ToDto()
    {
        return new MedalSettingsDto(Locked);
    }
}