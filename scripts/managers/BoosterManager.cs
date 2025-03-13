
using System;
using Godot;

public class BoosterManager : IInjectable
{
    public event Action OnBoosterPrimed;
    public event Action OnBoosterDeactivated;

    public BoosterManager()
    {
        InjectionManager.Register<BoosterManager>(this);
    }

    public void OnBoosterButtonToggled(bool toggleState)
    {
        if (toggleState)
            OnBoosterPrimed?.Invoke();
        else
            OnBoosterDeactivated?.Invoke();
    }

    public void ProcessBooster(Stack stack)
    {
        GD.Print("booster process");

        //TODO: maybe need to rename this so it's clear that nothing else should be able to activate until this booster is done
        //but the booster has been actually set off rather than deactivated
        OnBoosterDeactivated?.Invoke();
    }
}