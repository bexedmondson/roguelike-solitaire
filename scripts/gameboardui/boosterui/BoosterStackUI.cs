
using Godot;

public partial class BoosterStackUI : Control
{
    [Export]
    private Control stackHighlight;

    [Export]
    private Button boosterButton;

    private Stack stack;
    private BoosterManager boosterManager;

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

    public void SetStack(Stack stack)
    {
        this.stack = stack;
    }

    private void OnBoosterPrimed()
    {
        GD.Print("on primed");
        stackHighlight.SetProcess(true);
        stackHighlight.Visible = true;

        boosterButton.SetProcessInput(true);
        boosterButton.Visible = true;
    }

    private void OnBoosterDeactivated()
    {
        GD.Print("on deactivate");
        stackHighlight.SetProcess(false);
        stackHighlight.Visible = false;

        boosterButton.SetProcessInput(false);
        boosterButton.Visible = false;
    }

    public void OnBoosterClick()
    {
        GD.Print("booster click " + Name);
        boosterManager.ProcessBooster(stack);
    }
}