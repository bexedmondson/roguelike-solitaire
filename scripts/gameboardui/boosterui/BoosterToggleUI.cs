using Godot;

public partial class BoosterToggleUI : Button
{
    private BoosterManager boosterManager;

    public override void _EnterTree()
    {
        boosterManager = InjectionManager.Get<BoosterManager>();
    }

    public override void _Toggled(bool toggledOn)
    {
        boosterManager.OnBoosterButtonToggled(toggledOn);
        boosterManager.OnBoosterDeactivated += OnBoosterDeactivated;
    }

    private void OnBoosterDeactivated()
    {
        boosterManager.OnBoosterDeactivated -= OnBoosterDeactivated;
        this.SetPressedNoSignal(false);
    }
}