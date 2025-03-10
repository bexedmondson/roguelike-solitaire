using System.Threading.Tasks;
using Godot;

public class StateSolitaireGameSetup : AbstractState
{
    protected override string Name => nameof(StateSolitaireGameSetup);

    protected override async Task DoStateTasksAsync()
    {
        var scoreManager = InjectionManager.Get<ScoreManager>();
        scoreManager.ResetScore();
        scoreManager.SetScoreTracking(false); //so that it doesn't track the initial deal of the cards and associated card flipping

        var tableauManager = InjectionManager.Get<TableauManager>();
        var tableau = tableauManager.NewTableau();
        tableau.Deal();

        GameSceneManager gameSceneManager = InjectionManager.Get<GameSceneManager>();
        gameSceneManager.AddGameSceneNodeToTree();

        EndState();
    }
}