using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;

public class StateDisableLoadingScreen : AbstractState
{
    protected override string Name => nameof(StateDisableLoadingScreen);

    protected override async Task DoStateTasksAsync()
    {
        var loadingScreenLayer = InjectionManager.Get<CurrentSceneAccessor>().LoadingScreenLayer;

        loadingScreenLayer.Visible = false;
        loadingScreenLayer.SetProcess(false);
    }
}