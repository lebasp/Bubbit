using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pool<T> where T : MonoBehaviour
{
    Dictionary<T, bool> m_objects = new Dictionary<T, bool>();
    GameObject m_prefab;
    Transform m_parent;

    bool m_initialized = false;

    public static Pool<T> CreatePool(List<T> preInstantiated, GameObject prefab, Transform parent)
    {
        var pool = new Pool<T>();

        pool.m_parent = parent;
        pool.m_prefab = prefab;

        foreach (var obj in preInstantiated)
        {
            pool.m_objects[obj] = false;
            obj.gameObject.SetActive(false);
        }

        pool.m_initialized = true;

        return pool;
    }

    public T GetObject()
    {
        if (!m_initialized)
        {
            Debug.Log("POOL NOT INITIALIZED");
            return null;
        }

        T obj = null;

        foreach (var key in m_objects.Keys)
            if (!m_objects[key])
            {
                obj = key;
                m_objects[key] = true;
                break;
            }

        if(obj == null)
        {
            var go = GameObject.Instantiate(m_prefab);
            obj = go.GetComponent<T>();
            go.transform.SetParent(m_parent, false);
            m_objects[obj] = true;
        }

        return obj;
    }

    public void DeactivateFromPool(T obj)
    {
        if (!m_initialized)
        {
            Debug.Log("POOL NOT INITIALIZED");
        }

        if(m_objects.ContainsKey(obj))
            m_objects[obj] = false;
    }

    public void DeactivateWholePool()
    {
        var buffer = new List<T>(m_objects.Keys);

        foreach (var key in buffer)
        {
            m_objects[key] = false;
        }
    }




}
