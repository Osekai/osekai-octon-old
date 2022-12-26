namespace Osekai.Octon;

public class TimeThrottlerPerSecond
{
    public int MaxRequestsPerSecond { get; }

    public TimeThrottlerPerSecond(int maxRequestsPerSecond)
    {
        MaxRequestsPerSecond = maxRequestsPerSecond;

        _queue = new LinkedList<TaskCompletionSource>();
        _lock = new object();
    
        UpdateCheckpoints();
    }

    private void UpdateCheckpoints()
    {
        DateTime now = DateTime.Now;
        _currentSecondCheckpoint = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
        _nextSecondCheckpoint = _currentSecondCheckpoint.AddSeconds(1);
    }

    private int _executedRequestsInCurrentSecond;
    private readonly LinkedList<TaskCompletionSource> _queue;
    private readonly object _lock;

    private Task? _timerTask;

    private DateTime _currentSecondCheckpoint;
    private DateTime _nextSecondCheckpoint;

    private void RemoveTaskCompletionSource(TaskCompletionSource taskCompletionSource)
    {
        lock (_lock)
            _queue.Remove(taskCompletionSource);
    }

    private async Task GetTimerTask()
    {
        await Task.Delay((int)(DateTime.Now - _currentSecondCheckpoint).TotalMilliseconds);
    
        lock (_lock)
        {
            while (DateTime.Now <= _nextSecondCheckpoint)
            {
                // Spin wait
            }
            
            UpdateCheckpoints();
            
            _executedRequestsInCurrentSecond = 0;

            while (_queue.Count > 0 && ++_executedRequestsInCurrentSecond <= MaxRequestsPerSecond)
            {
                TaskCompletionSource taskCompletionSource = _queue.First!.Value;
                
                _queue.RemoveFirst();
               
                if (!taskCompletionSource.Task.IsCanceled)
                    taskCompletionSource.SetResult();
            }

            if (_queue.Count > 0)
                _timerTask = GetTimerTask();
        }
    }

    public Task WaitAsync(CancellationToken cancellationToken = default)
    {
        Task task = Task.CompletedTask;
        
        lock (_lock)
        {
            if (DateTime.Now > _nextSecondCheckpoint)
            {
                _executedRequestsInCurrentSecond = 0;
                UpdateCheckpoints();
            }
        
            TaskCompletionSource taskCompletionSource = new TaskCompletionSource();
            cancellationToken.Register(() => taskCompletionSource.SetCanceled(cancellationToken));

            if (_queue.Count != 0 || ++_executedRequestsInCurrentSecond > MaxRequestsPerSecond)
            {
                _queue.AddLast(taskCompletionSource);
                cancellationToken.Register(() => RemoveTaskCompletionSource(taskCompletionSource));
            
                _timerTask ??= GetTimerTask();
            
                task = taskCompletionSource.Task;
            }
        }

        return task;
    }
}