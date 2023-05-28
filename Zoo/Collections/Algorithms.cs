using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoo.Collections
{
    public static class Algorithms
    {
        public static T? Find<T>(IMyIterator<T> it, Predicate<T> predicate) where T : class
        {
            for (T obj = it.First(); !it.IsCompleted; obj = it.Next())
            {
                if (predicate(obj))
                {
                    return obj;
                }
            }
            return null;
        }
        public static void ForEach<T>(IMyIterator<T> it, Action<T> func)
        {
            for (T obj = it.First(); !it.IsCompleted; obj = it.Next())
            {
                func(obj);
            }
        }
        public static int CountIf<T>(IMyIterator<T> it, Predicate<T> predicate)
        {
            int count = 0;
            for (T obj = it.First(); !it.IsCompleted; obj = it.Next())
            {
                if (predicate(obj))
                {
                    count++;
                }
            }
            return count;
        }
    }

}
