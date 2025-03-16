using System.Threading.Tasks;
using Godot;

public class GameSceneManager : IInjectable
{
    private Node activeGameSceneNode = null;

    public GameSceneManager()
    {
        InjectionManager.Register(this);
    }

    public void InstantiateScene(PackedScene loadedScene)
    {
        if (activeGameSceneNode != null)
        {
            GD.PushError("game scene manager doesn't handle multiple game scene yet!!");
        }

        var gameSceneInstance = loadedScene.Instantiate();
        activeGameSceneNode = gameSceneInstance;

        activeGameSceneNode.SetProcess(false);
    }

    public void AddGameSceneNodeToTree()
    {   
        var currentSceneAccessor = InjectionManager.Get<CurrentSceneAccessor>();
        currentSceneAccessor.ActiveScreenSceneNode = activeGameSceneNode;
        currentSceneAccessor.CurrentSceneTree.Root.AddChild(activeGameSceneNode);

        activeGameSceneNode.SetProcess(true);
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