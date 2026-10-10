using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;


public class TutorialClip1 : ITutorialStep 
{ 

    public override string Text{ get { return Localization.CurrentLanguage.GetString("TUTORIAL_1"); } }
    public override string Tip{ get { return Localization.CurrentLanguage.GetString("PRO_TIP_1"); } }

    [SerializeField]
    Image m_startBubble;

    [SerializeField]
    Image m_endBubble;

    [SerializeField]
    RectTransform m_connectionLine;

    [SerializeField]
    Transform m_hand;

    [SerializeField]
    Sprite m_selectedSprite;

    [SerializeField]
    Sprite m_originalSprite;

    Sequence m_sequence;

    [SerializeField]
    GameObject m_circleIndicator;

    [SerializeField]
    Transform[] path;

    Transform m_endBubbleTransform;
    Transform m_startBubbleTransform;

    void Awake()
    {
        m_endBubbleTransform = m_endBubble.transform;   
        m_startBubbleTransform = m_startBubble.transform;
    }

    public override void Enable()
    {      
        gameObject.SetActive(true);

        var vpath = new Vector3[path.Length];
        for (int i = 0; i < path.Length; i++)
        {
            vpath[i] = path[i].position;
        }

        m_sequence = DOTween.Sequence();

        m_hand.gameObject.SetActive(true);
        m_sequence.OnPlay(() => m_startBubble.sprite = m_selectedSprite); 
        m_sequence.Append(m_hand.DOLocalMove(m_endBubbleTransform.localPosition, 0.8f, true).SetEase(Ease.Linear));
        m_sequence.Join(m_connectionLine.DOSizeDelta(new Vector2(SetEndPointAt(m_endBubbleTransform.localPosition), 8), 0.8f, true).SetEase(Ease.Linear).OnStepComplete(() =>
                {
                    m_circleIndicator.SetActive(true);
                    m_endBubble.sprite = m_selectedSprite;
                }));
        m_sequence.Append(  m_endBubbleTransform.DOPath(vpath, 1, PathType.CatmullRom, PathMode.Ignore, 10, Color.white).SetLoops(3, LoopType.Yoyo).SetEase(Ease.Linear) );
        m_sequence.Join(  m_hand.DOPath(vpath, 1, PathType.CatmullRom, PathMode.Ignore, 10, Color.white).SetLoops(3, LoopType.Yoyo).SetEase(Ease.Linear) );
        m_sequence.Join( m_connectionLine.DOLocalRotate(new Vector3(0,0, -m_connectionLine.localRotation.eulerAngles.z), 1).SetLoops(3, LoopType.Yoyo).SetEase(Ease.Linear));
        m_sequence.SetAutoKill(false);            

        StartCoroutine(DoClip());
    }

    IEnumerator DoClip()
    {
        while (true)
        {
            if (m_sequence.IsPlaying())
            {
                yield return new WaitForSeconds(1);
            }
            else
            {               
                Reset();

                yield return new WaitForSeconds(2);
               
                m_hand.gameObject.SetActive(true);
                m_sequence.Rewind();
                m_sequence.Play();

            }
        }
    }

    public float SetEndPointAt(Vector3 position)
    {
		var size =   Vector2.Distance(position, m_connectionLine.localPosition ) ;

		var norm = (position - m_connectionLine.localPosition);
        m_connectionLine.localRotation = Quaternion.Euler(0,0, Mathf.Atan2(norm.y, norm.x) * Mathf.Rad2Deg);

        return size;
    }

    void Reset()
    {
        m_hand.position = m_startBubbleTransform.position;
        m_hand.gameObject.SetActive(false);

        m_startBubble.sprite = m_originalSprite;
        m_endBubble.sprite = m_originalSprite;
        m_connectionLine.sizeDelta = new Vector2(0, 7);
        m_circleIndicator.SetActive(false);
    }

    public override void Disable()
    {
        StopAllCoroutines();
        m_sequence.Rewind();
        m_sequence.Kill();
        Reset();

        gameObject.SetActive(false);
    }
	
}
