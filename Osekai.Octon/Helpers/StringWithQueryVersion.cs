using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Osekai.Octon.Helpers;

public class StringWithVersionQueryHtmlContent: IHtmlContent
{
    private string _input;

    private const string VersionQuery = "?v=" + Constants.Version;

    public StringWithVersionQueryHtmlContent(string input)
    {
        _input = input;
    }

    public static StringWithVersionQueryHtmlContent Create(string input) =>
        new StringWithVersionQueryHtmlContent(input);

    public void WriteTo(TextWriter writer, HtmlEncoder encoder)
    {
        encoder.Encode(writer, _input);
        encoder.Encode(writer, VersionQuery);
    }
}