using System.Collections.Generic;
using Godot;

public class DiscardPile : Stack
{
    public void Add(Card card)
    {
        m_cards.Add(card);
    }

}