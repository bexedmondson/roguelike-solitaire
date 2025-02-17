using Godot;

//autoloaded
public partial class CurrentSceneAccessor : Node, IInjectable
{
    public SceneTree CurrentSceneTree => GetTree();

    public CanvasLayer LoadingScreenLayer { get; set; }

    public Node ActiveScreenSceneNode { get; set; }

    public override void _Ready()
    {
        base._Ready();
        InjectionManager.Register(this);

        LoadingScreenLayer = GetNode<CanvasLayer>("/root/Shell/LoadingCanvasLayer");
    }
}