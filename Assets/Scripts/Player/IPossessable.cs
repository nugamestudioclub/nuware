using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the interface the objects that your players "control" need to implement.
/// </summary>
public interface IPossessable
{
    /// <summary>
    /// Primarily used to customize a possessable's appearance to fit a player.
    /// This means changing the color of the item to match the player, swapping
    /// to a correct model, etc.
    /// </summary>
    void Initialize();

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
