using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : AMicrogame
{
    [SerializeField]
    private float GAME_LENGTH_SECONDS = 7;
    [SerializeField]
    private KeyDisplayController KEY_DISPLAY_CONTROLLER;

    public override MicrogameData GetData()
    {
        return new MicrogameData("Game Where You Have to Spam Buttons Really Fast on Your Controller",
            "The title is self-explanatory you idiot",
            GameType.CompetitiveFFA);
    }

    public override float AwakeGame(IList<int> microgame_players, DifficultyType difficulty)
    {
        float f = base.AwakeGame(microgame_players, difficulty);
        StartGame();
        return f;
    }

    public override void StartGame()
    {
        KEY_DISPLAY_CONTROLLER.ResetAll();
    }

    protected override float CalculateGameDuration(DifficultyType difficulty)
    {
        return GAME_LENGTH_SECONDS;
    }

    // Start is called before the first frame update
    void Start()
    {
        AwakeGame(null, DifficultyType.Medium);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
