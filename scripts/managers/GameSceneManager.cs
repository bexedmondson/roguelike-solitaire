using System.Threading.Tasks;
using Godot;

public class GameSceneManager : IInjectable
{
    private Node activeGameSceneNode = null;

    public GameSceneManager()
    {
        InjectionManager.Register(this);
    }

    public void AddScene(PackedScene loadedScene)
    {
        if (activeGameSceneNode != null)
        {
            GD.PrintErr("game scene manager doesn't handle multiple game scene yet!!");
        }

        var gameSceneInstance = loadedScene.Instantiate();
        activeGameSceneNode = gameSceneInstance;
        
        var currentSceneAccessor = InjectionManager.Get<CurrentSceneAccessor>();
        currentSceneAccessor.ActiveScreenSceneNode = activeGameSceneNode;
        currentSceneAccessor.CurrentSceneTree.Root.AddChild(activeGameSceneNode);
    }

    public async Task RemoveActiveGameSceneNode()
    {
        if (activeGameSceneNode != null)
        {
            activeGameSceneNode.QueueFree();
            activeGameSceneNode = null;
        }

        var currentSceneAccessor = InjectionManager.Get<CurrentSceneAccessor>();

        await currentSceneAccessor.CurrentSceneTree.ToSignal(currentSceneAccessor.CurrentSceneTree, SceneTree.SignalName.NodeRemoved);
    }
}