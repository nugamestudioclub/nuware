/// <summary>
/// A data container for information about a Microgame.
/// </summary>
[System.Serializable]
public struct MicrogameData
{
    public readonly string Name;
    public readonly string HintText;

    // TODO: Add a list-of "control-type hint" field. That way, players know what the control-scheme
    // of the microgame their heading into is.

    public readonly GameType GameType;
    public readonly int NameHash;
    
    public MicrogameData(string name, string hint_Text, GameType game_type)
    {
        Name = name;
        HintText = hint_Text; 
        GameType = game_type;

        NameHash = name.GetHashCode();
    }
}
