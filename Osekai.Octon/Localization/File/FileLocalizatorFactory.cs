using System.Text;
using Microsoft.Extensions.ObjectPool;

namespace Osekai.Octon.Localization.File;

public class FileLocalizatorFactory: ILocalizatorFactory
{
    protected string BasePath { get; }
    
    protected ObjectPool<StringBuilder> StringBuilderObjectPool { get; }

    public FileLocalizatorFactory(ObjectPool<StringBuilder> stringBuilderObjectPool, string basePath)
    {
        StringBuilderObjectPool = stringBuilderObjectPool;
        BasePath = basePath;
    }
    
    public ILocalizator CreateLocalizatorFromLanguageCode(string code)
    {
        if (!Directory.Exists(Path.Combine(BasePath, code)))
            code = "en_GB"; 
        
        return new FileLocalizator(StringBuilderObjectPool, BasePath, code);
    }
}