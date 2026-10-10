using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BubbleAnimation : MonoBehaviour 
{   

    float m_originaSize;

    [SerializeField]
    float m_finalSize;


    Sequence m_sequence;
    Transform m_transform;

    Tween m_shakeTween;


	public void Setup ()
    {
        m_transform = transform;

        m_originaSize = m_transform.localScale.x;

        m_sequence = DOTween.Sequence();
        m_sequence.Append(m_transform.DOScale(new Vector3(m_finalSize, m_originaSize, 0), 0.15f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutBounce));
        m_sequence.Append(m_transform.DOScale(new Vector3(m_originaSize, m_finalSize, 0), 0.15f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutBounce));
        m_sequence.SetAutoKill(false);
        m_sequence.Pause();

        m_shakeTween = m_transform.DOShakeScale(0.7f, 30, 10, 90).SetEase(Ease.InOutBounce).Pause();
        m_shakeTween.SetAutoKill(false);
    }       

    public bool IsPlaying()
    {
        return m_sequence.IsPlaying();
    }

    public void Shake()
    {
        m_shakeTween.Play();
    }

    public void StopShake()
    {
        m_shakeTween.Pause();
        m_shakeTween.Rewind();
    }

    public void Play()
    {
        
        m_sequence.SetLoops(-1).Play();
    }

    public void Pause()
    {
        m_sequence.Pause();
    }

    public void OnDestroy()
    {
        if(m_sequence != null)
            m_sequence.Kill();
    }

}
