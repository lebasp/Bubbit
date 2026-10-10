using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircleIndicator : MonoBehaviour {


#region STATICS
CanvasSettings m_canvasSettings {get { return CanvasSettings.GetInstance(); } }
#endregion

float m_spriteSize = 30;

bool m_active = false;

Transform m_transform;

public GameObject prefab;

Transform m_gameObjectAnchor;

    [SerializeField]
    List<GameObject> m_pool = new List<GameObject>();

    static CircleIndicator s_instance;
    public static CircleIndicator GetInstance(){ return s_instance; }

    void Awake ()
    {
        if (!s_instance)
            s_instance = this;

        m_transform = this.transform;

        foreach (var go in m_pool)
            go.SetActive(false);
    }

    public void Show (Transform gameObjectAnchor, float radius)
    {
        radius = m_canvasSettings.Convert(radius);

        m_gameObjectAnchor = gameObjectAnchor;

        float circunference = 2 * 3.1415f * radius;

        float amount = (int)(circunference / m_spriteSize);

        for (int i = 0; i < amount; i++)
        {
            SetPointAtAngle(i, radius, (360f / amount) * i); 
        }

        m_active = true;

        StartCoroutine(Move());
    }

    void SetPointAtAngle(int index, float radius, float ang)
    {
        Vector2 pos;
        pos.x = radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.y = radius * Mathf.Sin(ang * Mathf.Deg2Rad);

        var rot = Quaternion.FromToRotation(Vector2.up, pos);

        if (index > m_pool.Count - 1)
        {
            var go = Instantiate(prefab, pos, rot) as GameObject;
            go.transform.SetParent(m_transform, false);
            m_pool.Add(go);

        }
        else
        {
            m_pool[index].SetActive(true);

            var tmpTrans = m_pool[index].transform;

            tmpTrans.rotation = rot;
            tmpTrans.localPosition = pos;
        }
    }

    public void Hide()
    {
        m_active = false;

        foreach (var go in m_pool)
            go.SetActive(false);
    }

    public IEnumerator Move()
    {
        while (m_active)
        {
            m_transform.position = m_gameObjectAnchor.position;

            yield return new WaitForFixedUpdate();
        }
    }

}
