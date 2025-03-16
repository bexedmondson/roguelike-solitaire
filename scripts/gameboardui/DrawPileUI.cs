using Godot;

public partial class DrawPileUI : CardStackUI
{
    private Tableau tableau;

    public void OnClick()
    {
        if (tableau == null)
            tableau = InjectionManager.Get<TableauManager>().CurrentTableau;

        foreach (var child in GetChildren())
        {
            if (!child.IsNodeReady())
                return;
        }

        //GD.Print("deck input");
        if (stack.IsEmpty)
        {
            tableau.FlipDeck();
            return;
        }
        
        /*GD.Print("deck before:");

        foreach (Card card in stack.cards)
        {
            GD.Print($"  {card.Name}");
        }*/

        tableau.MoveCards(stack, tableau.discardPile, new Godot.Collections.Array<Card>(){stack.cards[^1]});

        /*GD.Print("deck after:");

        foreach (Card card in stack.cards)
        {
            GD.Print($"  {card.Name}");
        }*/
    }
}