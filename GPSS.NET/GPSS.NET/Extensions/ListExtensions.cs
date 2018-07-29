using System;
using System.Collections.Generic;
using System.Linq;

namespace GPSS.Extensions
{
    internal static class ListExtensions
    {
        public static List<T> Clone<T>(this List<T> original) where T : ICloneable
        {
            return original.Select(item => (T)item.Clone()).ToList();
        }
    }
}
