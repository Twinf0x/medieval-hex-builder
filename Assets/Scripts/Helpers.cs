using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Random = System.Random;

public static class Helpers
{
    private static Random rand = new Random();
    public static IEnumerable<TValue> RandomValues<TKey, TValue>(IDictionary<TKey, TValue> dict)
    {
        List<TValue> values = Enumerable.ToList(dict.Values);
        int size = dict.Count;
        while(true)
        {
            yield return values[rand.Next(size)];
        }
    }

    public static IEnumerable<TKey> RandomKeys<TKey, TValue>(IDictionary<TKey, TValue> dict)
    {
        List<TKey> keys = Enumerable.ToList(dict.Keys);
        int size = dict.Count;
        while(true)
        {
            yield return keys[rand.Next(size)];
        }
    }

    public static void Shuffle<T>(this IList<T> list)  
    {  
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = rand.Next(n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }  
    }
}
