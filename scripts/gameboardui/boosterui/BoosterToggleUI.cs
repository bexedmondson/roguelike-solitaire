using System;
using Godot;

public partial class BoosterToggleUI : TextureButton
{
    [Export]
    private Label countLabel;

    private BoosterManager boosterManager;
    private InventoryManager inventoryManager;

    public override void _EnterTree()
    {
        boosterManager = InjectionManager.Get<BoosterManager>();
        inventoryManager = InjectionManager.Get<InventoryManager>();
        inventoryManager.OnBoosterCountChanged += OnBoosterCountChanged;
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

    private void OnBoosterCountChanged(Type type, int newCount)
    {
        if (type == typeof(BoosterShuffleStack))
        {
            countLabel.Text = newCount.ToString();
            this.Disabled = newCount == 0;
        }
    }
}