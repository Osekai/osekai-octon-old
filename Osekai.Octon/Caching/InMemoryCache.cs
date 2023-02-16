namespace Osekai.Octon.Caching;

public class InMemoryCache: ICache
{
    private readonly struct Entry
    {
        public Entry(object? value, DateTimeOffset expiresAt)
        {
            ExpiresAt = expiresAt;
            Value = value;
        }
        
        public DateTimeOffset ExpiresAt { get; }
        public object? Value { get; }
    }

    private readonly ReaderWriterLockSlim _dictionaryRwLock;
    private readonly object _cleanTaskLock;
    
    private readonly Dictionary<string, Entry> _objects;
    private Task? _cleanTask;

    private readonly int _cleanTaskDelay;
    
    public InMemoryCache(int cleanTaskDelay = 1000 * 60)
    {
        _cleanTaskDelay = cleanTaskDelay;
        _dictionaryRwLock = new ReaderWriterLockSlim();
        _cleanTaskLock = new object();
        
        _objects = new Dictionary<string, Entry>();
    }

    private void Clean()
    {
        _dictionaryRwLock.EnterWriteLock();
        
        DateTimeOffset now =  DateTimeOffset.Now;
        List<string> expiredEntryKeys = new List<string>();

        try
        {
            foreach (var (key, entry) in _objects)
                if (now > entry.ExpiresAt) 
                    expiredEntryKeys.Add(key);

            foreach (string key in expiredEntryKeys)
                _objects.Remove(key);
            
            _objects.TrimExcess();
        }
        finally
        {
            _dictionaryRwLock.ExitWriteLock();
        }
    }
    
    private void StartCleanTask()
    {
        _dictionaryRwLock.EnterReadLock();
        try
        {
            if (_objects.Count == 0)
                return;
        }
        finally
        {
            _dictionaryRwLock.ExitReadLock();
        }

        lock (_cleanTaskLock)
        {
            if (_cleanTask != null)
                return;

            _cleanTask = Task.Run(async () =>
            {
                await Task.Delay(_cleanTaskDelay);
                Clean();

                lock (_cleanTaskLock)
                    _cleanTask = null;

                StartCleanTask();
            });
        }
    }

    public Task<T?> GetAsync<T>(string name, CancellationToken cancellationToken = default) where T : class
    {
        Entry entry;
        bool result;

        _dictionaryRwLock.EnterReadLock();
        try
        {
            result = _objects.TryGetValue(name, out entry);
        }
        finally
        {
            _dictionaryRwLock.ExitReadLock();
        }
        
        if (result)
        {
            if (DateTimeOffset.Now > entry.ExpiresAt)
                return Task.FromResult<T?>(null);

            if (entry.Value is not T value)
                throw new ArgumentException($"The entry \"{name}\" is not of type \"{typeof(T)}\"");

            return Task.FromResult<T?>(value);
        }

        return Task.FromResult<T?>(null);
    }

    public Task SetAsync<T>(string name, T data, long expiresAfter = 3600, CancellationToken cancellationToken = default) where T : class?
    {
        try
        {
            _dictionaryRwLock.EnterWriteLock();
            try
            {
                _objects[name] = new Entry(data, DateTimeOffset.Now.AddSeconds(expiresAfter));
            }
            finally
            {
                _dictionaryRwLock.ExitWriteLock();
            }
            
            return Task.CompletedTask;
        }
        finally
        {
            StartCleanTask();
        }
    }

    public Task DeleteAsync(string name, CancellationToken cancellationToken = default)
    {
        try
        {
            _dictionaryRwLock.EnterWriteLock();
            try
            {
                _objects.Remove(name, out _);
            }
            finally
            {
                _dictionaryRwLock.ExitWriteLock();
            }
            
            return Task.CompletedTask;
        }
        finally
        {
            StartCleanTask();    
        }
    }
}