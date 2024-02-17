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
        int player_id = m_party.AddPlayer(input.gameObject);

        // todo replace
        GameObject clone = Instantiate(m_playerPossessible);

        clone.transform.position = m_podiumPositions[player_id - 1].position + Vector3.up * 15f;
        clone.GetComponent<MeshRenderer>().material.color = m_party.GetPlayerData(player_id).Color;

        m_idToRepresentation.Add(player_id, clone);

        IPossessable possessable = clone.GetComponent<IPossessable>();
        possessable.Initialize(player_id);

        m_party.Possess(player_id, possessable);
    }

    public void OnPlayerRemoved(PlayerInput input)
    {
        int id = m_party.GetPlayerID(input.gameObject);

        m_party.Free(id);
        m_party.RemovePlayer(input.gameObject);
        m_idToRepresentation[id].GetComponent<Rigidbody>().AddForce(Vector3.up * Random.Range(1f, 15f) + Vector3.forward * Random.Range(-5f, 5f), ForceMode.Impulse);

        Destroy(input.gameObject, 2f);
        Destroy(m_idToRepresentation[id], 4f);

        m_idToRepresentation.Remove(id);
    }
}
