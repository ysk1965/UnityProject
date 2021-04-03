using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListUtil
{
    public static void Shuffle(IList list)
    {
        int count = list.Count;
        int last = count - 1;
        for (int i = 0; i < last; ++i)
        {
            int r = UnityEngine.Random.Range(i, count);
            var tmp = list[i];
            list[i] = list[r];
            list[r] = tmp;
        }
    }
}
