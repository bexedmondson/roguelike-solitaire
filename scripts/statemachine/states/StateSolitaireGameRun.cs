using System.Threading.Tasks;
using Godot;

public class StateSolitaireGameRun : AbstractState
{
    protected override string Name => nameof(StateSolitaireGameRun);

    private Tableau tableau = null;
    private StateTicker stateTicker;

    protected override async Task DoStateTasksAsync()
    {
        //TODO move tableau initialisation to here or its own state, just definitely out of SolitaireGameUI
        var tableauManager = InjectionManager.Get<TableauManager>();
        tableau = tableauManager.CurrentTableau;
        tableau.OnTableauComplete += EndState;

        ScoreManager scoreManager = InjectionManager.Get<ScoreManager>();
        scoreManager.SetScoreTracking(true);
    }
}