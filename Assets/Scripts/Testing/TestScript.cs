using UnityEngine;
using UnityEngine.InputSystem;

public class TestScript : MonoBehaviour
{
    private int m_playerCount = 0;

    public void OnPlayerJoined(PlayerInput input)
    {
        string name = input.gameObject.name = $"Player {++m_playerCount}";

        Debug.Log($"Player has joined as {name}.");
    }
}
