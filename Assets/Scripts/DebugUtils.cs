using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DebugUtils
{
    public static void PrintDictionary<K,V>(IDictionary<K, V> map)
    {
        StringBuilder sb = new StringBuilder();

        foreach (var item in map)
        {
            sb.Append(item.ToString() + "\n");
        }

        Debug.Log(sb.ToString());
    }
}
