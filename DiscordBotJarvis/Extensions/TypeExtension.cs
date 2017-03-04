using System.Collections;
using System.Collections.Generic;

namespace DiscordBotJarvis.Extensions
{
    public static class TypeExtension
    {
        public static bool IsCollection(this object obj)
        {
            if (obj == null) return false;
            return obj is ICollection &&
                   obj.GetType().IsGenericType &&
                   obj.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(ICollection<>));
        }

        public static bool IsList(this object obj)
        {
            if (obj == null) return false;
            return obj is IList &&
                   obj.GetType().IsGenericType &&
                   obj.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }

        public static bool IsEnumerable(this object obj)
        {
            if (obj == null) return false;
            return obj is IEnumerable &&
                   obj.GetType().IsGenericType &&
                   obj.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(IEnumerable<>));
        }

        public static bool IsDictionary(this object obj)
        {
            if (obj == null) return false;
            return obj is IDictionary &&
                   obj.GetType().IsGenericType &&
                   obj.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>));
        }
    }
}
