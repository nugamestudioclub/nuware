using System.Collections.Generic;

public interface IMicrogame
{
    /// <summary>
    /// Returns a MicrogameData structure with the information regarding this microgame.
    /// </summary>
    /// <returns>A microgame data structure.</returns>
    MicrogameData GetData();

    /// <summary>
    /// Returns a list of PlayerDatas to enumerate who won the game. This value will be set
    /// throughout the course of the game, but only returned in the MicrogameManager, who will
    /// handle the doling out of penalties and victories.
    /// </summary>
    /// <returns>A list of all players who "won" the microgame.</returns>
    IList<int> GetWinners();

    /// <summary>
    /// This function handles the pre-intialization of things you may need
    /// before starting the game. This includes stuff like starting a countdown
    /// timer, intializing scores, setting positions, etc.
    /// 
    /// Takes in a list of players of the game.
    /// </summary>
    void AwakeGame(IList<int> microgame_players);

    /// <summary>
    /// The basic signal to start your game.
    /// For example, this could be called after a countdown you start in AwakeGame, 
    /// "unlocking" your players, allowing them to move around and interact with your microgame scene.
    /// 
    /// Will be called AFTER AwakeGame()
    /// </summary>
    void StartGame();

    /// <summary>
    /// The basic signal to end the game.
    /// For example, once players complete the objective, this could be called to "lock" all
    /// the players and play a win SFX. This would hold until the game's timer terminates, after
    /// which the MicrogameManager would go about picking a new microgame.
    /// </summary>
    void EndGame();
}
