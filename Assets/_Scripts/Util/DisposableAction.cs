using System;
using UnityEngine;

public class DisposableAction : IDisposable
{
    Action m_action;
    GameObject m_gameObject;

    public DisposableAction(GameObject gameObject, Action action)
    {
        m_gameObject = gameObject;
        m_action = action;
    }

    public void Run()
    {
        if (m_gameObject && m_action != null)
            m_action.Invoke();
        else
            Dispose();
    }

    public void Dispose()
    {
        m_action = null;
    }

}

public class DisposableAction<T> : IDisposable
{
    Action<T> m_action;
    GameObject m_gameObject;

    public DisposableAction(GameObject gameObject, Action<T> action)
    {
        m_gameObject = gameObject;
        m_action = action;
    }

    public void Run(T obj)
    {
        if (m_gameObject && m_action != null)
            m_action.Invoke(obj);
        else
            Dispose();
    }

    public void Dispose()
    {
        m_action = null;
    }

}

public static class DActionExtension
{
    public static DisposableAction<T> addAction<T>(this MonoBehaviour mb, Action<T> action)
    {
        return new DisposableAction<T>(mb.gameObject, action);
    }

    public static DisposableAction addAction(this MonoBehaviour mb, Action action)
    {
        return new DisposableAction(mb.gameObject, action);
    }
}