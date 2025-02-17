using Godot;
using System;

public partial class Shell : Node
{
    private AbstractState m_currentState;

    private AbstractState[] m_stateMachine = new AbstractState[]
    {
        new StateInitialiseSystems(),
        
        new StateLoadConfigData(),
        new StateLoadSaveData(),
        new StateLoadHomeScene(),
     
        new StateDisableLoadingScreen()
    };

	public override void _Ready()
	{
        m_currentState = m_stateMachine[0];
        m_currentState.Begin();
	}

    public override void _Process(double delta)
    {
        if (!m_currentState.isFinished)
            return;
        
        // check if current finished state is final state
        int currentStateIndex = Array.IndexOf(m_stateMachine, m_currentState);

        // if not final state, transition to next state
        if (currentStateIndex < m_stateMachine.Length - 1)
        {
            m_currentState = m_stateMachine[currentStateIndex + 1];
            GD.Print("beginning state: " + m_currentState.GetType().ToString());
            m_currentState.Begin();
            return;
        }

        this.SetProcess(false);
    }
}