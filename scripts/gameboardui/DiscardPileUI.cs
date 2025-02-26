using Godot;

public partial class DiscardPileUI : CardStackUI
{
    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        return false;
    }
}