using System.Threading.Tasks;
using Godot;

public class StateLoadHomeScene : AbstractState
{
    protected override string Name => nameof(StateLoadHomeScene);

    private readonly string m_mainScenePath = "res://scenes/menu.tscn";

    protected override async Task DoStateTasksAsync()
    {
        ResourceLoader.LoadThreadedRequest(m_mainScenePath, cacheMode: ResourceLoader.CacheMode.Ignore);

        Godot.Collections.Array progress = new Godot.Collections.Array();
        while (ResourceLoader.LoadThreadedGetStatus(m_mainScenePath, progress) == ResourceLoader.ThreadLoadStatus.InProgress)
        {
            GD.Print($"StateLoadGameScene: awaiting scene load, {progress[0]}");
            //have to do this because ResourceLoader doesn't have anything awaitable
            await Task.Delay(25);
        }

        var mainScene = (PackedScene)ResourceLoader.LoadThreadedGet(m_mainScenePath);
        var mainSceneInstance = mainScene.Instantiate();
        
        var currentSceneAccessor = InjectionManager.Get<CurrentSceneAccessor>();
        currentSceneAccessor.ActiveScreenSceneNode = mainSceneInstance;
        currentSceneAccessor.CurrentSceneTree.Root.AddChild(mainSceneInstance);
    }
}