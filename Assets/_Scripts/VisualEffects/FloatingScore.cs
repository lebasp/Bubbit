using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class FloatingScore : MonoBehaviour {

    [SerializeField]
    bool m_isUI;

    Color m_defaultColor;

    Transform m_transform;

    TextMesh m_textMesh;

    Text m_text;

    void Awake()
    {
        if(m_isUI)
            m_text = GetComponent<Text>();
        else
            m_textMesh = GetComponent<TextMesh>();
        
        m_transform = transform;

        m_defaultColor = m_isUI?m_text.color:m_textMesh.color;
    }
     
    public void Setup (Vector3 spawnPoint, int points, Pool<FloatingScore> m_pool = null)
    {
        gameObject.SetActive(true);

        m_transform.localScale = Vector3.one;

        var text = "+" + points;

        if (m_isUI)
            m_text.text = text;
        else
            m_textMesh.text = text;

        m_transform.localPosition = spawnPoint;

        var sequence = DOTween.Sequence();

        sequence.Insert(0, DOTween.ToAlpha(GColor, SColor, 0, 0.5f).SetDelay(0.3f));
        sequence.Insert(0, DOTween.ToAxis(GScale, SScale, 1.5f, 0.7f, AxisConstraint.None));
        //sequence.Insert(0, m_transform.DOMoveY (m_transform.position.y + 10, 0.7f));

        sequence.Play();
        sequence.OnComplete(() =>
            {
                gameObject.SetActive(false);

                if(m_isUI)
                    m_text.color = m_defaultColor;
                else
                    m_textMesh.color = m_defaultColor;

                if(m_pool != null)
                    m_pool.DeactiveFromPool(this);
            });

    }

    Color GColor() { return m_isUI?m_text.color:m_textMesh.color; }
    void SColor(Color color)
    {
        if (m_isUI)
            m_text.color = color;
        else
            m_textMesh.color = color;
    }

    Vector3 GScale(){   return Vector3.one;    }
    void SScale(Vector3 vector3){ m_transform.localScale = vector3; }

}
