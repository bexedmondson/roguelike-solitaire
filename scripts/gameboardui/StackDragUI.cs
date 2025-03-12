using Godot;

public partial class StackDragUI : Control
{
    [Export]
    public Container container;

    public override void _EnterTree()
    {
        base._EnterTree();

        container.ChildEnteredTree += SetSeparation;
    }

    private void SetSeparation(Node node)
    {
        container.ChildEnteredTree -= SetSeparation;

        var childControl = node as CardUI;
        childControl.ItemRectChanged += () => OnCardReady(childControl);
    }

    private void OnCardReady(CardUI cardUI)
    {
        //GD.Print("child size in " + this.Name + " is " + cardUI.Size + ", " + cardUI.Name);
        double adjustedSize = -cardUI.Size.Y * 0.75;
        //GD.Print(Name + " separation to " + adjustedSize);
        int separation = Mathf.RoundToInt( adjustedSize );

        container.AddThemeConstantOverride("separation", separation);
    }
}