using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class TutorialClip4 : ITutorialStep
{
    public override string Text{ get { return Localization.CurrentLanguage.GetString("TUTORIAL_4"); } }
    public override string Tip{ get { return Localization.CurrentLanguage.GetString("PRO_TIP_4"); } }

    [SerializeField]
    RectTransform[] m_bubbles;

    [SerializeField]
    RectTransform m_endTransform;

    Tweener m_rotateTween;

    Sequence m_sequence;

    public override void Enable()
    {  
        gameObject.SetActive(true);

        m_rotateTween = m_endTransform.DORotate(new Vector3(0, 0, -360), 2.5f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);

        if (m_bubbles.Length > 0)
        {
            m_sequence = DOTween.Sequence();

            m_bubbles[0].gameObject.SetActive(true);                    


            for (int i = 0; i < m_bubbles.Length; i++)
            {
                var tmpI = i;

                var tween = m_bubbles[i].DOLocalMove(m_endTransform.localPosition, 0.8f, true).SetEase(Ease.Linear);

                tween.OnStepComplete(() =>
                    {
                        m_bubbles[tmpI].gameObject.SetActive(false);

                        if (tmpI + 1 < m_bubbles.Length)
                            m_bubbles[tmpI + 1].gameObject.SetActive(true);
                        else
                            m_bubbles[0].gameObject.SetActive(true);
                    });

                m_sequence.Append(tween);
            }

            m_sequence.SetAutoKill(false);

            m_sequence.SetLoops(-1, LoopType.Restart).Play();
        }
    }
    public void Reset()
    {
        foreach (var bubble in m_bubbles)
            bubble.gameObject.SetActive(false);
    }

    public override void Disable()
    {
        m_sequence.Pause();
        m_sequence.Rewind();
        m_sequence.Kill();


        m_rotateTween.Pause();
        m_rotateTween.Rewind();
        m_rotateTween.Kill();
        Reset();

        gameObject.SetActive(false);
    }

}
