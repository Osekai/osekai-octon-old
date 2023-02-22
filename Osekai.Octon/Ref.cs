using System.Text.Json.Serialization;

namespace Osekai.Octon;

public class Ref<T>
{
    public Ref()
    {
        _included = false;
        _value = default!;
    }

    public Ref(T value)
    {
        _included = true;
        _value = value;
    }

    private T _value;
    private bool _included;
    
    public bool Included
    {
        get => _included;
        set
        {
            if (!value)
                _value = default!;

            _included = value;
        }
    }

    public T Value
    {
        get => _value;
        set
        {
            _included = true;
            _value = value;
        }
    }
}