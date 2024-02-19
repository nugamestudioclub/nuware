using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputScript : IPossessable
{
    [SerializeField]
    private KeyDisplayController KEY_DISPLAY_CONTROLLER;

    IDictionary<ButtonType, string> buttonsAsStrings = new Dictionary<ButtonType, string>
    { 
        { ButtonType.North, "Y" }, 
        { ButtonType.South, "A" },
        { ButtonType.East, "B" },
        { ButtonType.West, "X" }
    };

    IDictionary<int, KeyDisplayController.Player> playersAsPlayers = new Dictionary<int, KeyDisplayController.Player>
    {
        { 1, KeyDisplayController.Player.ONE },
        { 2, KeyDisplayController.Player.TWO },
        { 3, KeyDisplayController.Player.THREE },
        { 4, KeyDisplayController.Player.FOUR },
    };

    KeyDisplayController.Player player;


    public void Initialize(int player)
    {
        this.player = playersAsPlayers[player];
    }

    public void OnButtonEvent(IDictionary<ButtonType, InputContextType> map)
    {
        foreach (ButtonType button in map.Keys)
        {
            if (map[button].Equals(InputContextType.Performed))
            {
                KEY_DISPLAY_CONTROLLER.OnButtonPress(player, buttonsAsStrings[button]);
            }
        }
    }

    public void OnLateralEvent((Vector2 Data, InputContextType ContextType) data_tuple)
    {
        // do nothing
    }
}
