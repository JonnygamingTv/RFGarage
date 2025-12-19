using LiteDB;
using Rocket.Core.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RFGarage.DatabaseManagers
{
    public class LiteDbWrapper : IDisposable
    {
        private bool _disposed;
        private readonly SemaphoreSlim _writeLock = new SemaphoreSlim(1, 1);
        private readonly LiteDatabase _db;
        public LiteDbWrapper(string LiteDB_ConnectionString)
        {
            _db = new LiteDatabase(LiteDB_ConnectionString);
        }

        // Writes are serialized
        public async Task RunWriteAsync(Action<LiteDatabase> action)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(LiteDbWrapper));

            await _writeLock.WaitAsync().ConfigureAwait(false);
            try
            {
                action(_db);
            }
            catch (Exception ex)
            {
                Logger.LogError("LiteDB write failed");
                Logger.LogException(ex);
                throw;
            }
            finally
            {
                _writeLock.Release();
            }
        }

        public async Task<T> RunWriteAsync<T>(Func<LiteDatabase, T> action)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(LiteDbWrapper));

            await _writeLock.WaitAsync().ConfigureAwait(false);
            try
            {
                return action(_db);
            }
            finally
            {
                _writeLock.Release();
            }
        }

        // Reads do not lock at wrapper level
        public T RunRead<T>(Func<LiteDatabase, T> action)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(LiteDbWrapper));

            return action(_db);
        }

        public void RunRead(Action<LiteDatabase> action)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(LiteDbWrapper));

            action(_db);
        }

        public Task<T> RunReadAsync<T>(Func<LiteDatabase, T> action)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(LiteDbWrapper));

            // Run the synchronous action on a background thread to avoid blocking caller
            return Task.Run(() => action(_db));
        }
        public Task RunReadAsync(Action<LiteDatabase> action)
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(LiteDbWrapper));

            return Task.Run(() => action(_db));
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;
            _db.Dispose();
            _writeLock.Dispose();
        }
    }
}
