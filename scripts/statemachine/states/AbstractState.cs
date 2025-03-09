
using System;
using System.Collections;
using System.Threading.Tasks;
using Godot;

public abstract class AbstractState
{
    public bool isFinished { get; set; } = false;

    protected abstract string Name { get; }

    public async void Begin()
    {
        isFinished = false;

       await StateTasksAsync();

       isFinished = true;
    }

    private async Task StateTasksAsync()
    {
        GD.Print($"State tasks begin: {Name}");

        await DoStateTasksAsync();

        GD.Print($"State tasks end: {Name}");
    }

    protected virtual async Task DoStateTasksAsync()
    {
        
    }
}