using System;
using System.Collections.Generic;
using System.Linq;

namespace GPSS.Extensions
{
    internal static class DictionaryExtensions
    {
        public static Dictionary<TKey, TValue> Clone<TKey, TValue>(this Dictionary<TKey, TValue> original) where TValue : ICloneable
        {
            Dictionary<TKey, TValue> ret = new Dictionary<TKey, TValue>(original.Count, original.Comparer);
            foreach (KeyValuePair<TKey, TValue> entry in original)
                ret.Add(entry.Key, (TValue)entry.Value.Clone());

            return ret;
        }

        public static Dictionary<TKey, int> CloneMap<TKey, TKeyParent>(this Dictionary<TKey, int> map, List<TKeyParent> list) where TKey : TKeyParent
        {
            return map.Values
                .Select(v => new KeyValuePair<TKey, int>((TKey)list[v], v))
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}
