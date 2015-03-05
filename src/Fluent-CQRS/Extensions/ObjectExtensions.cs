using System;
using fastJSON;

namespace Fluent_CQRS.Extensions
{
    public static class ObjectExtensions
    {
        public static string AsJSON(this Object source)
        {
            return JSON.ToJSON(source, new JSONParameters
            {
                EnableAnonymousTypes = true,
                SerializeNullValues = true,
                UseFastGuid = false
            });
        }
    }
}
