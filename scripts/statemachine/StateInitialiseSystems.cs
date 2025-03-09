using System.Collections.Generic;
using System.Threading.Tasks;

public class StateInitialiseSystems : AbstractState
{
    protected override string Name => nameof(StateInitialiseSystems);

    protected override async Task DoStateTasksAsync()
    {
        ScoreManager scoreManager = new ScoreManager();
    }
}