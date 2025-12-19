using System;
using System.Threading.Tasks;
using RFRocketLibrary.API.Interfaces;

namespace RFRocketLibrary.Models
{
    // Original from: https://github.com/Gentlee/SerialQueue
    // Original Author: Gentlee
    public class SerialQueue : ISerialQueue
    {
        private readonly object _locker = new();
        private readonly WeakReference<Task?> _lastTask = new(null);

        public Task? Enqueue(Action action)
        {
            return Enqueue(() =>
            {
                action();
                return true;
            });
        }

        public Task<T>? Enqueue<T>(Func<T> function)
        {
            lock (_locker)
            {
                var resultTask = _lastTask.TryGetTarget(out var lastTask)
                    ? lastTask?.ContinueWith(_ => function(), TaskContinuationOptions.ExecuteSynchronously)
                    : Task.Run(function);
                _lastTask.SetTarget(resultTask);
                return resultTask;
            }
        }

        public Task? Enqueue(Func<Task> asyncAction)
        {
            lock (_locker)
            {
                var resultTask = _lastTask.TryGetTarget(out var lastTask)
                    ? lastTask?.ContinueWith(_ => asyncAction(), TaskContinuationOptions.ExecuteSynchronously)
                        .Unwrap()
                    : Task.Run(asyncAction);
                _lastTask.SetTarget(resultTask);
                return resultTask;
            }
        }

        public Task<T>? Enqueue<T>(Func<Task<T>> asyncFunction)
        {
            lock (_locker)
            {
                var resultTask = _lastTask.TryGetTarget(out var lastTask)
                    ? lastTask?.ContinueWith(_ => asyncFunction(), TaskContinuationOptions.ExecuteSynchronously)
                        .Unwrap()
                    : Task.Run(asyncFunction);
                _lastTask.SetTarget(resultTask);
                return resultTask;
            }
        }
    }
}