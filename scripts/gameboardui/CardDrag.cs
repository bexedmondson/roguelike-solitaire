
using System;
using Godot;

public partial class CardDrag : GodotObject
{
    public CardStackUI sourceStack;

    public Godot.Collections.Array<Card> cards = new();

    public void OnDrop(object dropTarget)
    {
        //GD.Print($"on drop, source {sourceStack.Name}, target {dropTarget}");

        sourceStack.RemoveCards(cards);
    }
}