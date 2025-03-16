using System;
using Godot;

public class BoosterShuffleStack : IBooster
{
    public event Action OnBoosterEffectComplete;

    public void DoBoosterEffect(Stack stack)
    {
        //GD.Print("do booster effect: shuffle stack");
        stack.Shuffle();
        //GD.Print("do booster effect: shuffle stack complete");
        OnBoosterEffectComplete?.Invoke();
    }
}