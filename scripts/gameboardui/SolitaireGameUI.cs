using Godot;

public partial class SolitaireGameUI : Control
{
    [Export]
    private Godot.Collections.Array<CardStackUI> stackUIs;

    [Export]
    private Godot.Collections.Array<FoundationUI> foundationUIs;

    [Export]
    private CardStackUI deck;

    private Tableau tableau;

    public override void _EnterTree()
    {
        tableau = new Tableau();
        tableau.Deal();

        for (int i = 0; i < tableau.stacks.Count; i++)
        {
            var stack = tableau.stacks[i];
            stackUIs[i].SetCards(stack);
        }

        foreach (FoundationUI foundationUI in foundationUIs)
        {
            foundationUI.OnFoundationUpdated += OnFoundationUpdated;
        }

        deck.SetCards(tableau.deck);
    }

    public void OnFoundationUpdated()
    {
        foreach (FoundationUI foundationUI in foundationUIs)
        {
            if (!foundationUI.IsReady())
                return;
        }
        
        GD.Print("yay you did it");
    }
}