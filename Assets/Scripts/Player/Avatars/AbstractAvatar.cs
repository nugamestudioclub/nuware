using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An abstract class for avatars. Provides empty definitions for events, and a setup for Initialize
/// that exposes a new abstract method for further initialization. 
/// 
/// Methods are documented in the interface.
/// </summary>
public abstract class AbstractAvatar : MonoBehaviour, IAvatar
{
    /// <summary>
    /// The associated party manager. Included for convenience, but probably won't be used for much
    /// beyond getting a player's data.
    /// </summary>
    protected PartyManager _partyManager { get; private set; }

    /// <summary>
    /// The bound player number to this avatar. Used so that you can access it's player data and know
    /// things about it for stuff. Yippee!
    /// </summary>
    protected int _boundPlayerNumber { get; private set; }

    /// <summary>
    /// The playerdata associated with the instance of this avatar.
    /// </summary>
    protected PlayerData _playerData { get; private set; }

    /// <summary>
    /// Assigns the given values to the fields of the avatar then calls a method to finish initialization.
    /// </summary>
    /// <param name="player_number"></param>
    /// <param name="manager"></param>
    public void Initialize(int player_number, PartyManager manager)
    {
        _partyManager = manager;
        _boundPlayerNumber = player_number;
        _playerData = manager.GetPlayerData(player_number);

        InitAvatar();
    }

    public virtual void OnButtonEvent(IDictionary<ButtonType, InputContextType> map)
    {
        // pass
    }

    public virtual void OnLateralEvent((Vector2 Data, InputContextType ContextType) data_tuple)
    {
        // pass
    }

    public virtual void DestroyAvatar()
    {
        // pass
    }

    /// <summary>
    /// For further initialization of the avatar post assigning values to the party manager and bound player number.
    /// E.g. setting the color of the avatar.
    /// </summary>
    protected abstract void InitAvatar();
}
