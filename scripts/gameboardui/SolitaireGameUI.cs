using Godot;

public partial class SolitaireGameUI : Control
{
    [Export]
    private Godot.Collections.Array<CardStackUI> stackUIs;

    [Export]
    private Godot.Collections.Array<FoundationUI> foundationUIs;

    [Export]
    private CardStackUI drawPileUI;

    [Export]
    private CardStackUI discardPileUI;

    private Tableau tableau;

    public override void _EnterTree()
    {
        var tableauManager = InjectionManager.Get<TableauManager>();
        tableau = tableauManager.CurrentTableau;

        for (int i = 0; i < tableau.stacks.Count; i++)
        {
            var stack = tableau.stacks[i];
            stackUIs[i].InitialiseWithStack(stack);
        }

        for (int i = 0; i < foundationUIs.Count; i++)
        {
            Foundation foundation = tableau.foundations[i];
            foundationUIs[i].InitialiseWithStack(foundation);
        }

        drawPileUI.InitialiseWithStack(tableau.drawPile);

        discardPileUI.InitialiseWithStack(tableau.discardPile);
    }
}