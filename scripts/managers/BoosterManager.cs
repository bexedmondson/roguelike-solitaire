
using System;
using Godot;

public class BoosterManager : IInjectable
{
    public event Action<AbstractBooster> OnBoosterPrimed;
    public event Action OnBoosterDeactivated;

    private InventoryManager inventoryManager;

    private AbstractBooster activeBooster;

    public BoosterManager()
    {
        InjectionManager.Register<BoosterManager>(this);
    }

    public void OnBoosterButtonToggled<T>(T booster, bool toggleState) where T : AbstractBooster
    {
        if (activeBooster != null && activeBooster.state == AbstractBooster.State.Activated)
        {
            GD.PushWarning("Cannot turn on or off a booster while one is activated! Current booster: " + activeBooster);
            return;
        }

        if (toggleState)
        {
            if (activeBooster != null)
                OnBoosterDeactivated?.Invoke();

            if (inventoryManager == null)
                inventoryManager = InjectionManager.Get<InventoryManager>();

            if (inventoryManager.GetBoosterCount<T>() <= 0)
            {
                GD.PushWarning("No inventory for booster that's trying to activate! Current booster: " + activeBooster);
                return;
            }

            activeBooster = booster;
            activeBooster.state = AbstractBooster.State.Primed;
            OnBoosterPrimed?.Invoke(booster);
        }
        else
        {
            activeBooster.state = AbstractBooster.State.Deactivated;
            OnBoosterDeactivated?.Invoke();
            activeBooster = null;
        }
    }

    public void ProcessBooster()
    {
        //GD.Print("booster process");
        if (activeBooster == null)
        {
            GD.PushError("trying to process booster but no booster active!");
            return;
        }
        
        activeBooster.OnBoosterEffectComplete += OnBoosterEffectComplete;
        activeBooster.DoBoosterEffect();
    }

    private void OnBoosterEffectComplete()
    {
        //TODO: maybe need to rename this so it's clear that nothing else should be able to activate until this booster is done
        //but the booster has been actually set off rather than deactivated
        OnBoosterDeactivated?.Invoke();
    }
}