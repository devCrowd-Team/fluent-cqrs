using System;
using System.Reflection;

namespace Fluent_CQRS
{
    internal static class MessageExtensions
    {
        public  static bool Is<T>(this T t, Type type) 
        {
            return type.GetTypeInfo().IsAssignableFrom(t.GetType().GetTypeInfo());
        }
    }
}
