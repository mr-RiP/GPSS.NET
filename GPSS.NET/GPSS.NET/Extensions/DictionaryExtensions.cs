using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
