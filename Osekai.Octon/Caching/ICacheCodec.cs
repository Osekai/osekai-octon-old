namespace Osekai.Octon.Caching;

public interface ICacheCodec
{
    void EncodeObject(Type type, object obj, Stream outputStream);
    object DecodeObject(Type type, Stream outputStream);
    object DecodeObject(Type type, ReadOnlyMemory<byte> memory);
}