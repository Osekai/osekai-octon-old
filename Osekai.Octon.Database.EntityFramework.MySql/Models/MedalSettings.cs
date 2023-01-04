using Osekai.Octon.Database.Dtos;

namespace Osekai.Octon.Database.EntityFramework.MySql.Models;

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