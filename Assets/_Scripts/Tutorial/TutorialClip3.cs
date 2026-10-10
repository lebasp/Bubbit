using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class TutorialClip3 : ITutorialStep
{
    public override string Text{ get { return Localization.CurrentLanguage.GetString("TUTORIAL_3"); } }
    public override string Tip{ get { return Localization.CurrentLanguage.GetString("PRO_TIP_3"); } }

    [SerializeField]
    Image m_heart;

    [SerializeField]
    Sprite m_fullHeart;

    [SerializeField]
    Sprite m_emptyHeart;

    [SerializeField]
    RectTransform m_startBubble;

    [SerializeField]
    RectTransform m_endBubble;

    Tween m_tween;

    public override void Enable()
    {   
        gameObject.SetActive(true);

        m_tween = m_startBubble.DOLocalMove(m_endBubble.localPosition, 0.8f, true).SetEase(Ease.Linear).OnStepComplete(() =>
            {
                m_heart.sprite = m_emptyHeart;
                m_startBubble.gameObject.SetActive(false);
                m_endBubble.gameObject.SetActive(false);
            });
        
        m_tween.SetAutoKill(false);

        StartCoroutine(DoClip());   
    }

    IEnumerator DoClip()
    {
        while (true)
        {
            if (m_tween.IsPlaying())
            {
                yield return new WaitForSeconds(1);
            }
            else
            {
                yield return new WaitForSeconds(2);

                Reset();

                m_tween.Rewind();
                m_tween.Play();
            }
        }
    }

    void Reset()
    {
        m_heart.sprite = m_fullHeart;
        m_startBubble.gameObject.SetActive(true);
        m_endBubble.gameObject.SetActive(true);
    }

    public override void Disable()
    {
        StopAllCoroutines();
        m_tween.Rewind();
        m_tween.Kill();
        Reset();

        gameObject.SetActive(false);
    }
}
