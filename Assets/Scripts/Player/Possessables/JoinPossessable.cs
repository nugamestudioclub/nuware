using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class JoinPossessable : MonoBehaviour, IPossessable
{
    public void Initialize()
    {
        Debug.Log("Hello hello!");
    }

    public void OnButtonEvent(IDictionary<ButtonType, InputContextType> map)
    {
        Debug.Log(PrintDictionary(map));
    }

    public void OnLateralEvent((Vector2 Data, InputContextType ContextType) data_tuple)
    {
        // pass
    }

    private string PrintDictionary(IDictionary<ButtonType, InputContextType> map)
    {
        StringBuilder sb = new StringBuilder();

        foreach (var item in map)
        {
            sb.Append(item.ToString() + "\n");
        }

        return sb.ToString();
    }

}
