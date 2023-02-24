using System.Text.Json.Serialization;

namespace Osekai.Octon;

public readonly struct Ref<T>
{
    public Ref(T value)
    {
        Value = value;
    }
    
    public static implicit operator Ref<T>(T v) => new Ref<T>(v);
    public static implicit operator T(Ref<T> v) => v.Value;
    
    public T Value { get; }
}