using Godot;

public partial class CardDeckUI : CardStackUI
{
    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        return false;
    }

    public override void _GuiInput(InputEvent @event)
    {
        GD.Print("deck input");

        var temp = cards[^1];
        cards.RemoveAt(cards.Count - 1);
        cards.Insert(0, temp);

        UpdateCardVisuals();
    }
}