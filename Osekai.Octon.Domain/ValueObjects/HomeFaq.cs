namespace Osekai.Octon.Domain.ValueObjects;

public readonly struct HomeFaq
{
    public HomeFaq(int id, int appId, string title, string content, string localizationPrefix)
    {
        Id = id;
        AppId = appId;
    }

    public int Id { get; }
    public int AppId { get; }

    private readonly string _title = null!;
    private readonly string _content = null!;
    private readonly string _localizationPrefix = null!;
    
    public string Title
    {
        get => _title;
        init
        {
            if (value.Length is < Specifications.HomeFaqTitleMinLength or > Specifications.HomeFaqTitleMaxLength)
                throw new ArgumentOutOfRangeException(nameof(value), value.Length, $"Invalid {nameof(Title)} length");

            _title = value;
        }
    }

    public string Content
    {
        get => _content;
        init
        {
            if (value.Length is < Specifications.HomeFaqContentMinLength or > Specifications.HomeFaqContentMaxLength)
                throw new ArgumentOutOfRangeException(nameof(value), value.Length, $"Invalid {nameof(Content)} length");

            _content = value;
        }
    }

    public string LocalizationPrefix
    {
        get => _localizationPrefix;
        init
        {
            if (value.Length is < Specifications.HomeFaqLocalizationPrefixMinLength or > Specifications.HomeFaqContentMaxLength)
                throw new ArgumentOutOfRangeException(nameof(value), value.Length, $"Invalid {nameof(LocalizationPrefix)} length");

            _localizationPrefix = value;
        }
    }
}