using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    /// <summary>
    /// takes random elements from sourceList num times and adds it to the list.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="sourceList"></param>
    /// <param name="num"></param>
    public static void takeRandomElements<T>(this List<T> list, List<T> sourceList, int num)
    {
        for(int i = 0; i < num; i++)
        {
            int index = UnityEngine.Random.Range(0, sourceList.Count);
            list.Add(sourceList[index]);
            sourceList.RemoveAt(index);  
        }
        return; 
    }


    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
