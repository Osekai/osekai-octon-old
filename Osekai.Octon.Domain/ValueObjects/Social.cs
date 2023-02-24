namespace Osekai.Octon.Domain.ValueObjects;

public readonly struct Social
{
    public Social(string name, string link, string icon)
    {
        Name = name;
        Link = link;
        Icon = icon;
    }
    
    private readonly string _name = null!;
    private readonly string _link = null!;
    private readonly string _icon = null!;

    public string Icon
    {
        get => _icon;
        init
        {
            if (value.Length is < Specifications.SocialIconMinLength or > Specifications.SocialIconMaxLength)
                throw new ArgumentOutOfRangeException(nameof(value), value.Length, $"Invalid {nameof(Icon)} length");

            _icon = value;
        }
    }
    
    public string Name
    {
        get => _name;
        init
        {
            if (value.Length is < Specifications.SocialNameMinLength or > Specifications.SocialNameMaxLength)
                throw new ArgumentOutOfRangeException(nameof(value), value.Length, $"Invalid {nameof(Name)} length");

            _name = value;
        }
    }

    public string Link
    {
        get => _link;
        init
        {
            if (value.Length is < Specifications.SocialLinkMinLength or > Specifications.SocialLinkMaxLength)
                throw new ArgumentOutOfRangeException(nameof(value), value.Length, $"Invalid {nameof(Link)} length");
            
            _link = value;
        }
    }
}