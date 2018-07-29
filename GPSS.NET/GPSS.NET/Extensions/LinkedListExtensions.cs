using System;
using System.Collections.Generic;
using System.Linq;

namespace GPSS.Extensions
{
    internal static class LinkedListExtensions
    {
        public static LinkedList<T> Clone<T>(this LinkedList<T> list) where T : ICloneable
        {
            return new LinkedList<T>(list.Select(t => (T)t.Clone()));
        }
    }
}
