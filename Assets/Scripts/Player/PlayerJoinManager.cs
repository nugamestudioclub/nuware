using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoinManager : MonoBehaviour
{
    [SerializeField]
    private List<Transform> m_podiumPositions;

    // exposed here because this is the only situation in which that works.
    // (partymanager is in the same scene)
    [SerializeField]
    private PartyManager m_party;

    public void OnPlayerJoined(PlayerInput input)
    {
        string player_name = m_party.AddPlayer(input.gameObject);

        // todo replace
        GameObject temp = GameObject.CreatePrimitive(PrimitiveType.Cube);
        int num = m_party.GetPlayerCount();

        temp.transform.position = m_podiumPositions[num - 1].position + Vector3.up * 25f;
        temp.AddComponent<Rigidbody>();
        temp.GetComponent<MeshRenderer>().material.color = m_party.GetPlayerData(player_name).Color;
    }

    // doesn't fully work as of yet bc it doesn't "free up" the slot of the disconnected player.
    // i.e. if someone leaves at 4 players and rejoins, we'll have a "player 5" at 4 players.
    public void OnPlayerRemoved(PlayerInput input)
    {
        m_party.RemovePlayer(input.gameObject);
    }
}
