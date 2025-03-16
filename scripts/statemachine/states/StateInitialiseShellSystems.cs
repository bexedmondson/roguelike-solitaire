using System.Collections.Generic;
using System.Threading.Tasks;

public class StateInitialiseShellSystems : AbstractState
{
    protected override string Name => nameof(StateInitialiseShellSystems);

    protected override async Task DoStateTasksAsync()
    {
        ScoreManager scoreManager = new ScoreManager();
        TableauManager tableauManager = new TableauManager();
        GameSceneManager gameSceneManager = new GameSceneManager();
        BoosterManager boosterManager= new BoosterManager();
        MoveManager moveManager = new MoveManager();

        EndState();
    }
}