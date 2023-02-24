namespace Osekai.Octon.Persistence.EntityFramework.MySql.Entities;

internal sealed class MedalSettings
{
    public int Id { get; set; }
    public int MedalId { get; set; }
    public Medal Medal { get; set; } = null!;
    public bool Locked { get; set; }

    public Domain.ValueObjects.MedalSettings ToValueObject()
    {
        return new Domain.ValueObjects.MedalSettings(Locked);
    }
}