using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using System;

public class TransitionEffect : MonoBehaviour 
{
    static TransitionEffect s_instance;
    public static TransitionEffect GetInstance()
    {
        return s_instance;
    }

    [SerializeField]
    GameObject m_root;

    [SerializeField]
    Image  m_image;

	void Awake ()
    {
        if (s_instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(this.gameObject);

            InstantBlackout();
        }
	}

    public void InstantBlackout()
    {
        m_image.color = new Color(m_image.color.r, m_image.color.g,  m_image.color.b,  1);
    }

    public void Blackout(Action onFinished)
    {
        if (m_image.color.a == 1)
        {
            onFinished();
        }
        else
        {
            m_image.color = new Color(m_image.color.r, m_image.color.g, m_image.color.b, 0);

            m_root.SetActive(true);

            var tween = m_image.DOFade(1, 0.2f).SetEase(Ease.InCirc);
            tween.OnComplete(() => onFinished());
            tween.Play();
        }
    }

    public void Dismiss(DisposableAction finished = null)
    {
        m_image.color = new Color(m_image.color.r, m_image.color.g,  m_image.color.b,  1);

        var tween = m_image.DOFade(0, 0.2f).SetEase(Ease.OutCirc);
        tween.OnComplete(() =>
            {
                if(finished != null)
                    finished.Run();

                m_root.SetActive(false);
            });
        tween.Play();
    }
}
