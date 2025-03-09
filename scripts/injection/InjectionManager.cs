using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using Godot;

public interface IInjectable
{
    
}

public class InjectionManager
{
    private static InjectionManager instance;
    public static InjectionManager Instance()
    {
        if (instance == null)
            instance = new InjectionManager();
        return instance;
    }

    private static Dictionary<Type, IInjectable> m_injectableMap = new Dictionary<Type, IInjectable>();

    public static void Register<T>(T injectable) where T : IInjectable
    {
        if (m_injectableMap.ContainsKey(typeof(T)))
        {
            GD.PushError("[InjectionManager] Injectable of type " + typeof(T).AssemblyQualifiedName + " already registered! Discarding new.");
            return;
        }

        m_injectableMap.Add(typeof(T), injectable);
    }

    public static T Get<T>() where T : IInjectable
    {
        if (m_injectableMap.TryGetValue(typeof(T), out IInjectable injectableT))
        {
            return (T)injectableT;
        }

        GD.PrintErr($"Injection of type {typeof(T)} failed!");
        return default(T);
    }

    public static bool Has<T>() where T : IInjectable
    {
        return m_injectableMap.ContainsKey(typeof(T));
    }
}