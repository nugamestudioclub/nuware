using System.Collections.Generic;
using UnityEngine;

public abstract class AMicrogame : MonoBehaviour, IMicrogame
{
    [SerializeField]
    private IList<PlayerData> m_currentWinners;

    public void AwakeGame(IList<PlayerData> microgame_players)
    {
        // everyone is a winner, initially :)
        m_currentWinners = microgame_players;
    }

    public IList<PlayerData> GetWinners()
    {
        return m_currentWinners;
    }

    public abstract void StartGame();
    public abstract void EndGame();

    public abstract MicrogameData GetData();
}
