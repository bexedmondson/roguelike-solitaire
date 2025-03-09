using System.Threading.Tasks;

public class StateSolitaireGame : AbstractState
{
    protected override string Name => nameof(StateSolitaireGame);

    private Tableau tableau = null;

    protected override async Task DoStateTasksAsync()
    {
        var sceneTreeRoot = InjectionManager.Get<CurrentSceneAccessor>().CurrentSceneTree.Root;
        StateTicker sceneTicker = sceneTreeRoot.GetNode<StateTicker>("%SceneTicker");
        sceneTicker.OnTick += OnSceneTick;
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