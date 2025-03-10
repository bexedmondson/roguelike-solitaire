using System.Threading.Tasks;
using Godot;

public class StateSolitaireGame : AbstractState
{
    protected override string Name => nameof(StateSolitaireGame);

    private Tableau tableau = null;
    private StateTicker stateTicker;

    protected override async Task DoStateTasksAsync()
    {
        //TODO move tableau initialisation to here or its own state, just definitely out of SolitaireGameUI
        var tableauManager = InjectionManager.Get<TableauManager>();
        tableau = tableauManager.CurrentTableau;
        if (tableau != null)
        {
            tableau.OnTableauComplete += EndState;
        }
        else
        {
            var sceneTreeRoot = InjectionManager.Get<CurrentSceneAccessor>().CurrentSceneTree.Root;

            stateTicker = sceneTreeRoot.GetNodeOrNull<StateTicker>("/root/GameRound/StateTicker");

            if (stateTicker == null)
            {
                GD.Print("ticker is null!");
                return;
            }

            stateTicker.OnTick += OnSceneTick;
        }
    }

    private void OnSceneTick()
    {
        //make sure we have a reference to the current tableau as soon as it exists
        //or if it doesn't then let's just try the next frame
        tableau ??= InjectionManager.Get<TableauManager>().CurrentTableau;
        if (tableau == null)
            return;

        tableau.OnTableauComplete += EndState;
        stateTicker.OnTick -= OnSceneTick;
    }
}