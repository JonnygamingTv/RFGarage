using System;
using System.Threading;
using System.Threading.Tasks;
using RocketExtensions.Models;

namespace RocketExtensions.Utilities
{
    /// <summary>
    /// A collection of embedded tools to help manage threading and async operations
    /// </summary>
    public static class ThreadTool
    {
        public delegate void VoidPattern();

        public delegate void VoidPattern<in TA>(TA arg1);

        public delegate void VoidPattern<in TA, in TB>(TA arg1, TB arg2);

        public delegate void VoidPattern<in TA, in TB, in TC>(TA arg1, TB arg2, TC arg3);

        public delegate void VoidPattern<in TA, in TB, in TC, in TD>(TA arg1, TB arg2, TC arg3, TD arg4);

        public delegate void
            VoidPattern<in TA, in TB, in TC, in TD, in TE>(TA arg1, TB arg2, TC arg3, TD arg4, TE arg5);

        public delegate Task TaskPattern();

        public delegate Task TaskPattern<in TA>(TA arg1);

        public delegate Task TaskPattern<in TA, in TB>(TA arg1, TB arg2);

        public delegate Task TaskPattern<in TA, in TB, in TC>(TA arg1, TB arg2, TC arg3);

        public delegate Task TaskPattern<in TA, in TB, in TC, in TD>(TA arg1, TB arg2, TC arg3, TD arg4);

        public delegate Task
            TaskPattern<in TA, in TB, in TC, in TD, in TE>(TA arg1, TB arg2, TC arg3, TD arg4, TE arg5);

        public delegate T FuncPattern<out T>();

        public delegate T FuncPattern<out T, in TA>(TA arg1);

        public delegate T FuncPattern<out T, in TA, in TB>(TA arg1, TB arg2);

        public delegate T FuncPattern<out T, in TA, in TB, in TC>(TA arg1, TB arg2, TC arg3);

        public delegate T FuncPattern<out T, in TA, in TB, in TC, in TD>(TA arg1, TB arg2, TC arg3, TD arg4);

        public delegate T FuncPattern<out T, in TA, in TB, in TC, in TD, in TE>(TA arg1, TB arg2, TC arg3, TD arg4,
            TE arg5);

        public static async Task RunOnGameThreadAsync(VoidPattern action)
        {
            var callback = new TaskCompletionSource<Exception?>();

            FastTaskDispatcher.QueueOnMainThread(() =>
            {
                try
                {
                    action();
                    callback.SetResult(null);
                }
                catch (Exception? ex)
                {
                    callback.SetResult(ex);
                }
            });

            var err = await callback.Task;
            if (err != null)
            {
                throw err;
            }
        }

        public static async Task RunOnGameThreadAsync<TA>(VoidPattern<TA> action, TA arg1)
        {
            var callback = new TaskCompletionSource<Exception?>();

            FastTaskDispatcher.QueueOnMainThread(() =>
            {
                try
                {
                    action(arg1);
                    callback.SetResult(null);
                }
                catch (Exception? ex)
                {
                    callback.SetResult(ex);
                }
            });

            var err = await callback.Task;
            if (err != null)
            {
                throw err;
            }
        }

        public static async Task RunOnGameThreadAsync<TA, TB>(VoidPattern<TA, TB> action, TA arg1, TB arg2)
        {
            var callback = new TaskCompletionSource<Exception?>();

            FastTaskDispatcher.QueueOnMainThread(() =>
            {
                try
                {
                    action(arg1, arg2);
                    callback.SetResult(null);
                }
                catch (Exception? ex)
                {
                    callback.SetResult(ex);
                }
            });

            var err = await callback.Task;
            if (err != null)
            {
                throw err;
            }
        }

        public static async Task RunOnGameThreadAsync<TA, TB, TC>(VoidPattern<TA, TB, TC> action, TA arg1, TB arg2,
            TC arg3)
        {
            var callback = new TaskCompletionSource<Exception?>();

            FastTaskDispatcher.QueueOnMainThread(() =>
            {
                try
                {
                    action(arg1, arg2, arg3);
                    callback.SetResult(null);
                }
                catch (Exception? ex)
                {
                    callback.SetResult(ex);
                }
            });

            var err = await callback.Task;
            if (err != null)
            {
                throw err;
            }
        }

        public static async Task RunOnGameThreadAsync<TA, TB, TC, TD>(VoidPattern<TA, TB, TC, TD> action, TA arg1,
            TB arg2, TC arg3, TD arg4)
        {
            var callback = new TaskCompletionSource<Exception?>();

            FastTaskDispatcher.QueueOnMainThread(() =>
            {
                try
                {
                    action(arg1, arg2, arg3, arg4);
                    callback.SetResult(null);
                }
                catch (Exception? ex)
                {
                    callback.SetResult(ex);
                }
            });

            var err = await callback.Task;
            if (err != null)
            {
                throw err;
            }
        }

        public static async Task RunOnGameThreadAsync<TA, TB, TC, TD, TE>(VoidPattern<TA, TB, TC, TD, TE> action,
            TA arg1, TB arg2, TC arg3, TD arg4, TE arg5)
        {
            var callback = new TaskCompletionSource<Exception?>();

            FastTaskDispatcher.QueueOnMainThread(() =>
            {
                try
                {
                    action(arg1, arg2, arg3, arg4, arg5);
                    callback.SetResult(null);
                }
                catch (Exception? ex)
                {
                    callback.SetResult(ex);
                }
            });

            var err = await callback.Task;
            if (err != null)
            {
                throw err;
            }
        }

        public static async Task<T?> RunOnGameThreadAsync<T>(FuncPattern<T> action)
        {
            var callback = new TaskCompletionSource<Exception?>();
            var argue = default(T);
            FastTaskDispatcher.QueueOnMainThread(() =>
            {
                try
                {
                    argue = action();
                    callback.SetResult(null);
                }
                catch (Exception? ex)
                {
                    callback.SetResult(ex);
                }
            });

            var err = await callback.Task;
            if (err != null)
            {
                throw err;
            }

            return argue;
        }

        public static async Task<T?> RunOnGameThreadAsync<T, TA>(FuncPattern<T, TA> action, TA arg1)
        {
            var callback = new TaskCompletionSource<Exception?>();
            var argue = default(T);
            FastTaskDispatcher.QueueOnMainThread(() =>
            {
                try
                {
                    argue = action(arg1);
                    callback.SetResult(null);
                }
                catch (Exception? ex)
                {
                    callback.SetResult(ex);
                }
            });

            var err = await callback.Task;
            if (err != null)
            {
                throw err;
            }

            return argue;
        }

        public static async Task<T?> RunOnGameThreadAsync<T, TA, TB>(FuncPattern<T, TA, TB> action, TA arg1,
            TB arg2)
        {
            var callback = new TaskCompletionSource<Exception?>();
            var argue = default(T);
            FastTaskDispatcher.QueueOnMainThread(() =>
            {
                try
                {
                    argue = action(arg1, arg2);
                    callback.SetResult(null);
                }
                catch (Exception? ex)
                {
                    callback.SetResult(ex);
                }
            });

            var err = await callback.Task;
            if (err != null)
            {
                throw err;
            }

            return argue;
        }

        public static async Task<T?> RunOnGameThreadAsync<T, TA, TB, TC>(FuncPattern<T, TA, TB, TC> action, TA arg1,
            TB arg2, TC arg3)
        {
            var callback = new TaskCompletionSource<Exception?>();
            var argue = default(T);
            FastTaskDispatcher.QueueOnMainThread(() =>
            {
                try
                {
                    argue = action(arg1, arg2, arg3);
                    callback.SetResult(null);
                }
                catch (Exception? ex)
                {
                    callback.SetResult(ex);
                }
            });

            var err = await callback.Task;
            if (err != null)
            {
                throw err;
            }

            return argue;
        }

        public static async Task<T?> RunOnGameThreadAsync<T, TA, TB, TC, TD>(FuncPattern<T, TA, TB, TC, TD> action,
            TA arg1, TB arg2, TC arg3, TD arg4)
        {
            var callback = new TaskCompletionSource<Exception?>();
            var argue = default(T);
            FastTaskDispatcher.QueueOnMainThread(() =>
            {
                try
                {
                    argue = action(arg1, arg2, arg3, arg4);
                    callback.SetResult(null);
                }
                catch (Exception? ex)
                {
                    callback.SetResult(ex);
                }
            });

            var err = await callback.Task;
            if (err != null)
            {
                throw err;
            }

            return argue;
        }

        public static async Task<T?> RunOnGameThreadAsync<T, TA, TB, TC, TD, TE>(
            FuncPattern<T, TA, TB, TC, TD, TE> action, TA arg1, TB arg2, TC arg3, TD arg4, TE arg5)
        {
            var callback = new TaskCompletionSource<Exception?>();
            var argue = default(T);
            FastTaskDispatcher.QueueOnMainThread(() =>
            {
                try
                {
                    argue = action(arg1, arg2, arg3, arg4, arg5);
                    callback.SetResult(null);
                }
                catch (Exception? ex)
                {
                    callback.SetResult(ex);
                }
            });
            var err = await callback.Task;
            if (err != null)
            {
                throw err;
            }

            return argue;
        }

        public static void QueueOnThreadPool(TaskPattern task)
        {
            async void CallBack(object _) => await task();

            ThreadPool.QueueUserWorkItem(CallBack);
        }

        public static void QueueOnThreadPool<TA>(TaskPattern<TA> task, TA arg1)
        {
            async void CallBack(object _) => await task(arg1);

            ThreadPool.QueueUserWorkItem(CallBack);
        }

        public static void QueueOnThreadPool<TA, TB>(TaskPattern<TA, TB> task, TA arg1, TB arg2)
        {
            async void CallBack(object _) => await task(arg1, arg2);

            ThreadPool.QueueUserWorkItem(CallBack);
        }

        public static void QueueOnThreadPool<TA, TB, TC>(TaskPattern<TA, TB, TC> task, TA arg1, TB arg2, TC arg3)
        {
            async void CallBack(object _) => await task(arg1, arg2, arg3);

            ThreadPool.QueueUserWorkItem(CallBack);
        }

        public static void QueueOnThreadPool<TA, TB, TC, TD>(TaskPattern<TA, TB, TC, TD> task, TA arg1, TB arg2,
            TC arg3, TD arg4)
        {
            async void CallBack(object _) => await task(arg1, arg2, arg3, arg4);

            ThreadPool.QueueUserWorkItem(CallBack);
        }

        public static void QueueOnThreadPool<TA, TB, TC, TD, TE>(TaskPattern<TA, TB, TC, TD, TE> task, TA arg1,
            TB arg2,
            TC arg3, TD arg4, TE arg5)
        {
            async void CallBack(object _) => await task(arg1, arg2, arg3, arg4, arg5);

            ThreadPool.QueueUserWorkItem(CallBack);
        }

        public static void QueueOnThreadPool(VoidPattern task)
        {
            ThreadPool.QueueUserWorkItem(_ => task());
        }

        public static void QueueOnThreadPool<TA>(VoidPattern<TA> task, TA arg1)
        {
            ThreadPool.QueueUserWorkItem(_ => task(arg1));
        }

        public static void QueueOnThreadPool<TA, TB>(VoidPattern<TA, TB> task, TA arg1, TB arg2)
        {
            ThreadPool.QueueUserWorkItem(_ => task(arg1, arg2));
        }

        public static void QueueOnThreadPool<TA, TB, TC>(VoidPattern<TA, TB, TC> task, TA arg1, TB arg2, TC arg3)
        {
            ThreadPool.QueueUserWorkItem(_ => task(arg1, arg2, arg3));
        }

        public static void QueueOnThreadPool<TA, TB, TC, TD>(VoidPattern<TA, TB, TC, TD> task, TA arg1, TB arg2,
            TC arg3, TD arg4)
        {
            ThreadPool.QueueUserWorkItem(_ => task(arg1, arg2, arg3, arg4));
        }

        public static void QueueOnThreadPool<TA, TB, TC, TD, TE>(VoidPattern<TA, TB, TC, TD, TE> task, TA arg1,
            TB arg2,
            TC arg3, TD arg4, TE arg5)
        {
            ThreadPool.QueueUserWorkItem(_ => task(arg1, arg2, arg3, arg4, arg5));
        }
    }
}