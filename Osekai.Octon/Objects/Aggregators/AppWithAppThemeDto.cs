namespace Osekai.Octon.Objects.Aggregators;

public interface IReadOnlyAppWithAppTheme
{
    public IReadOnlyApp App { get; }
    public IReadOnlyAppTheme? AppTheme { get; }
}