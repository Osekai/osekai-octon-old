using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.ObjectPool;

namespace Osekai.Octon.Localization;

public abstract class AbstractLocalizator: ILocalizator
{
    protected ObjectPool<StringBuilder> StringBuilderObjectPool { get; }
    
    public AbstractLocalizator(ObjectPool<StringBuilder> stringBuilderObjectPool)
    {
        StringBuilderObjectPool = stringBuilderObjectPool;
    }

    private static readonly Regex FormatStringRegex = new Regex(@"(?:\$(?<Variable>\d+))", RegexOptions.Compiled);
    private static readonly Regex LocalizeStringRegex = new Regex(@"(?:\?\?(?<Substring>[^\?]+)\?\?)", RegexOptions.Compiled);
    
    protected string FormatStringAsync(string value, object[] variables, CancellationToken cancellationToken = default)
    {
        MatchCollection matches = FormatStringRegex.Matches(value);

        if (matches.Count == 0)
            return value;
        
        StringBuilder stringBuilder = StringBuilderObjectPool.Get();
        try
        {
            Match? lastMatch = null;
            
            foreach (Match match in matches)
            {
                stringBuilder.Append(value[..match.Index]);
                
                int variableIndex = int.Parse(match.Groups["Variable"].ValueSpan);
                stringBuilder.Append(variables[variableIndex - 1]);
                
                lastMatch = match;
            }

            stringBuilder.Append(lastMatch == null ? value : value[(lastMatch.Index + lastMatch.Length)..]);

            return stringBuilder.ToString();
        }
        finally
        {
            StringBuilderObjectPool.Return(stringBuilder);
        }
    }
    
    public async ValueTask<string> LocalizeStringAsync(string value, object[]?[]? variables = null, CancellationToken cancellationToken = default)
    {
        variables ??= Array.Empty<object[]>();
        
        MatchCollection matches = LocalizeStringRegex.Matches(value);

        if (matches.Count == 0)
            return value;
        
        StringBuilder stringBuilder = StringBuilderObjectPool.Get();
        try
        {
            Match? lastMatch = null;

            int index = 0;
            foreach (Match match in matches)
            {
                stringBuilder.Append(value[(lastMatch != null ? (lastMatch.Index + lastMatch.Length) : 0)..match.Index]);
                
                string substring = match.Groups["Substring"].Value;

                object[]? parameters = variables.Length <= index ? null : variables[index];
                stringBuilder.Append(await GetStringAsync(substring, parameters, cancellationToken));
                
                lastMatch = match;
                index++;
            }

            stringBuilder.Append(lastMatch == null ? value : value[(lastMatch.Index + lastMatch.Length)..]);

            return stringBuilder.ToString();
        }
        finally
        {
            StringBuilderObjectPool.Return(stringBuilder);
        }
    }
    
    protected string CreateDefaultString(string source, string key)
    {
        StringBuilder stringBuilder = StringBuilderObjectPool.Get();
        
        try
        {
            stringBuilder.Append("__");
            stringBuilder.Append(source);
            stringBuilder.Append(".");
            stringBuilder.Append(key);
            stringBuilder.Append("__");

            return stringBuilder.ToString();
        }
        finally
        {
            StringBuilderObjectPool.Return(stringBuilder);    
        }
    }
    
    public abstract ValueTask<string> GetStringAsync(string identifier, object[]? variables = null, CancellationToken cancellationToken = default);
    
}