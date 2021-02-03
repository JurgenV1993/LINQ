using System;
using System.Collections.Generic;
using System.Text;

namespace LinqSamples
{
    public static class MyLinq
    {
        public static IEnumerable<T> Filter<T>(IEnumerable <T> source, Func<T,bool> predicate) 
        {
            var result = new List<T>();
            foreach (var item in source) 
            {
                if (predicate(item)) 
                {
                    result.Add(item);
                }
            }
            return result;
        }
    }
}
