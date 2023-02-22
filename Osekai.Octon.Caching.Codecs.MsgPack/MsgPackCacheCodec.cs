using System.Text.Json;
using MessagePack;

namespace Osekai.Octon.Caching.Codecs.MsgPack;

public class MsgPackCacheCodec: ICacheCodec
{
    public void EncodeObject(Type type, object obj, Stream outputStream)
    {
        MessagePackSerializer.Serialize(type, outputStream, obj,
            MessagePack.Resolvers.ContractlessStandardResolver.Options);
    }

    public object DecodeObject(Type type, Stream outputStream)
    {
        return MessagePackSerializer.Deserialize(type, outputStream, MessagePack.Resolvers.ContractlessStandardResolver.Options);
    }

    public object DecodeObject(Type type, ReadOnlyMemory<byte> memory)
    {
        return MessagePackSerializer.Deserialize(type, memory, MessagePack.Resolvers.ContractlessStandardResolver.Options);
    }
}