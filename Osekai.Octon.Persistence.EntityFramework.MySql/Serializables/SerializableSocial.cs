using System.Text.Json.Serialization;

namespace Osekai.Octon.Persistence.EntityFramework.MySql.Serializables;

public readonly struct SerializableSocial
{
    [JsonConstructor]
    public SerializableSocial(string name, string link, string icon)
    {
        Name = name;
        Link = link;
        Icon = icon;
    }

    public string Name { get; }
    public string Link { get; }
    public string Icon { get; }
}