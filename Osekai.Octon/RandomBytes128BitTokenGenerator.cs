using System.Buffers;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.ObjectPool;

namespace Osekai.Octon;

public class RandomBytes128BitTokenGenerator: ITokenGenerator
{
    private readonly ObjectPool<StringBuilder> _stringBuilderObjectPool;
    
    public RandomBytes128BitTokenGenerator(ObjectPool<StringBuilder> stringBuilderObjectPool)
    {
        _stringBuilderObjectPool = stringBuilderObjectPool;
    }
    
    public string GenerateToken()
    {
        byte[] bytes = ArrayPool<byte>.Shared.Rent(16)[0..16];
        RandomNumberGenerator.Fill(bytes);

        StringBuilder stringBuilder = _stringBuilderObjectPool.Get();

        try
        {
            for (int i = 0; i < bytes.Length; ++i)
                stringBuilder.Append($"{bytes[i]:x2}");

            return stringBuilder.ToString();
        }
        finally
        {
            _stringBuilderObjectPool.Return(stringBuilder);
        }
    }
}