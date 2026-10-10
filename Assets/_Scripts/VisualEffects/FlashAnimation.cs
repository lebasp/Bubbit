using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class FlashAnimation : MonoBehaviour {

    [SerializeField]
    Graphic m_target;

    Color m_defaultColor;

    Tween m_tween;

    void Awake()
    {
        m_defaultColor = m_target.color;
    }

    public void Flash()
    {
        DOTween.To(GColor,SColor,Color.white, 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutExpo);
    }

    Color GColor() { return m_defaultColor; }
    void SColor(Color color){ m_target.color = color; }

}
