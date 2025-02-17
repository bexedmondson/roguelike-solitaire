
using System;
using Godot;

public partial class CardDrag : GodotObject
{
    public CardStackUI sourceStack;

    public Godot.Collections.Array<Card> cards;

    public void OnDrop(object dropTarget)
    {
        GD.Print($"on drop, source {sourceStack.Name}, target {dropTarget}");

        if (dropTarget is CardStackUI targetStack)
        {
            if (targetStack == sourceStack)
            {
                GD.Print("target and source the same");
                return;
            }

            sourceStack.RemoveCards(cards);
        }
        else
        {
            GD.Print("target not a card stack");
        }
    }

}