using Godot;

public partial class DiscardPileUI : CardStackUI
{
    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        return false;
    }

    public void AddCard(Card card)
    {
        cards.Insert(0, card);

        UpdateCardVisuals();
    }

    public Card[] GetDiscardPileAndClear()
    {
        var temp = cards.ToArray();
        
        cards.Clear();
        UpdateCardVisuals();

        return temp;
    }
}