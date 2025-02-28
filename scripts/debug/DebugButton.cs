
using System.ComponentModel.DataAnnotations;
using Godot;

public partial class DebugButton : Button
{
    private double timeSinceFirstClick = 0d;

    private int clickCount = 0;

    public override void _Process(double delta)
    {
        if (clickCount == 0)
            return;
        
        timeSinceFirstClick += delta;

        if (timeSinceFirstClick > 1)
        {
            clickCount = 0;
            timeSinceFirstClick = 0;
        }
    }

    public void OnClick()
    {
        clickCount++;

        if (clickCount >= 3)
        {
            timeSinceFirstClick = 0;
            clickCount = 0;
            ShowDebug();
        }
    }

    private void ShowDebug()
    {
        var tableau = InjectionManager.Get<Tableau>();
        tableau.LogTableau();
    }
}