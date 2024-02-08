using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    [SerializeField]
    private int m_maxPlayers = 4;

    [SerializeField]
    private List<Color> m_playerColors = new()
    {
        Color.red, Color.green, Color.blue, Color.yellow
    };

    private int m_numPlayers;

    private Dictionary<string, PlayerData> m_playerMap;
    private Dictionary<string, PlayerInputHandler> m_playerInputMap;

    private void Awake()
    {
        m_numPlayers = 0;
        m_playerMap = new();
        m_playerInputMap = new();
    }

    public int GetPlayerCount() => m_numPlayers;

    public string[] GetPlayerNames() => m_playerMap.Keys.ToArray();

    public PlayerData GetPlayerData(string name) => m_playerMap[name];

    public string AddPlayer(GameObject player)
    {
        if (m_numPlayers > m_maxPlayers)
        {
            Debug.LogWarning("Player count max reached!");
            return "";
        }

        player.name = $"Player {++m_numPlayers}";

        m_playerMap.Add(player.name, new PlayerData(-1, m_playerColors[m_numPlayers - 1])); // real values set in UI
        m_playerInputMap.Add(player.name, player.GetComponent<PlayerInputHandler>());

        player.transform.parent = transform;

        return player.name;
    }

    public void RemovePlayer(GameObject player)
    {
        m_playerMap.Remove(player.name);
        m_playerInputMap.Remove(player.name);
        m_numPlayers--;

        Destroy(player);
    }

    public void Possess(string player, IPossessable target) => m_playerInputMap[player].Possess(target);

    public void Free(string player) => m_playerInputMap[player].Free();
}
