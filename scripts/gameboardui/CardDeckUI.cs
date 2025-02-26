using Godot;

public partial class CardDeckUI : CardStackUI
{
    [Export]
    private DiscardPileUI discardPileUI;

    public override bool _CanDropData(Vector2 atPosition, Variant data)
    {
        return false;
    }

    public override CardDrag GetDragData(CardUI selectedCard)
    {
        return default(CardDrag);
    }

    public void OnClick()
    {
        //GD.Print("deck input");
        if (stack.IsEmpty)
        {
            FlipDeck();
            return;
        }
        
        GD.Print("deck before:");

        foreach (Card card in stack.cards)
        {
            GD.Print($"  {card.suit.Name()}{card.level}");
        }

        var temp = stack.cards[^1];
        stack.cards.RemoveAt(stack.cards.Count - 1);
        discardPileUI.AddCard(temp);

        UpdateCardVisuals();

        GD.Print("deck after:");

        foreach (Card card in stack.cards)
        {
            GD.Print($"  {card.suit.Name()}{card.level}");
        }
    }

    private void FlipDeck()
    {
        var discardPile = discardPileUI.GetDiscardPileAndClear();
        stack.cards.AddRange(discardPile);
        UpdateCardVisuals();
    }

    protected  void UpdateCardFlipStatus()
    {
        foreach (var card in stack.cards)
        {
            if (card.flipped)
                card.flipped = false;
        }
    }
}