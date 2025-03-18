using System;

public abstract class AbstractBooster
{
    public enum State
    {
        None,
        Primed,
        Activated,
        Deactivated
    }

    public State state;

    public event Action OnBoosterEffectComplete;

    public abstract void DoBoosterEffect();

    protected void CompleteEffect()
    {
        OnBoosterEffectComplete?.Invoke();
    }
}

public abstract class AbstractBooster<T> : AbstractBooster where T : AbstractBoosterData
{
    protected T data;

    public void SetupBoosterData(T boosterData)
    {
        this.data = boosterData;
    }
}

public abstract class AbstractBoosterData
{

}