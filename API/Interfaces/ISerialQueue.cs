using System;
using System.Threading.Tasks;

namespace RFRocketLibrary.API.Interfaces
{
    public interface ISerialQueue
    {
        Task? Enqueue(Action action);
        Task<T>? Enqueue<T>(Func<T> function);
        Task? Enqueue(Func<Task> asyncAction);
        Task<T>? Enqueue<T>(Func<Task<T>> asyncFunction);
    }
}