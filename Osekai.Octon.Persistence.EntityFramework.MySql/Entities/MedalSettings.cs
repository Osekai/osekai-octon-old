namespace Osekai.Octon.Persistence.EntityFramework.MySql.Entities;

internal sealed class MedalSettings
{
    public int Id { get; set; }
    public int MedalId { get; set; }
    public Medal Medal { get; set; } = null!;
    public bool Locked { get; set; }

    public Domain.Entities.MedalSettings ToEntity()
    {
        return new Domain.Entities.MedalSettings(Locked);
    }
}