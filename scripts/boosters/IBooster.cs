using System;

public interface IBooster
{
    public event Action OnBoosterEffectComplete;

    //TODO swap to abstract w/new generic type that contains the info needed to do the booster effect? 
    void DoBoosterEffect(Stack stack);
}