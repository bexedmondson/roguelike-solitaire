
using System;
using System.Collections.Generic;
using Godot;

public partial class CardDrag : GodotObject
{
    public Stack sourceStack;

    public Godot.Collections.Array<Card> cards = new();

    public void InitialiseCardDrag(Stack sourceStack, List<Card> cards)
    {
        this.sourceStack = sourceStack;
        this.cards = new Godot.Collections.Array<Card>(cards);
    }

    public void DoCardDrop(Stack targetStack)
    {
        var tableau = InjectionManager.Get<Tableau>();
        tableau.MoveCards(sourceStack, targetStack, cards);
    }
}