using System.Collections.Generic;
using TNRD;
using UnityEngine;

public abstract class AMicrogame : MonoBehaviour, IMicrogame
{
    [SerializeField]
    private IList<int> m_currentWinners;

    [SerializeField]
    private SerializableInterface<IAvatar>[] m_avatars = new SerializableInterface<IAvatar>[4];

    private PartyManager m_partyManager;

    public virtual float AwakeGame(IList<int> microgame_players, DifficultyType difficulty)
    {
        var persistance = PersistantManager.instance;

        // everyone is a winner, initially :)
        m_currentWinners = microgame_players;

        m_partyManager = 
            persistance.FindInHierarchy(
                persistance.GetComponentPredicate<PartyManager>())
            .GetComponent<PartyManager>();

        if (!m_partyManager)
        {
            Debug.LogError("Party Manager not found!");
            return -1f;
        }

        // shuffle?

        for (int i = 0; i < m_avatars.Length; i++)
        {
            m_partyManager.BindPlayer(microgame_players[i], m_avatars[i].Value);
        }

        return CalculateGameDuration(difficulty);
    }

    /// <summary>
    /// UnbindPlayer all avatar player binds. Assumes there to be 4 players.
    /// </summary>
    public virtual void EndGame()
    {
        m_partyManager.UnbindPlayer(1);
        m_partyManager.UnbindPlayer(2);
        m_partyManager.UnbindPlayer(3);
        m_partyManager.UnbindPlayer(4);
    }

    public IList<int> GetWinners()
    {
        return m_currentWinners;
    }

    public abstract void StartGame();

    public abstract MicrogameData GetData();

    protected abstract float CalculateGameDuration(DifficultyType difficulty);
}
