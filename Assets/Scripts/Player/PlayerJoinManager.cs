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

    [SerializeField]
    private GameObject m_playerPossessible;

    private Dictionary<int, GameObject> m_idToRepresentation;

    private void Awake()
    {
        m_idToRepresentation = new();
    }

    public void OnPlayerJoined(PlayerInput input)
    {
        int player_index = m_party.AddPlayer(input.gameObject);

        // todo replace
        GameObject clone = Instantiate(m_playerPossessible);

        clone.transform.position = m_podiumPositions[player_index - 1].position + Vector3.up * 15f;
        clone.AddComponent<Rigidbody>();
        clone.GetComponent<MeshRenderer>().material.color = m_party.GetPlayerData(player_index).Color;

        m_idToRepresentation.Add(player_index, clone);

        m_party.Possess(player_index, clone.GetComponent<IPossessable>());
    }

    public void OnPlayerRemoved(PlayerInput input)
    {
        int id = m_party.GetPlayerID(input.gameObject);

        m_party.RemovePlayer(input.gameObject);
        m_idToRepresentation.Remove(id);
        m_party.Free(id);

        input.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.back * 15f, ForceMode.Impulse);

        Destroy(input.gameObject, 2f);
    }
}
