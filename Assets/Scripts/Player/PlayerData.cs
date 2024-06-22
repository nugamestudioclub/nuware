using UnityEngine;

/// <summary>
/// Player data class. Stores data about a player like their current score, color, and a reference to their
/// input handler object. Not a structure.
/// </summary>
[System.Serializable]
public class PlayerData
{
    /// <summary>
    /// The score of the player. Synonymous with their "health" in competitive modes.
    /// </summary>
    public int Score;

    /// <summary>
    /// The color of the player's avatars.
    /// </summary>
    public Color Color;

    /// <summary>
    /// The input handler associated with this player.
    /// </summary>
    public readonly PlayerInputHandler InputHandler;

    /// <summary>
    /// Basic parameterized struct constructor. Does nothing special.
    /// </summary>
    public PlayerData(int score, Color color, PlayerInputHandler handler)
    {
        Score = score;
        Color = color;
        InputHandler = handler;
    }
}
