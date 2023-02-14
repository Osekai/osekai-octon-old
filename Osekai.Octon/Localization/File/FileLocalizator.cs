using System.Text;
using System.Text.Json;
using Microsoft.Extensions.ObjectPool;
using Nito.AsyncEx;

namespace Osekai.Octon.Localization.File;

public class FileLocalizator: AbstractLocalizator
{
    protected string LanguageCode { get; }
    protected string BasePath { get; }
    protected string FilePath { get; }

    public FileLocalizator(ObjectPool<StringBuilder> stringBuilderObjectPool, string basePath, string languageCode) : base(stringBuilderObjectPool)
    {
        BasePath = basePath;
        LanguageCode = languageCode;
        FilePath = Path.Combine(BasePath, LanguageCode);
        SourceCache = new Dictionary<string, IReadOnlyDictionary<string, string>>();
        
        _lock = new AsyncReaderWriterLock();
    }

    private readonly AsyncReaderWriterLock _lock;
    
    private async Task<IReadOnlyDictionary<string, string>?> LoadSource(string source, CancellationToken cancellationToken = default)
    {
        
        string path = Path.Combine(FilePath, source + ".json");
        if (!System.IO.File.Exists(path))
            return null;

        await using FileStream fileStream = System.IO.File.OpenRead(path);

        IReadOnlyDictionary<string, string>? dictionary = await JsonSerializer.DeserializeAsync<IReadOnlyDictionary<string, string>>(fileStream, JsonSerializerOptions.Default, cancellationToken);
        if (dictionary == null)
            throw new InvalidDataException("Localization dictionary is null");

        return dictionary;
    }

    protected Dictionary<string, IReadOnlyDictionary<string, string>> SourceCache { get; }

    private async ValueTask<string> GetStringFromSourceAsync(
        string source, string key, object[] variables, CancellationToken cancellationToken = default)
    {
        IReadOnlyDictionary<string, string>? keys;
        IDisposable lockHandle = await _lock.ReaderLockAsync(cancellationToken);

        try
        {
            if (!SourceCache.TryGetValue(source, out keys))
            {
                AwaitableDisposable<IDisposable> newLockTask = _lock.WriterLockAsync(cancellationToken);
                lockHandle.Dispose();
                lockHandle = await newLockTask;

                keys = await LoadSource(source, cancellationToken);

                if (keys != null)
                    SourceCache.Add(source, keys);
                else
                    return CreateDefaultString(source, key);
            }
        }
        finally
        {
            lockHandle.Dispose();
        }

        if (!keys.TryGetValue(key, out string? value))
            return CreateDefaultString(source, key);

        return FormatStringAsync(value, variables, cancellationToken);
    }
    
    public override ValueTask<string> GetStringAsync(string identifier, object[]? variables = null, CancellationToken cancellationToken = default)
    {
        int firstDotIndex = identifier.IndexOf('.');
        if (firstDotIndex == -1)
            throw new ArgumentException("Invalid identifier (must at least contain one '.')");

        string source = identifier[..firstDotIndex];
        string key = identifier[(firstDotIndex + 1)..];

        return GetStringFromSourceAsync(source, key, variables ?? Array.Empty<object>(), cancellationToken);
    }
}