
using System;
using Godot;

public partial class BoosterStackUI : Control
{
    [Export]
    private Control stackHighlight;

    [Export]
    private Button boosterButton;

    private Stack stack;
    private CardStackUI stackUI;
    private BoosterManager boosterManager;
    private BoosterShuffleStack activeBooster;

    public override void _EnterTree()
    {
        boosterManager = InjectionManager.Get<BoosterManager>();
        boosterManager.OnBoosterPrimed += OnBoosterPrimed;
        boosterManager.OnBoosterDeactivated += OnBoosterDeactivated;
    }

    public override void _ExitTree()
    {
        boosterManager.OnBoosterPrimed -= OnBoosterPrimed;
        boosterManager.OnBoosterDeactivated -= OnBoosterDeactivated;
    }

    public void SetStack(Stack stack, CardStackUI stackUI)
    {
        this.stack = stack;
        this.stackUI = stackUI;
    }

    private void OnBoosterPrimed(AbstractBooster booster)
    {
        if (booster is not BoosterShuffleStack boosterShuffleStack)
            return;

        activeBooster = boosterShuffleStack;

        //GD.Print("on primed");
        stackHighlight.SetProcess(true);
        stackHighlight.Visible = true;

        boosterButton.SetProcessInput(true);
        boosterButton.Visible = true;
    }

    public void OnBoosterClick()
    {
        //GD.Print("booster click " + Name);
        BoosterShuffleStackData boosterData = new BoosterShuffleStackData();
        boosterData.stack = stack;
        boosterData.cardStackUI = stackUI;

        activeBooster.SetupBoosterData(boosterData);

        boosterManager.ProcessBooster();
    }

    private void OnBoosterDeactivated()
    {
        //GD.Print("on deactivate");
        activeBooster = null;

        stackHighlight.SetProcess(false);
        stackHighlight.Visible = false;

        boosterButton.SetProcessInput(false);
        boosterButton.Visible = false;
    }
}