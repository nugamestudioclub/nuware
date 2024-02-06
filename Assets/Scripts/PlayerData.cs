/// <summary>
/// Currently a very empty structure that just holds the ID of a player.
/// Maybe have more things added in the future, but currently represents 
/// just a player.
/// </summary>
[System.Serializable]
public struct PlayerData
{
    public readonly int ID;

    public PlayerData(int id)
    {
        ID = id;
    }

    public override readonly bool Equals(object obj)
    {
        if (obj is PlayerData data)
        {
            return ID == data.ID;
        }

        return false;
    }

    public override readonly int GetHashCode() 
    {
        return ID;
    }
}
