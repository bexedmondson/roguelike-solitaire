using System.Threading.Tasks;

public class StateLoadConfigData : AbstractState
{
    protected override string Name => nameof(StateLoadConfigData);

    protected override async Task DoStateTasksAsync()
    {
        DataLoader dataLoader = new DataLoader();

        await dataLoader.LoadAllResources();
        
        EndState();
    }
}