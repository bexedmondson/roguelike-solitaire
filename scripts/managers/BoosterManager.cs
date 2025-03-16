
using System;
using Godot;

public class BoosterManager : IInjectable
{
    public event Action OnBoosterPrimed;
    public event Action OnBoosterDeactivated;

    private IBooster activeBooster;

    public BoosterManager()
    {
        InjectionManager.Register<BoosterManager>(this);
    }

    public void OnBoosterButtonToggled(IBooster booster, bool toggleState)
    {
        if (toggleState)
        {
            activeBooster = booster;
            OnBoosterPrimed?.Invoke();
        }
        else
        {
            activeBooster = null;
            OnBoosterDeactivated?.Invoke();
        }
    }

    //TODO: change parameter to generic type mentioned in IBooster TODO
    public void ProcessBooster(Stack stack)
    {
        //GD.Print("booster process");
        if (activeBooster == null)
        {
            GD.PushError("trying to process booster but no booster active!");
            return;
        }

        activeBooster.OnBoosterEffectComplete += OnBoosterEffectComplete;
        activeBooster.DoBoosterEffect(stack);
    }

    private void OnBoosterEffectComplete()
    {
        //TODO: maybe need to rename this so it's clear that nothing else should be able to activate until this booster is done
        //but the booster has been actually set off rather than deactivated
        OnBoosterDeactivated?.Invoke();
    }
}