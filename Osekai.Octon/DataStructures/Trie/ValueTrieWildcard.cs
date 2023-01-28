namespace Osekai.Octon.DataStructures;

public partial class ValueTrie
{
    public readonly struct Wildcard<T>
    {
        public Wildcard(T permissionActionType)
        {
            Value = permissionActionType;
        }
        
        public T Value { get; }
    }
}