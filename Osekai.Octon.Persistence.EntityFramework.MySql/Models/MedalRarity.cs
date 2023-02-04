namespace Osekai.Octon.Persistence.EntityFramework.MySql.Models;

internal sealed class MedalRarity
{
    public int Id { get; set; }
    public int MedalId { get; set; }
    
    public Medal? Medal { get; set; } 
    
    public float Frequency { get; set; }
    public int Count { get; set; }
}