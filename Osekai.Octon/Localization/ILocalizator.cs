namespace Osekai.Octon.Localization;

public interface ILocalizator
{
    public ValueTask<string> GetStringAsync(string identifier, object[]? variables = null, CancellationToken cancellationToken = default);
    public ValueTask<string> LocalizeStringAsync(string value, object[]?[]? variables = null, CancellationToken cancellationToken = default);
}