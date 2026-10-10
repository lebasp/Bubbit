using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class PulsingAnimation : MonoBehaviour
{
    Transform m_transform;

    Text m_text;
    TextMesh m_textMesh;

    [SerializeField]
    float m_percentage = 1;
    [SerializeField]
    float m_speed = 0;

    bool m_started = false;
    Tweener m_tweener;

    public void Awake()
    {
        m_transform = transform;
    }

    public void Pulse()
    {
        if (m_started)
            return;

        m_started = true;

        m_tweener= m_transform.DOScale(new Vector3(m_percentage, m_percentage, 0), m_speed).SetLoops(-1, LoopType.Yoyo);
    }

    public void StopPulse()
    {
        m_started = false;

        m_tweener.Kill();

        m_transform.localScale = Vector2.one;
    }


}
