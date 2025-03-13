using System;
using System.Collections.Generic;
using Godot;

public class Stack
{
    protected List<Card> m_cards;
    public List<Card> cards => m_cards;//.ToArray();

    public event Action OnStackUpdated;

    public int Count => m_cards != null ? m_cards.Count : 0;

    public bool IsEmpty => Count == 0;

    public Card CurrentEndCard => IsEmpty ? null : m_cards[0];

    private ScoreManager scoreManager;

    protected Stack() { }

    public Stack(List<Card> initialCards)
    {
        m_cards = initialCards;
        scoreManager = InjectionManager.Get<ScoreManager>();
        InternalOnStackUpdated();
    }

    public void AddCards(IEnumerable<Card> droppedCards)
    {
        m_cards.InsertRange(0, droppedCards);

        InternalOnStackUpdated();
    }
    
    public void AddCards(Card droppedCard)
    {
        m_cards.Insert(0, droppedCard);

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

    public void RemoveCards(Card removedCard)
    {
        cards.Remove(removedCard);
        
        InternalOnStackUpdated();
    }

    protected void InternalOnStackUpdated()
    {
        UpdateCardFlipStatus();
        OnStackUpdated?.Invoke();
    }

    protected virtual void UpdateCardFlipStatus()
    {
        if (IsEmpty)
            return;

        if (!CurrentEndCard.flipped)
        {
            CurrentEndCard.flipped = true;
            scoreManager?.OnCardFlipped(this);
        }
    }

    public virtual bool CanDropSingleCard(Card dropCard)
    {
        if (IsEmpty)
            return dropCard.level == 13;
        //GD.Print($"current drop card {dropCard.suit} {dropCard.level}");
        //GD.Print($"current stack end card {CurrentEndCard.suit} {CurrentEndCard.level}");
        
        return dropCard.suit.CanStackOnSuit(CurrentEndCard.suit) && dropCard.level == CurrentEndCard.level - 1;
    }

    public virtual bool CanDropCards(Godot.Collections.Array<Card> dropCards)
    {
        if (dropCards.Count == 0)
        {
            GD.PushWarning("Why are you trying to drop no cards on a stack?");
            return false;
        }
   
        return CanDropSingleCard(dropCards[^1]);
    }

    public List<Card> GetStackSectionFromSelectedCard(Card selectedCard)
    {
        //GD.Print($"selectedcard {selectedCard.suit}{selectedCard.level}");
        //GD.Print($"index {cards.IndexOf(selectedCard)}, lastindex {cards.Count - 1}");
        int selectedCardIndex = m_cards.IndexOf(selectedCard);
        var dragCards = m_cards.GetRange(0, selectedCardIndex+1);//, cards.Count - selectedCardIndex);

        //GD.Print($"drag count {dragCards.Count} {dragCards[0].suit}{dragCards[0].level}");
        //if (dragCards.Count > 1)
            //GD.Print($"  {dragCards[1].suit}{dragCards[1].level}");

        return dragCards;
    }

    public void Cleanup()
    {
        OnStackUpdated = null;
    }
}