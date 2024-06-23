using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A class for handling player management and storage.
/// </summary>
public class PartyManager : MonoBehaviour
{
    /// <summary>
    /// The max number of players to allow in the game.
    /// </summary>
    private const int MAX_PLAYERS = 4;

    /// <summary>
    /// The set of all player_number numbers in-use.
    /// </summary>
    private ISet<int> m_players;

    /// <summary>
    /// A map of player_number object Instance Ids to their respective player_number number.
    /// </summary>
    private Dictionary<int, int> m_playerIdNumberMap;

    /// <summary>
    /// A map of player_number numbers to their respective player_number data structure.
    /// </summary>
    private Dictionary<int, PlayerData> m_playerDataMap;

    private void Awake()
    {
        m_playerDataMap = new();
        m_playerIdNumberMap = new();

        m_players = new HashSet<int>();
    }

    /// <summary>
    /// Given a playerinput-containing object, attempts to find the input's associated player number.
    /// </summary>
    /// <param name="player_obj"></param>
    /// <returns></returns>
    public int GetPlayerNumber(GameObject player_obj) => m_playerIdNumberMap[player_obj.GetInstanceID()];

    /// <summary>
    /// Given a player number, attempts to get their associated player data information.
    /// </summary>
    /// <param name="player_number"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if an invalid player number is given.</exception>
    public PlayerData GetPlayerData(int player_number)
    {
        if (player_number > MAX_PLAYERS && player_number <= 0)
        {
            throw new ArgumentOutOfRangeException(string.Format("Player number {0} is invalid.", player_number));
        }

        return m_playerDataMap[player_number];
    }

    /// <summary>
    /// Registers the given gameobject as a player_number, assigning them the lowest-available player_number number.
    /// Adds them to the id-number map as well as the number-data map. The default values are -1 for health,
    /// a random color, and the associated PlayerInputHandler monobehavior. Additionally, the given <paramref name="player"/>
    /// is reparented to be a child of the PartyManager.
    /// </summary>
    /// <param name="player">The gameobject to register. Must have a PlayerInputHandler component.</param>
    /// <returns>The number of the added player_number.</returns>
    public int AddPlayerToParty(GameObject player)
    {
        if (m_players.Count > MAX_PLAYERS)
        {
            Debug.LogWarning("Player count max reached!");
            return -1;
        }

        // get lowest number free
        int player_number = AddLowestNumber();

        // update maps
        m_playerIdNumberMap.Add(player.GetInstanceID(), player_number);
        m_playerDataMap.Add(
            player_number, 
            new PlayerData(
                -1, 
                UnityEngine.Random.ColorHSV(0f, 1f, 1f, 1f, 1f, 1f), 
                player.GetComponent<PlayerInputHandler>()));
        m_players.Add(player_number);

        // reparent to manager so it remains persistant.
        player.transform.parent = transform;

        return player_number;
    }

    /// <summary>
    /// Attempts to remove the given player number from the party. Destroys the associated player_number input object as well.
    /// </summary>
    /// <param name="player_number">The player_number number to remove.</param>
    public void RemovePlayerFromParty(int player_number)
    {
        if (!m_players.Contains(player_number))
        {
            throw new ArgumentException(string.Format("Player number {0} does not exist in {1}", player_number, m_players.ToString()));
        }

        // remove from map and set
        m_playerDataMap.Remove(player_number); 
        m_players.Remove(player_number);

        // remove the associated input gameobject as well
        var player_obj = m_playerDataMap[player_number].InputHandler.gameObject;
        m_playerIdNumberMap.Remove(player_obj.GetInstanceID());

        Destroy(player_obj);
    }

    public void BindPlayer(int player_number, IAvatar target_avatar, bool do_initialize_avatar = true)
    {
        if (do_initialize_avatar) target_avatar.Initialize(player_number, this);

        m_playerDataMap[player_number].InputHandler.Possess(target_avatar);
    }

    public void UnbindPlayer(int player_number, bool invoke_destroy_avatar = true)
    {
        m_playerDataMap[player_number].InputHandler.Free(invoke_destroy_avatar);
    }

    private int AddLowestNumber()
    {
        var list = m_players.ToList(); // not great but whatever
        list.Sort();

        int low = 1;

        foreach (var item in list)
            if (low == item) low = item + 1;

        return low;
    }
}
