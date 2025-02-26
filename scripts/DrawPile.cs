using System.Collections.Generic;
using Godot;

public class DrawPile : Stack
{
    public void Add(Card card)
    {
        m_cards.Add(card);
    }

    public void Shuffle()
    {
        var rng = new RandomNumberGenerator();

        int n = Count;  
        while (n > 1) 
        {  
            n--;  
            int k = rng.RandiRange(0, n + 1);  
            Card temp = m_cards[k];  
            m_cards[k] = m_cards[n];  
            m_cards[n] = temp;  
        }
    }

    public List<Card> DealCards(int count)
    {
        List<Card> dealtCards = new();
        for (int i = 0; i < count; i++)
        {
            Card card = m_cards[0];
            dealtCards.Add(card);
            m_cards.Remove(card);
        } 

        return dealtCards;
    }
}