using System;
using System.Collections.Generic;

namespace Utilities
{
    public static class DictionaryExtensions
    {
        public static TValue GetOrAdd<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TValue> valueFactory)
        {
            if (!dictionary.TryGetValue(key, out var value))
            {
                value = valueFactory();
                dictionary[key] = value;
            }
            return value;
        }
    }
}