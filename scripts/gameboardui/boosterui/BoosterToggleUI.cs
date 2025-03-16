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
        //TODO figure out nice way to put different boosters in here. maybe make this abstract and inherit? hmm
        boosterManager.OnBoosterButtonToggled(new BoosterShuffleStack(), toggledOn);
        boosterManager.OnBoosterDeactivated += OnBoosterDeactivated;
    }

    private void OnBoosterDeactivated()
    {
        boosterManager.OnBoosterDeactivated -= OnBoosterDeactivated;
        this.SetPressedNoSignal(false);
    }
}