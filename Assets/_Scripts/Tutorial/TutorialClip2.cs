using UnityEngine;
using System.Collections;
//[MIGRATION] TODO: NEWWER VERSION
//using com.shephertz.app42.paas.sdk.csharp.imageProcessor;
using DG.Tweening;
using UnityEngine.UI;

public class TutorialClip2 : ITutorialStep
{
    public override string Text{ get { return Localization.CurrentLanguage.GetString("TUTORIAL_2"); } }
    public override string Tip{ get { return Localization.CurrentLanguage.GetString("PRO_TIP_2"); } }

    [SerializeField]
    RectTransform m_startBubble;

    [SerializeField]
    RectTransform m_endBubble;

    [SerializeField]
    Text m_pointText;

    [SerializeField]
    FloatingScore m_floatingScore;

    Tween m_tween;

    public override void Enable()
    {    
        gameObject.SetActive(true);

        m_tween = m_startBubble.DOLocalMove(m_endBubble.localPosition, 0.8f, true).SetEase(Ease.Linear).OnStepComplete(() =>
            {
                m_startBubble.gameObject.SetActive(false);
                m_pointText.text = "2";
                m_floatingScore.Setup(m_endBubble.localPosition, 2);
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
        m_startBubble.gameObject.SetActive(true);
        m_pointText.text = "1";
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
