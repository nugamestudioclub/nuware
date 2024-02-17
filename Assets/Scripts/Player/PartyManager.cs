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

    private ISet<int> m_players;

    private Dictionary<int, int> m_playerInstanceIDMap;
    private Dictionary<int, PlayerData> m_playerDataMap;
    private Dictionary<int, PlayerInputHandler> m_playerInputMap;

    private void Awake()
    {
        m_playerDataMap = new();
        m_playerInputMap = new();
        m_playerInstanceIDMap = new();

        m_players = new HashSet<int>();
    }

    public int GetPlayerCount() => m_players.Count;

    public int[] GetPlayers() => m_players.ToArray();

    public PlayerData GetPlayerData(int id) => m_playerDataMap[id];

    public int GetPlayerID(GameObject player_obj) => m_playerInstanceIDMap[player_obj.GetInstanceID()];

    private int AddLowestNumber()
    {
        var list = m_players.ToList(); // not great but whatever
        list.Sort();

        int low = 1;

        foreach (var item in list)
            if (low == item) low = item + 1;

        return low;
    }

    public int AddPlayer(GameObject player)
    {
        if (m_players.Count > m_maxPlayers)
        {
            Debug.LogWarning("Player count max reached!");
            return -1;
        }

        int player_id = AddLowestNumber();
        player.name = $"Player {player_id}";

        m_playerInstanceIDMap.Add(player.GetInstanceID(), player_id);
        m_playerDataMap.Add(player_id, new PlayerData(-1, m_playerColors[player_id - 1])); // player HP is set upon game start
        m_playerInputMap.Add(player_id, player.GetComponent<PlayerInputHandler>());

        player.transform.parent = transform;

        return player_id;
    }

    public void RemovePlayer(GameObject player)
    {
        int id = m_playerInstanceIDMap[player.GetInstanceID()];

        m_playerDataMap.Remove(id); 
        m_playerInputMap.Remove(id);
        m_players.Remove(id);
        m_playerInstanceIDMap.Remove(player.GetInstanceID());

        Destroy(player);
    }

    public void Possess(int player, IPossessable target) => m_playerInputMap[player].Possess(target);

    public void Free(int player) => m_playerInputMap[player].Free();
}
