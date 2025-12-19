using System;
using System.Collections.Generic;
using System.Diagnostics;
using RFRocketLibrary.Utils;
using UnityEngine;
using Action = System.Action;
using Logger = Rocket.Core.Logging.Logger;

namespace RocketExtensions.Models
{
    /// <summary>
    /// A faster high-performance version of <seealso cref="Rocket.Core.Utils.TaskDispatcher"/>
    /// </summary>
    public class FastTaskDispatcher : MonoBehaviour
    {
        private static bool Initialized { get; set; }
        private static GameObject? Object { get; set; }
        private static FastTaskDispatcher? Component { get; set; }

        private static void CheckInit()
        {
            if (Initialized)
                return;

            Initialized = true;
            Object = new GameObject("RocketExtensions.FastTaskDispatcher");
            DontDestroyOnLoad(Object);
            Component = Object.AddComponent<FastTaskDispatcher>();
        }

        public static void QueueOnMainThread(Action action, float time = 0f)
        {
            CheckInit();

            if (Component is null)
                return;

            if (time != 0)
                lock (Component.DelayedQueue)
                {
                    Component.DelayedQueue.Add(new DelayedAction {Time = Time.time + time, Action = action});
                }
            else
                lock (Component.Queue)
                {
                    Component.Queue.Add(action);
                    Component.QueueLength++;
                }
        }

        private int QueueLength { get; set; }

        private List<Action> Queue { get; } = new();
        private List<DelayedAction> DelayedQueue { get; } = new();


        public void FixedUpdate()
        {
            while (QueueLength > 0)
            {
                Action action;
                lock (Queue)
                {
                    QueueLength -= 1;
                    action = Queue[0];
                    Queue.RemoveAtFast(0);
                }

                try
                {
                    action();
                }
                catch (Exception e)
                {
                    var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                    Logger.LogError($"[{caller}] [ERROR] FastTaskDispatcher: {e.Message}");
                    Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                }
            }

            IEnumerable<DelayedAction> delayedActions;
            lock (DelayedQueue)
                delayedActions = DelayedQueue.GetAndRemoveAllFast(d => d.Time <= Time.time);

            foreach (var delayedAction in delayedActions)
            {
                try
                {
                    delayedAction.Action();
                }
                catch (Exception e)
                {
                    var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                    Logger.LogError($"[{caller}] [ERROR] FastTaskDispatcher: {e.Message}");
                    Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                }
            }
        }
    }

    public struct DelayedAction
    {
        public float Time;
        public Action Action;
    }
}