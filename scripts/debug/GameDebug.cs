using System;

public static class GameDebug
{
    //TODO change to autoproperty if/when godot moves to c#>13, .net>10
    private static bool m_on = false;
    public static bool On {
        get => m_on;
        set {
            if (m_on != value)
            {
                m_on = value;
                OnGameDebugToggled?.Invoke();
            }
        }
    }
    
    public static event Action OnGameDebugToggled;
}