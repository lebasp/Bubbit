using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class RibonAnimation : MonoBehaviour {

    [SerializeField]
    Color m_colorFlash = Color.white;
 
    Sequence m_sequence;

    Color m_defaultColor;

    void OnEnable ()
    {   
        if (m_sequence == null)
        {
            m_sequence = DOTween.Sequence();

            var target = GetComponent<Graphic>();

            m_sequence.Append(DOTween.To(() => target.color, (color) => target.color = color, m_colorFlash, 0));
            m_sequence.Append(DOTween.To(() => target.color, (color) => target.color = color, target.color, 0).SetDelay(0.5f));        

            m_sequence.SetLoops(-1, LoopType.Yoyo);
            m_sequence.SetAutoKill(false);
        }

        m_sequence.Play();
	}

    void OnDisable()
    {
        m_sequence.Pause();
    }

    void OnDestroy()
    {
        m_sequence.Kill();
    }
  



}
