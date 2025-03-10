
using System.Collections.Generic;

public class StateMachine
{

    private AbstractState m_currentState;

    private AbstractState[] m_stateMachine = new AbstractState[]
    {
        new StateInitialiseShellSystems(),
        
        new StateLoadConfigData(),
        new StateLoadSaveData(),
        new StateLoadHomeScene(),
     
        new StateDisableLoadingScreen()
    };

    public void Begin()
    {
        InitialiseStates(out var initialState);

        TransitionToState(initialState);
    }

    private void InitialiseStates(out AbstractState initialState)
    {
        var shellSystemsState = new StateInitialiseShellSystems();
        initialState = shellSystemsState;

        var loadConfigDataState = new StateLoadConfigData();
        var loadSaveDataState = new StateLoadSaveData();
        var loadHomeSceneState = new StateLoadHomeScene();
        var disableLoadingScreenState = new StateDisableLoadingScreen();
        var solitaireGameState = new StateSolitaireGame();
        var enableLoadingScreenState = new StateEnableLoadingScreen();
        var teardownGameState = new StateTeardownGame();

        shellSystemsState.AddStateTransitions(loadConfigDataState);
        loadConfigDataState.AddStateTransitions(loadSaveDataState);
        loadSaveDataState.AddStateTransitions(loadHomeSceneState);
        loadHomeSceneState.AddStateTransitions(disableLoadingScreenState);
        disableLoadingScreenState.AddStateTransitions(solitaireGameState);
        solitaireGameState.AddStateTransitions(enableLoadingScreenState);
        enableLoadingScreenState.AddStateTransitions(teardownGameState);
        teardownGameState.AddStateTransitions(loadHomeSceneState);
    }

    private void TransitionToState(AbstractState newState)
    {
        if (m_currentState != null)
        {
            m_currentState.OnFinished -= TransitionToState;
        }

        m_currentState = newState;
        m_currentState.OnFinished += TransitionToState;

        m_currentState.Run();
    }
}