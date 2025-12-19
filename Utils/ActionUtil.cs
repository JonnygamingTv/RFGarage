using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RFRocketLibrary.Utils
{
    public static class ActionUtil
    {
        public static IEnumerator<WaitForSeconds> Delay(Action action, float delay)
        {
            yield return new WaitForSeconds(delay);
            try
            {
                action.Invoke();
            }
            catch
            {
            }
        }

        public static IEnumerator Repeat(Action action, Func<bool> cancelCondition)
        {
            while (!cancelCondition())
            {
                try
                {
                    action.Invoke();
                }
                catch
                {
                    yield break;
                }

                yield return null;
            }
        }

        public static IEnumerator<WaitForSeconds> Repeat(Action action, float interval, Func<bool> cancelCondition)
        {
            while (!cancelCondition())
            {
                try
                {
                    action.Invoke();
                }
                catch
                {
                    yield break;
                }

                yield return new WaitForSeconds(interval);
            }
        }
    }
}