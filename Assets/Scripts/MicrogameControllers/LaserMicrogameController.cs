using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMicrogameController : AMicrogame
{
    public static bool gameStarted = false;

    private void Awake()
    {
        AwakeGame(new List<int>(), DifficultyType.VeryEasy);
    }

    public override MicrogameData GetData()
    {
        throw new System.NotImplementedException();
    }

    public override float AwakeGame(IList<int> microgame_players, DifficultyType difficulty)
    {
        float f = base.AwakeGame(microgame_players, difficulty);

        StartGame();
        return f;
    }

    public override void StartGame()
    {
        Debug.Log("Game has started");
        gameStarted = true;

    }

    protected override float CalculateGameDuration(DifficultyType difficulty)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
