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
        tableau = new Tableau();
        tableau.Deal();

        for (int i = 0; i < tableau.stacks.Count; i++)
        {
            var stack = tableau.stacks[i];
            stackUIs[i].InitialiseWithStack(stack);
        }

        foreach (FoundationUI foundationUI in foundationUIs)
        {
            foundationUI.OnFoundationUpdated += OnFoundationUpdated;
        }

        drawPileUI.InitialiseWithStack(tableau.drawPile);

        discardPileUI.InitialiseWithStack(tableau.discardPile);
    }

    public void OnFoundationUpdated()
    {
        GD.Print("on foundation updated");
        foreach (FoundationUI foundationUI in foundationUIs)
        {
            if (!foundationUI.IsReady())
                return;
        }
        
        GD.Print("yay you did it");
    }
}