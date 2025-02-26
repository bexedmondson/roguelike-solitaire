using System;
using System.Collections.Generic;
using Godot;

public class Stack
{
    protected List<Card> m_cards;
    public List<Card> cards => m_cards;//.ToArray();

    public event Action OnStackUpdated;

    public int Count => m_cards.Count;

    public bool IsEmpty => Count == 0;

    protected Stack() { }

    public Stack(List<Card> initialCards)
    {
        m_cards = initialCards;
        InternalOnStackUpdated();
    }

    public void AddCards(IEnumerable<Card> droppedCards)
    {
        m_cards.InsertRange(0, droppedCards);

        InternalOnStackUpdated();
    }

    public void RemoveCards(Godot.Collections.Array<Card> removedCards)
    {
        foreach (Card card in removedCards)
        {
            m_cards.Remove(card);
        }
        
        InternalOnStackUpdated();
    }

    protected void InternalOnStackUpdated()
    {
        UpdateCardFlipStatus();
        OnStackUpdated?.Invoke();
    }

    protected virtual void UpdateCardFlipStatus()
    {
        if (!cards[0].flipped)
            cards[0].flipped = true;
    }

    public bool CanDropCards(Godot.Collections.Array<Card> dropCards)
    {
        if (dropCards.Count == 0)
        {
            GD.PushWarning("Why are you trying to drop no cards on a stack?");
            return false;
        }

        if (IsEmpty)
            return dropCards[^1].level == 13;
        
        Card currentEndCard = cards[0]; GD.Print($"current stack end card {currentEndCard.suit} {currentEndCard.level}");
        var topDropCard = dropCards[^1]; GD.Print($"current drop card {topDropCard.suit} {topDropCard.level}");
        
        return topDropCard.suit.CanStackOnSuit(currentEndCard.suit) && topDropCard.level == currentEndCard.level - 1;
    }

    public List<Card> GetStackSectionFromSelectedCard(Card selectedCard)
    {
        //GD.Print($"selectedcard {selectedCard.card.suit}{selectedCard.card.level}");
        //GD.Print($"index {cards.IndexOf(selectedCard.card)}, lastindex {cards.Count - 1}");
        int selectedCardIndex = m_cards.IndexOf(selectedCard);
        var dragCards = m_cards.GetRange(0, selectedCardIndex+1);//, cards.Count - selectedCardIndex);

        //GD.Print($"drag count {dragCards.Count} {dragCards[0].suit}{dragCards[0].level}");
        //if (dragCards.Count > 1)
            //GD.Print($"  {dragCards[1].suit}{dragCards[1].level}");

        return dragCards;
    }
}