using Godot;

public partial class SolitaireGameUI : Control
{
    [Export]
    private Godot.Collections.Array<CardStackUI> stackUIs;

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
    }
}