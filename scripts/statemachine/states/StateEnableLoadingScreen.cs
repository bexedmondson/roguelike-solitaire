using System.Threading.Tasks;

public class StateEnableLoadingScreen : AbstractState
{
    protected override string Name => nameof(StateEnableLoadingScreen);

    protected override async Task DoStateTasksAsync()
    {
        var loadingScreenLayer = InjectionManager.Get<CurrentSceneAccessor>().LoadingScreenLayer;

        loadingScreenLayer.Visible = true;
        loadingScreenLayer.SetProcess(true);
        EndState();
    }
}