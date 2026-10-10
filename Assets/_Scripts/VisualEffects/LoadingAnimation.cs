using UnityEngine;
using System.Collections;
using DG.Tweening;

public class LoadingAnimation : MonoBehaviour {

    Tweener m_tween;
	
	void OnEnable () 
	{	
        m_tween = transform.DORotate(new Vector3(0, 0, -360), 1f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
	}

    void OnDisable()
    {
        m_tween.Pause();
        m_tween.Restart();
        m_tween.Kill();
    }
}
