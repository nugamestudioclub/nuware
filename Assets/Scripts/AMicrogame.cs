using System.Collections.Generic;
using TNRD;
using UnityEngine;

public abstract class AMicrogame : MonoBehaviour, IMicrogame
{
    [SerializeField]
    private IList<int> m_currentWinners;

    [SerializeField]
    private SerializableInterface<IPossessable>[] m_possessibles = new SerializableInterface<IPossessable>[4];

    private PartyManager m_partyManager;

    public virtual float AwakeGame(IList<int> microgame_players, DifficultyType difficulty)
    {
        // everyone is a winner, initially :)
        m_currentWinners = microgame_players;

        m_partyManager = 
            PersistantManager.instance.FindInHierarchy(
                PersistantManager.instance.GetComponentPredicate<PartyManager>())
            .GetComponent<PartyManager>();

        if (!m_partyManager)
        {
            Debug.LogError("Party Manager not found!");
            return -1f;
        }

        // shuffle?

        for (int i = 0; i < m_possessibles.Length; i++)
        {
            m_partyManager.Possess(microgame_players[i], m_possessibles[i].Value);
            m_possessibles[i].Value.Initialize(microgame_players[i]);
        }

        return CalculateGameDuration(difficulty);
    }

    public virtual void EndGame()
    {
        foreach (int id in m_partyManager.GetPlayers())
            m_partyManager.Free(id);
    }

    public IList<int> GetWinners()
    {
        return m_currentWinners;
    }

    public abstract void StartGame();

    public abstract MicrogameData GetData();

    protected abstract float CalculateGameDuration(DifficultyType difficulty);
}
