using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class FilledTiledBar : MonoBehaviour 
{
    RectTransform m_rectTransform;

    [SerializeField]
    RectTransform m_background;
    
    [SerializeField]
    Vector2 m_spriteSize;

    [SerializeField]
    float m_amount;

    [SerializeField]
    bool m_flashWhenUpdated;

    int divisions;

    public void Start()
    {
        m_rectTransform = GetComponent<RectTransform>();

        divisions = (int)(m_background.rect.width / m_spriteSize.x);

        m_background.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, divisions * m_spriteSize.x);


        AdjustBar();
    }

    public float Amount
    {
        get { return m_amount; }
        set
        {
            m_amount = value;

            AdjustBar();
        }
    }

    void AdjustBar()
    {
        var x = m_spriteSize.x + ((int)((divisions * m_amount)) * m_spriteSize.x);

        m_rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, x);
    }

 
}
