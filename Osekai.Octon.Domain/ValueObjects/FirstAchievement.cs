namespace Osekai.Octon.Domain.ValueObjects;

public readonly struct FirstAchievement
{
    private readonly string _firstAchievedBy = null!;
    
    public DateTimeOffset FirstAchievedDate { get; }

    public FirstAchievement(string firstAchievedBy, DateTimeOffset firstAchievedDate)
    {
        FirstAchievedBy = firstAchievedBy;
        FirstAchievedDate = firstAchievedDate;
    }
    
    public string FirstAchievedBy
    {
        get => _firstAchievedBy;
        init
        {
            if (value.Length is < Specifications.FirstAchievedByMinLength or > Specifications.FirstAchievedByMaxLength)
                throw new ArgumentOutOfRangeException(nameof(value), value.Length, $"Invalid {nameof(FirstAchievedBy)} length");

            _firstAchievedBy = value;
        }
    }
}