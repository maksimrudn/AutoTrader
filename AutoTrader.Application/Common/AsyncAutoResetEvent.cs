using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoTrader.Application.Common
{
    /// <summary>
    /// Taken from here
    /// https://stackoverflow.com/questions/32654509/awaitable-autoresetevent
    /// 
    /// Additional usefull information
    /// https://devblogs.microsoft.com/pfxteam/building-async-coordination-primitives-part-2-asyncautoresetevent/
    /// </summary>
    public sealed class AsyncAutoResetEvent: IDisposable
    {
        private bool _disposed = false;

        public AsyncAutoResetEvent(bool signaled)
        {
            _signaled = signaled;
        }

        private readonly Queue<(TaskCompletionSource, Task)> _queue = new();

        private bool _signaled = false;

        public Task WaitOne(int? timeout = null)
        {
            lock (_queue)
            {
                if (_signaled)
                {
                    _signaled = false;
                    return Task.CompletedTask;
                }
                else
                {
                    var tcs = new TaskCompletionSource();
                    Task timeoutTask = null;
                    if (timeout != null)
                    {
                        CancellationTokenSource cts = new CancellationTokenSource(timeout.Value);
                        var cancellationToken = cts.Token;

                        // If the token is cancelled, cancel the waiter.
                        var registration = cancellationToken.Register(() =>     
                                                            tcs.TrySetException(new TimeoutException()), 
                                                                                useSynchronizationContext: false);
                        
                        // If the waiter completes or faults, unregister our interest in cancellation.
                        tcs.Task.ContinueWith(
                            _ => registration.Unregister(),
                            cancellationToken,
                            TaskContinuationOptions.OnlyOnRanToCompletion | TaskContinuationOptions.NotOnFaulted,
                            TaskScheduler.Default);
                    }
                    _queue.Enqueue((tcs, timeoutTask));
                    return tcs.Task;
                }
            }
        }

        public void Set()
        {
            (TaskCompletionSource, Task)? toRelease = null;

            lock (_queue)
            {
                if (_queue.Count > 0)
                {
                    toRelease = _queue.Dequeue();
                }
                else if (!_signaled)
                {
                    _signaled = true;
                }
            }

            // It's possible that the TCS has already been cancelled.
            toRelease?.Item1.TrySetResult();
            toRelease?.Item2?.Dispose();
        }

        public void Reset()
        {
            lock (_queue)
            {
                _signaled = false;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    lock (_queue)
                    {
                        while (_queue.Count > 0)
                        {
                            var toRelease = _queue.Dequeue();
                            toRelease.Item1.TrySetCanceled();
                            toRelease.Item2?.Dispose();
                        }
                    }
                }

                _disposed = true;
            }
        }
    }
}
