using System;
using System.Collections.Generic;
using Godot;

public class Tableau : IInjectable
{
    private int stackCount = 7;
    public List<Stack> stacks;

    public DrawPile drawPile;
    public DiscardPile discardPile;

    public Tableau()
    {
        stacks = new List<Stack>();
        drawPile = new DrawPile();
        discardPile = new DiscardPile();

        for (int i = 1; i <= 13; i++)
        {
            drawPile.Add(new Card(){suit = Suit.Diamond, level = i});
            drawPile.Add(new Card(){suit = Suit.Heart, level = i});
            drawPile.Add(new Card(){suit = Suit.Club, level = i});
            drawPile.Add(new Card(){suit = Suit.Spade, level = i});
        }

        InjectionManager.Register<Tableau>(this);
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

    public void MoveCards(Stack sourceStack, Stack targetStack, Godot.Collections.Array<Card> cards)
    {
        //GD.Print($"on drop, source {sourceStack.Name}, target {dropTarget}");
        sourceStack.RemoveCards(cards);
        targetStack.AddCards(cards);
    }

    public void FlipDeck()
    {
        var cardsToFlip = discardPile.GetCardsAndClear();
        drawPile.AddCards(cardsToFlip);
    }
}