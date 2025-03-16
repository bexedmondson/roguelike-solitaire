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

    public override bool CanDropSingleCard(Card dropCard)
    {
        if (dropCard.suit != suit)
            return false;

        if (IsEmpty)
            return dropCard.level == 1;

        return dropCard.level == cards.Count + 1;
    }

    public override bool CanDropCards(Array<Card> dropCards)
    {
        if (dropCards.Count != 1)
        {
            GD.PushError("Can only drop one card at a time on a foundation!");
            return false;
        }

        return CanDropSingleCard(dropCards[0]);
    }
}