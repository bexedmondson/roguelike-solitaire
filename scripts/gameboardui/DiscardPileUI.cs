using Godot;

public partial class DiscardPileUI : CardStackUI
{
    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        return false;
    }

    public void AddCard(Card card)
    {
        stack.cards.Insert(0, card);

        UpdateCardVisuals();
    }

    public Card[] GetDiscardPileAndClear()
    {
        var temp = stack.cards.ToArray();
        //TODO: update this to do the logic not in the ui class
        stack.cards.Clear();
        UpdateCardVisuals();

        return temp;
    }
}