using System;
using System.Collections.Generic;

public class InventoryManager : IInjectable
{
    public event Action<Type, int> OnBoosterCountChanged;
    private Dictionary<Type, int> boosterInventory = new();

    public InventoryManager()
    {
        InjectionManager.Register(this);

        //TODO temporary!
        boosterInventory[typeof(BoosterShuffleStack)] = 5;
    }

    public int GetBoosterCount<T>() where T : AbstractBooster
    {
        return boosterInventory[typeof(T)];
    }

    public void AddBooster<T>() where T : AbstractBooster
    {
        var t = typeof(T);
        if (boosterInventory.TryGetValue(t, out var count))
            boosterInventory[t]++;
        else
            boosterInventory[t] = 1;
        OnBoosterCountChanged?.Invoke(t, boosterInventory[t]);
    }

    public void RemoveBooster<T>() where T : AbstractBooster
    {
        var t = typeof(T);
        boosterInventory[t]--;
        OnBoosterCountChanged?.Invoke(t, boosterInventory[t]);
    }
}