using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Random = System.Random;

public static class Helpers
{
    public static IEnumerable<TValue> RandomValues<TKey, TValue>(IDictionary<TKey, TValue> dict)
    {
        Random rand = new Random();
        List<TValue> values = Enumerable.ToList(dict.Values);
        int size = dict.Count;
        while(true)
        {
            yield return values[rand.Next(size)];
        }
    }

    public static IEnumerable<TKey> RandomKeys<TKey, TValue>(IDictionary<TKey, TValue> dict)
    {
        Random rand = new Random();
        List<TKey> keys = Enumerable.ToList(dict.Keys);
        int size = dict.Count;
        while(true)
        {
            yield return keys[rand.Next(size)];
        }
    }
}
