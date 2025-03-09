
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;

public abstract class AbstractState
{
    public event Action<AbstractState> OnFinished;

    protected abstract string Name { get; }

    private AbstractState defaultNextState;

    private List<AbstractStateTransition> nextStateTransitionAlternatives = new();

    public void AddStateTransitions(AbstractState defaultState, AbstractStateTransition[] stateTransitions = null)
    {
        defaultNextState = defaultState;

        if (stateTransitions != null)
            nextStateTransitionAlternatives.AddRange(stateTransitions);
    }

    public async void Run()
    {
        await StateTasksAsync();
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

    protected void EndState()
    {
        OnFinished?.Invoke(GetNextState());
    }

    protected AbstractState GetNextState()
    {
        foreach (var stateTransition in nextStateTransitionAlternatives)
        {
            if (stateTransition.EvaluateShouldTransition())
                return stateTransition.targetState;
        }

        return defaultNextState;
    }
}