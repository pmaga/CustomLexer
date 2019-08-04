using System;
using System.Collections.Generic;

namespace CustomLexer.Extensions
{
    public static class DictionaryExtensions
    {
        public static T GetOrAdd<T>(this Dictionary<string, T> results, string key, Func<T> createFunc)
        {
            T item;
            if (results.ContainsKey(key))
            {
                item = results[key];
            }
            else
            {
                item = createFunc();
                results.Add(key, item);
            }
            return item;
        }
    }
}