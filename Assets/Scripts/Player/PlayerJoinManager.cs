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
    private GameObject m_playerAvatar;

    private Dictionary<int, GameObject> m_numberToAvatar;

    private void Awake()
    {
        m_numberToAvatar = new();
    }

    public void OnPlayerJoined(PlayerInput input)
    {
        int player_number = m_party.AddPlayer(input.gameObject);
        input.gameObject.name = $"Player {player_number}";

        // todo replace
        GameObject clone = Instantiate(m_playerAvatar);

        clone.transform.position = m_podiumPositions[player_number - 1].position + Vector3.up * 15f;
        clone.GetComponent<MeshRenderer>().material.color = m_party.GetPlayerData(player_number).Color;

        m_numberToAvatar.Add(player_number, clone);

        IAvatar avatar = clone.GetComponent<IAvatar>();
        avatar.Initialize(player_number, m_party);

        m_party.Possess(player_number, avatar);
    }

    public void OnPlayerRemoved(PlayerInput input)
    {
        int number = m_party.GetPlayerNumber(input.gameObject);

        m_party.Free(number);
        m_party.RemovePlayer(number);
        m_numberToAvatar[number].GetComponent<Rigidbody>().AddForce(Vector3.up * Random.Range(1f, 15f) + Vector3.forward * Random.Range(-5f, 5f), ForceMode.Impulse);

        Destroy(input.gameObject, 2f);
        Destroy(m_numberToAvatar[number], 4f);

        m_numberToAvatar.Remove(number);
    }
}
