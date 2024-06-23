using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A class that provides listener implementations for when a player joins or leaves the game in the
/// main JoinScene.
/// </summary>
public class PlayerJoinManager : MonoBehaviour
{
    /// <summary>
    /// A list of transform origins for the positions to spawn avatars onto.
    /// </summary>
    [SerializeField]
    private List<Transform> m_podiumPositions;

    /// <summary>
    /// A reference to the party manager for modification of the party.
    /// exposed here because this is the only situation in which that works (partymanager is in the same scene).
    /// 
    /// DON'T do this in your own scenes.
    /// </summary>
    [SerializeField]
    private PartyManager m_party;

    /// <summary>
    /// The player avatar to spawn when a player joins.
    /// </summary>
    [SerializeField]
    private GameObject m_playerAvatar;

    /// <summary>
    /// Listener for when a player joins. Set up the player by creating an avatar for them to use and
    /// registering them in the party.
    /// </summary>
    /// <param name="input"></param>
    public void OnPlayerJoined(PlayerInput input)
    {
        // register player in party and set the name of their input handler for convenience
        int player_number = m_party.AddPlayerToParty(input.gameObject);
        input.gameObject.name = $"Player {player_number}";

        // create a join avatar for the player; todo replace with better avatar
        GameObject clone = Instantiate(m_playerAvatar);
        clone.transform.position = m_podiumPositions[player_number - 1].position + Vector3.up * 15f;

        // bind player to avatar
        m_party.BindPlayer(player_number, clone.GetComponent<IAvatar>());
    }

    /// <summary>
    /// Listener for when a player is removed by means of disabling their input source (i.e. turning off controller).
    /// Unbinds their avatar (and destroys it) and removes them from the party (and destroys their input).
    /// </summary>
    /// <param name="input"></param>
    public void OnPlayerRemoved(PlayerInput input)
    {
        int number = -1;
        
        try
        {
            number = m_party.GetPlayerNumber(input.gameObject);
        } 
        catch (KeyNotFoundException)
        {
            Debug.Log("Player number not found. This is fine to ignore if the player didn't disconnect via powering-off their controller.");
            return;
        }

        // free the player's avatar (and destroy it)
        m_party.UnbindPlayer(number);
        
        // unregister them from the party (and destroy their input)
        m_party.RemovePlayerFromParty(number);
    }
}
