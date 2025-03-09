using System.Threading.Tasks;
using Godot;

public class StateSolitaireGame : AbstractState
{
    protected override string Name => nameof(StateSolitaireGame);

    private Tableau tableau = null;

    protected override async Task DoStateTasksAsync()
    {
        var sceneTreeRoot = InjectionManager.Get<CurrentSceneAccessor>().CurrentSceneTree.Root;

        StateTicker stateTicker = sceneTreeRoot.GetNodeOrNull<StateTicker>("/root/GameRound/StateTicker");

        if (stateTicker == null)
        {
            GD.Print("ticker is null!");
            return;
        }

        stateTicker.OnTick += OnSceneTick;
    }

    private void OnSceneTick()
    {
        //make sure we have a reference to the current tableau as soon as it exists
        //or if it doesn't then let's just try the next frame
        if (tableau == null && !InjectionManager.Has<Tableau>())
            return;
        
        tableau ??= InjectionManager.Get<Tableau>();

        tableau.OnTableauComplete += EndState;
    }
}