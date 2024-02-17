using UnityEngine;

/// <summary>
/// Player data structure. Stores data about a player.
/// </summary>
[System.Serializable]
public struct PlayerData
{
    public int Health;
    public Color Color;

    public PlayerData(int hp, Color color)
    {
        Health = hp;
        Color = color;
    }
}
