using System;
using System.Collections.Generic;
using Godot;

public class Tableau
{
    public event Action OnTableauComplete;

    private int stackCount = 7;
    public List<Stack> stacks;

    public List<Foundation> foundations;

    public DrawPile drawPile;
    public DiscardPile discardPile;

    private ScoreManager scoreManager;

    public Tableau()
    {
        scoreManager = InjectionManager.Get<ScoreManager>();
        scoreManager.ResetScore();
        scoreManager.SetScoreTracking(false);

        stacks = new List<Stack>();
        foundations = new List<Foundation>();
        drawPile = new DrawPile();
        discardPile = new DiscardPile();

        var suits = Enum.GetValues(typeof(Suit));

        for (int i = 1; i <= 13; i++)
        {
            foreach (var suit in suits)
            {
                drawPile.Add(new Card(){suit = (Suit)suit, level = i, tableau = this});
                //GD.Print($"Added card {card.Name}");
            }
        }

        foreach (Suit suit in Enum.GetValues(typeof(Suit)))
        {
            var foundation = new Foundation(suit);
            foundations.Add(foundation);
            foundation.OnStackUpdated += CheckForCompletion;
        }
    }

    public void Deal()
    {
        drawPile.Shuffle();

        for (int i = 0; i < stackCount; i++)
        {
            List<Card> stackCards = drawPile.DealCards(i + 1);

            stacks.Add(new Stack(stackCards));
        }
    }

    public bool TryAutoMoveCard(Stack sourceStack, Card card)
    {
        if (sourceStack.CurrentEndCard != card)
            return false;

        foreach (Foundation foundation in foundations)
        {
            if (foundation.CanDropSingleCard(card))
            {
                MoveCards(sourceStack, foundation, card);
                return true;
            }
        }

        foreach (Stack stack in stacks)
        {
            if (stack.CanDropSingleCard(card))
            {
                MoveCards(sourceStack, stack, card);
                return true;
            }
        }

        return false;
    }

    public void MoveCards(Stack sourceStack, Stack targetStack, Godot.Collections.Array<Card> cards)
    {
        //GD.Print($"on move, source {sourceStack.Name}, target {dropTarget}");
        sourceStack.RemoveCards(cards);
        targetStack.AddCards(cards);
        scoreManager.OnCardMoved(sourceStack, targetStack);
    }

    public void MoveCards(Stack sourceStack, Stack targetStack, Card card)
    {
        //GD.Print($"on move, source {sourceStack.Name}, target {dropTarget}");
        sourceStack.RemoveCards(card);
        targetStack.AddCards(card);
        scoreManager.OnCardMoved(sourceStack, targetStack);
    }

    public void FlipDeck()
    {
        var cardsToFlip = discardPile.GetCardsAndClear();
        drawPile.AddCards(cardsToFlip);
    }

    public void CheckForCompletion()
    {
        if (!drawPile.IsEmpty)
            return;
        if (!discardPile.IsEmpty)
            return;
        foreach (Stack stack in stacks)
        {
            if (!stack.IsEmpty)
                return;
        }

        GD.Print("yay you did it!");
        OnTableauComplete?.Invoke();
    }

    public void LogTableau()
    {
        GD.Print("\n--- TABLEAU LOG ---\n");
        GD.Print("STACKS");

        for (int i = 0; i < stacks.Count; i++)
        {
            GD.Print($"Stack {i}");
            foreach (Card card in stacks[i].cards)
                GD.Print($"\t{card.suit}{card.level}");
        }

        GD.Print("\nFOUNDATIONS");
        for (int i = 0; i < foundations.Count; i++)
        {
            GD.Print($"Foundation {i}");
            foreach (Card card in foundations[i].cards)
                GD.Print($"\t{card.suit}{card.level}");
        }

        GD.Print($"\nDRAW PILE");
        foreach (Card card in drawPile.cards)
            GD.Print($"\t{card.suit}{card.level}");

        GD.Print($"\nDISCARD PILE");
        foreach (Card card in discardPile.cards)
            GD.Print($"\t{card.suit}{card.level}");
        
        GD.Print("\n--- TABLEAU LOG END ---\n");
    }
}