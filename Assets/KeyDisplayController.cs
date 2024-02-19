using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDisplayController : MonoBehaviour
{
    public enum Player
    {
        ONE, TWO, THREE, FOUR
    }

    [SerializeField]
    private int BUTTON_FONT_SIZE_NORMAL = 150;
    [SerializeField]
    private int BUTTON_FONT_SIZE_PRESSED = 200;
    [SerializeField]
    private int CORRECT_PRESS_POINTS = 1;
    [SerializeField]
    private int WRONG_PRESS_PENALTY = 2;
    [SerializeField]
    private int STARTING_SCORE = 30;

    private Dictionary<Player, int> scores;

    private GameObject key;
    private GameObject scoreDisplay;
    private List<string> buttons = new List<string> { "A", "B", "X", "Y" };
    private string selectedButton;
    private int totalPressCount = 0;
    private int pressLimit;

    // Start is called before the first frame update
    void Start()
    {
        ResetAll();
    }

    // resets the game to starting conditions
    public void ResetAll()
    {
        scores = new Dictionary<Player, int> { { Player.ONE, STARTING_SCORE }, { Player.TWO, STARTING_SCORE },
            { Player.THREE, STARTING_SCORE }, { Player.FOUR, STARTING_SCORE } };

        key = transform.GetChild(0).gameObject;
        scoreDisplay = transform.GetChild(1).gameObject;

        key.GetComponent<TMPro.TMP_Text>().alignment = TMPro.TextAlignmentOptions.Center;
        selectedButton = buttons[Random.Range(0, buttons.Count)];
        pressLimit = Random.Range(10, 15);

        SetButtonDisplaySize(BUTTON_FONT_SIZE_NORMAL);
        RefreshButtonDisplay();
        UpdateScoreDisplay();
    }

    void Update()
    {
        SetButtonDisplaySize(GetButtonDisplaySize() + (BUTTON_FONT_SIZE_NORMAL - GetButtonDisplaySize()) / 10);

        // this is temporary for testing purposes
        /*if (Input.GetKeyDown(KeyCode.A))
        {
            OnButtonPress(Player.ONE, "A");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            OnButtonPress(Player.ONE, "B");
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            OnButtonPress(Player.ONE, "X");
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            OnButtonPress(Player.ONE, "Y");
        }*/

        if (Input.GetKeyDown(KeyCode.A))
        {
            OnButtonPress(Player.ONE, "A");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            OnButtonPress(Player.ONE, "B");
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            OnButtonPress(Player.ONE, "X");
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            OnButtonPress(Player.ONE, "Y");
        }
    }

    // when the controller receives a button input from a player
    public void OnButtonPress(Player player, string button)
    {
        // scoring
        scores[player] += button.Equals(selectedButton) ? -CORRECT_PRESS_POINTS : WRONG_PRESS_PENALTY;

        // button randomizing
        if (totalPressCount < pressLimit)
        {
            totalPressCount += 1;
        } else
        {
            totalPressCount = 0;
            pressLimit = Random.Range(10, 15);
            RandomizeButton();
            RefreshButtonDisplay();
        }
        UpdateScoreDisplay();
        SetButtonDisplaySize(BUTTON_FONT_SIZE_PRESSED);
    }

    // sets the displayed button to the currently selected one
    private void RefreshButtonDisplay()
    {
        SetButtonDisplay(selectedButton);
    }

    // sets the displayed button to the given button string
    private void SetButtonDisplay(string button)
    {
        key.GetComponent<TMPro.TMP_Text>().SetText(selectedButton);
    }

    // gets the size of the button display as a positive integer
    private int GetButtonDisplaySize()
    {
        return (int) key.GetComponent<TMPro.TMP_Text>().fontSize;
    }

    // sets the size of the button display
    private void SetButtonDisplaySize(int size)
    {
        key.GetComponent<TMPro.TMP_Text>().fontSize = size;
    }

    // randomizes the currently selected button (but does not update the button display, which will
    // continue to store the previous button until its display is refreshed)
    private void RandomizeButton()
    {
        string newButton;
        do
        {
            newButton = buttons[Random.Range(0, buttons.Count)];
        } while (newButton == selectedButton);
        selectedButton = newButton;
    }

    private void UpdateScoreDisplay()
    {
        string scoreString = "";
        foreach (Player player in new List<Player> { Player.ONE, Player.TWO, Player.THREE, Player.FOUR })
        {
            scoreString += scores[player].ToString() + "\n";
        }
        scoreDisplay.GetComponent<TMPro.TMP_Text>().SetText(scoreString.Trim());
    }
}