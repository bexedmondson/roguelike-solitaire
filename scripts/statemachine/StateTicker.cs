using Godot;
using System;

public partial class StateTicker : Node
{
    public event Action OnTick;

    public override void _Process(double delta)
    {
        OnTick?.Invoke();
    }
}
