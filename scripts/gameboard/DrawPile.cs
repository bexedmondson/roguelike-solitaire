using System.Collections.Generic;
using Godot;

public class DrawPile : Stack
{
    public DrawPile()
    {
        m_cards = new();
    }
    
    protected DrawPile(List<Card> cards) : base(cards)
    {
    }

    public void Add(Card card)
    {
        m_cards.Add(card);
    }

    public List<Card> DealCards(int count)
    {
        List<Card> dealtCards = new();
        for (int i = 0; i < count; i++)
        {
            Card card = CurrentEndCard;
            dealtCards.Add(card);
            m_cards.Remove(card);
        } 

        return dealtCards;
    }

    protected override void UpdateCardFlipStatus()
    {
        foreach (var card in m_cards)
        {
            if (card.flipped)
                card.flipped = false;
        }
    }
}