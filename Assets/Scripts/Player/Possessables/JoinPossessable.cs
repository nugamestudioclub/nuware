using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinPossessable : MonoBehaviour, IPossessable
{
    public void Initialize()
    {
        Debug.Log("Hello hello!");
    }

    public void OnButtonEvent(IDictionary<ButtonType, InputContextType> map)
    {
        if (map[ButtonType.East] == InputContextType.Started) Debug.Log("Pressed a button!");
        if (map[ButtonType.North] == InputContextType.Started) Debug.Log("Quit.");
    }

    public void OnLateralEvent((Vector2 Data, InputContextType ContextType) data_tuple)
    {
        // pass
    }
}
