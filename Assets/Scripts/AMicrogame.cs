using System.Collections.Generic;
using TNRD;
using UnityEngine;

public abstract class AMicrogame : MonoBehaviour, IMicrogame
{
    [SerializeField]
    private IList<string> m_currentWinners;

    [SerializeField]
    private SerializableInterface<IPossessable>[] m_possessibles;

    private PartyManager m_partyManager;

    public virtual void AwakeGame(IList<string> microgame_players)
    {
        // everyone is a winner, initially :)
        m_currentWinners = microgame_players;

        m_partyManager = PersistantManager.instance.FindInHierarchy(
            PersistantManager.instance.GetComponentPredicate<PartyManager>())
            .GetComponent<PartyManager>();

        if (!m_partyManager)
        {
            Debug.LogError("Party Manager not found!");
            return;
        }

        // shuffle?

        for (int i = 0; i < m_possessibles.Length; i++)
        {
            m_partyManager.Possess(microgame_players[i], m_possessibles[i].Value);
            m_possessibles[i].Value.Initialize();
        }
    }

    public virtual void EndGame()
    {
        foreach (string name in m_partyManager.GetPlayerNames())
            m_partyManager.Free(name);
    }

    public IList<string> GetWinners()
    {
        return m_currentWinners;
    }

    public abstract void StartGame();

    public abstract MicrogameData GetData();
}
