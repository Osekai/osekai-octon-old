using System.Buffers;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.ObjectPool;

namespace Osekai.Octon.Providers;

public class RandomBytes128BitTokenProvider: ITokenProvider
{
    private ObjectPool<StringBuilder> _stringBuilderObjectPool;
    
    public RandomBytes128BitTokenProvider(ObjectPool<StringBuilder> stringBuilderObjectPool)
    {
        _stringBuilderObjectPool = stringBuilderObjectPool;
    }
    
    public string GenerateToken()
    {
        byte[] bytes = ArrayPool<byte>.Shared.Rent(32)[0..32];
        RandomNumberGenerator.Fill(bytes);

        StringBuilder stringBuilder = _stringBuilderObjectPool.Get();

        for (int i = 0; i < bytes.Length; ++i)
            stringBuilder.Append($"{bytes[i]:x2}");

        return stringBuilder.ToString();
    }
}