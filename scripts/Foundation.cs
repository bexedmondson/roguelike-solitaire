using System.Collections.Generic;
using Godot;
using Godot.Collections;

public class Foundation : Stack
{
    public Suit suit;

    public Foundation(Suit suit)
    {
        m_cards = new();
        this.suit = suit;
    }

    protected Foundation(List<Card> cards) : base(cards)
    {

    }

    public override bool CanDropCards(Array<Card> dropCards)
    {
        if (dropCards.Count != 1)
        {
            GD.PrintErr("Can only drop one card at a time on a foundation!");
            return false;
        }

        var card = dropCards[0];
        if (card.suit != suit)

        if (IsEmpty)
            return card.level == 1;

        return card.level == cards.Count + 1;
    }
}