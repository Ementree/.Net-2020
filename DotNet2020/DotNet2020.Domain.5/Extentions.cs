using System;
using System.Collections.Generic;
using System.Text;

namespace DotNet2020.Domain._5
{
    public static class Extentions
    {
        public static Dictionary<TKey, TValue> Clone<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            var clone = new Dictionary<TKey, TValue>();
            foreach (var item in dictionary)
                clone.Add(item.Key, item.Value);
            return clone;
        }

        public static IEnumerable<T> Clone<T>(this IEnumerable<T> values)
        {
            var list = new List<T>();
            foreach (var value in values)
                list.Add(value);
            return list;
        }
    }
}
