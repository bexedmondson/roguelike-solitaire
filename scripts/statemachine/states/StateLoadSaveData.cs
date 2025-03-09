using System.Threading.Tasks;
using Godot;

public class StateLoadSaveData : AbstractState
{
    protected override string Name => nameof(StateLoadSaveData);

    protected override async Task DoStateTasksAsync()
    {   
        EndState();
    }
}