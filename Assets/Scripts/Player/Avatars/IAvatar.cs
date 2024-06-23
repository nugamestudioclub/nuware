using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the interface the objects that your players "control" need to implement.
/// </summary>
public interface IAvatar
{
    /// <summary>
    /// Primarily used to customize a avatar's appearance to fit a player.
    /// This means changing the color of the item to match the player, swapping
    /// to a correct model, etc. Note that this method is meant to be called from PartyManager's
    /// binding method. If you *need* to call this method way before binding you can, but don't forget
    /// to modify the 2nd argument of the PartyManager's bind method so that the avatar doesn't
    /// get initialized twice.
    /// 
    /// Takes in the player id associated with the avatar, as well as the party manager
    /// that contains info about the player.
    /// </summary>
    void Initialize(int player_number, PartyManager manager);

    /// <summary>
    /// To be used when the Avatar is to be destroyed (i.e. if they die early in a 
    /// microgame or if the player leaves the game early). Note that unbinding a player from
    /// their avatar through PartyManager has a parameter to call this method. That is the
    /// preferred way to do this.
    /// 
    /// Used to handle death effects, like playing an animation or something like that.
    /// </summary>
    void DestroyAvatar();

    /// <summary>
    /// For one directional input, this is called 3 times:
    /// Once for when the input starts, once for when the input is fully performed, and once
    /// for when the input is released.
    /// 
    /// Note that on control sticks, this will get fired A LOT. This is because every possible
    /// minute directional change triggers a new invocation. You might just want to have a field
    /// in your script that saves the most recent input value and uses that.
    /// </summary>
    /// <param name="data_tuple"></param>
    void OnLateralEvent((Vector2 Data, InputContextType ContextType) data_tuple);

    /// <summary>
    /// Much like the above, fires three times upon button press:
    /// Once for start, once for performed (identical to start; invoked immediately after it), and once for 
    /// canceled (when button is lifted). Performed is the one you will most often be using.
    /// </summary>
    /// <param name="map"></param>
    void OnButtonEvent(IDictionary<ButtonType, InputContextType> map);
}
