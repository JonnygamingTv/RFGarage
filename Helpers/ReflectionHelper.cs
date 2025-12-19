using System;
using System.Diagnostics;
using System.Reflection;
using Rocket.Core.Logging;

namespace RFRocketLibrary.Utils
{
    public static class ReflectionHelper
    {
        #region Methods

        public static TAttribute? GetAttributeFrom<TAttribute>(object instance) where TAttribute : Attribute
        {
            try
            {
                object[]? attrs;
                if (instance is MethodInfo methodInfo)
                {
                    attrs = methodInfo.GetCustomAttributes(typeof(TAttribute), false);
                    return attrs.Length == 0 ? default : (TAttribute) attrs.GetValue(0);
                }
                attrs = instance.GetType().GetCustomAttributes(typeof(TAttribute), true);
                return attrs.Length == 0 ? default : (TAttribute) attrs.GetValue(0);
            }
            catch (Exception e)
            {
                var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                Logger.LogError($"[{caller}] [ERROR] ReflectUtil GetAttributeFrom: {e.Message}");
                Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                return null;
            }
        }

        public static FieldInfo? GetField(Type type, string name, BindingFlags flags = STATIC_INSTANCE_FLAGS)
        {
            try
            {
                return type.GetField(name, flags);
            }
            catch (Exception e)
            {
                var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                Logger.LogError($"[{caller}] [ERROR] ReflectUtil GetField: {e.Message}");
                Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                return null;
            }
        }

        public static FieldInfo? GetField<T>(string name)
        {
            return GetField(typeof(T), name);
        }

        public static MethodInfo? GetMethod(Type? type, string name, BindingFlags flags, Type[]? argTypes)
        {
            try
            {
                return type?.GetMethod(name, flags, null, CallingConventions.Any, argTypes ?? Type.EmptyTypes, null);
            }
            catch (Exception e)
            {
                var caller = new StackTrace().GetFrame(1).GetMethod().ReflectedType.Assembly.GetName().Name;
                Logger.LogError($"[{caller}] [ERROR] ReflectUtil GetField: {e.Message}");
                Logger.LogError($"[{caller}] [ERROR] Details: {e}");
                return null;
            }
        }

        public static MethodInfo? GetMethod(Type? type, string name, Type[]? argTypes = null)
        {
            return GetMethod(type, name, STATIC_INSTANCE_FLAGS, argTypes);
        }

        public static MethodInfo? GetMethod<T>(string name, BindingFlags flags, Type[]? argTypes)
        {
            return GetMethod(typeof(T), name, flags, argTypes);
        }

        public static MethodInfo? GetMethod<T>(string name, Type[]? argTypes = null)
        {
            return GetMethod(typeof(T), name, STATIC_INSTANCE_FLAGS, argTypes);
        }

        #endregion

        // Common binding flags
        public const BindingFlags INSTANCE_FLAGS = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        public const BindingFlags STATIC_FLAGS = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        public const BindingFlags STATIC_INSTANCE_FLAGS = INSTANCE_FLAGS | STATIC_FLAGS;

        // Empty object array, commonly used on method invocation.
        public static object[] EMPTY_ARGS => Array.Empty<object>();
    }
}