using System.Collections;

namespace Osekai.Octon.DataStructures;


public partial class ValueTrie<T>: IEnumerable<KeyValuePair<string, T>> where T: struct
{
    public T? Value { get; private set; }
    
    public ValueTrie.Wildcard<T>? Wildcard { get; private set; }

    public ValueTrie(T? value = null)
    {
        Value = value;
        _children = new SortedList<string, ValueTrie<T>>();
    }
    
    private readonly SortedList<string, ValueTrie<T>> _children;

    public ValueTrie<T>? GetNodeRecursive(string path)
    {
        if (!TriePathValidatin.IsValidPath(path))
            throw new ArgumentException("Invalid path");

        return InternalGetNodeRecursive(path);
    }

    public ValueTrie<T>? InternalGetNodeRecursive(string path)
    {
        if (path == string.Empty)
            return this;

        int subPathStartIndex = path.IndexOf('.');

        if (subPathStartIndex == -1)
            return _children.GetValueOrDefault(path);

        string head = path[..subPathStartIndex];
        string tail = path[(subPathStartIndex + 1)..];

        if (tail == "*")
            return Wildcard != null ? this : null;
        
        ValueTrie<T>? node = _children.GetValueOrDefault(head)?.InternalGetNodeRecursive(tail);

        if (node != null)
            return node;

        return Wildcard != null ? this : null;
    }

    public void AddValueRecursive(string path, T value)
    {
        if (!TriePathValidatin.IsValidPath(path))
            throw new ArgumentException("Invalid path");

        InternalAddValueRecursive(path, value);
    }

    private void InternalAddValueRecursive(string path, T value)
    {
        switch (path)
        {
            case "":
                Value = value;
                break;
            case "*":
                _children.Clear();
                _children.TrimExcess();
                Wildcard = new ValueTrie.Wildcard<T>(value);
                break;
            default:
                int subPathStartIndex = path.IndexOf('.');

                string head, tail;

                if (subPathStartIndex == -1)
                {
                    head = path[..path.Length];
                    tail = string.Empty;
                }
                else
                {
                    head = path[..subPathStartIndex];
                    tail = path[(subPathStartIndex + 1)..];
                }

                ValueTrie<T>? node = _children.GetValueOrDefault(head);

                if (node == null)
                {
                    node = new ValueTrie<T>();
                    _children.Add(head, node);
                }

                if (Wildcard != null && EqualityComparer<T>.Default.Equals(Wildcard.Value.Value, value))
                    return;    

                node.InternalAddValueRecursive(tail, value);
                break;
        }

    }
    
    
    public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
    {
        return new ValueTrieEnumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return new ValueTrieEnumerator(this);
    }
    
    private class ValueTrieEnumerator: IEnumerator<KeyValuePair<string, T>>
    {
        private readonly Stack<KeyValuePair<string, ValueTrie<T>>> _tries;
        private readonly ValueTrie<T> _root;

        private KeyValuePair<string, ValueTrie<T>>? _lastValueTrie;
            
        public ValueTrieEnumerator(ValueTrie<T> root)
        {
            _root = root;
            
            _tries = new Stack<KeyValuePair<string, ValueTrie<T>>>();
            _tries.Push(new KeyValuePair<string, ValueTrie<T>>(string.Empty, root));
            
            Current = default;
        }

        public bool MoveNext()
        {
            if (_lastValueTrie?.Value.Value != null)
            {
                Current = new KeyValuePair<string, T>(_lastValueTrie.Value.Key, _lastValueTrie.Value.Value.Value.Value);
                return true;
            }
            
            while (_tries.Count > 0)
            {
                var (path, trie) = _tries.Pop();

                foreach (var (pathChild, trieChild) in trie._children)
                {
                    string resultPath = (path != string.Empty ? path + "." : string.Empty) + pathChild;
                    _tries.Push(new KeyValuePair<string, ValueTrie<T>>(resultPath, trieChild));
                }

                if (trie.Wildcard != null)
                {
                    Current = new KeyValuePair<string, T>(path + ".*", trie.Wildcard.Value.Value);
                    _lastValueTrie = new KeyValuePair<string, ValueTrie<T>>(path, trie);
                    return true;
                }

                if (trie.Value != null)
                {
                    Current = new KeyValuePair<string, T>(path, trie.Value.Value);
                    return true;
                }
            }

            return false;
        }

        public void Reset()
        {
            _tries.Clear();
            _tries.Push(new KeyValuePair<string, ValueTrie<T>>(string.Empty, _root));
            
            Current = default;
        }

        public KeyValuePair<string, T> Current { get; private set; }

        object IEnumerator.Current => Current;

        public void Dispose() { }
    }
}