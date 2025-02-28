using System.Collections.Generic;
using Godot;

public class DiscardPile : Stack
{
    public DiscardPile()
    {
        m_cards = new();
    }

    protected DiscardPile(List<Card> cards) : base(cards)
    {
    }
    
    public void Add(Card card)
    {
        m_cards.Insert(0, card);

        InternalOnStackUpdated();
    }

    public Card[] GetCardsAndClear()
    {
        var temp = m_cards.ToArray();
        m_cards.Clear();

        InternalOnStackUpdated();

        return temp;
    }
}