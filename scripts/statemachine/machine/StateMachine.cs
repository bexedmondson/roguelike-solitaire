
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
        var solitaireGameSetupState = new StateSolitaireGameSetup();
        var disableLoadingScreenState = new StateDisableLoadingScreen();
        var solitaireGameRunState = new StateSolitaireGameRun();
        var enableLoadingScreenState = new StateEnableLoadingScreen();
        var solitaireGameTeardownState = new StateSolitaireGameTeardown();

        shellSystemsState.AddStateTransitions(loadConfigDataState);
        loadConfigDataState.AddStateTransitions(loadSaveDataState);
        loadSaveDataState.AddStateTransitions(loadHomeSceneState);
        loadHomeSceneState.AddStateTransitions(solitaireGameSetupState);
        
        solitaireGameSetupState.AddStateTransitions(disableLoadingScreenState);
        disableLoadingScreenState.AddStateTransitions(solitaireGameRunState);

        solitaireGameRunState.AddStateTransitions(enableLoadingScreenState);
        enableLoadingScreenState.AddStateTransitions(solitaireGameTeardownState);
        solitaireGameTeardownState.AddStateTransitions(loadHomeSceneState);
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