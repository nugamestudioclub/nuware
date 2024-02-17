using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrogameManager : MonoBehaviour
{
    public static MicrogameData CurrentMicrogameData;

    [SerializeField]
    private List<string> m_microgamePool;

    private string m_previousGame;


    private string PickNextGame()
    {
        if (m_microgamePool.Count == 1) return m_microgamePool[0];
        if (m_microgamePool.Count == 0)
        {
            Debug.LogError("There are no microgames in the pool!");
            return "";
        }

        string game;
        do
        {
            game = m_microgamePool[Random.Range(0, m_microgamePool.Count)];
        } while (!game.Equals(m_previousGame));

        return game;
    }


}
