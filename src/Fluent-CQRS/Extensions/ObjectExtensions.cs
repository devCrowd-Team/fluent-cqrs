using System;
using fastJSON;

namespace Fluent_CQRS.Extensions
{
    public static class ObjectExtensions
    {
        private static readonly JSONParameters _jsonParams =
            new JSONParameters
            {
                EnableAnonymousTypes = true,
                SerializeNullValues = true,
                UseFastGuid = false
            };

        public static string AsJSON(this Object source)
        {
            return JSON.ToJSON(source, _jsonParams);
        }
    }
}
