using System.Threading.Tasks;
using Godot;

public class StateSolitaireGameTeardown : AbstractState
{
    protected override string Name => nameof(StateSolitaireGameTeardown);

    protected override async Task DoStateTasksAsync()
    {
        var gameSceneManager = InjectionManager.Get<GameSceneManager>();
        await gameSceneManager.RemoveActiveGameSceneNode();

        var scoreManager = InjectionManager.Get<ScoreManager>();
        scoreManager.ResetScore();
        scoreManager.SetScoreTracking(false);

        EndState();
    }
}