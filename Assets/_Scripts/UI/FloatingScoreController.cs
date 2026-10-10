using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FloatingScoreController : MonoBehaviour 
{
    static FloatingScoreController s_instance;
    public static FloatingScoreController GetInstance(){ return s_instance; }

    [SerializeField]
    GameObject m_circleScoreAddedPoolPrefab;

    [SerializeField]
    List<FloatingScore> m_circleScoreAddedPool;

    Pool<FloatingScore> m_pool;

    Transform m_transform;

    void Awake()
    {
        if(!s_instance) s_instance = this;

        m_pool = Pool<FloatingScore>.CreatePool(m_circleScoreAddedPool, m_circleScoreAddedPoolPrefab, m_transform);
        m_transform = transform;
    }

    public void ShowPoints(Vector2 position, int points)
    {
        if (points <= 0)
            return;
        
        var score = m_pool.GetObject();
        score.Setup(position, points, m_pool);
    }
}
