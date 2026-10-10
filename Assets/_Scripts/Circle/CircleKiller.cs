using UnityEngine;
using System.Collections;

public class CircleKiller : MonoBehaviour 
{
	#region STATICS
	GameManager m_gameManager {get { return GameManager.GetInstance(); } }
	HealthManager m_healthManager { get { return HealthManager.GetInstance (); } }
	#endregion

    public RectTransform RectPivot
    {
        get
        {
            return m_rectPivot;
        }

        set
        {
            m_rectPivot = value;
        }
    }  

    [SerializeField]
    RectTransform m_rectPivot;

    [SerializeField]
    bool m_mayLoseGame;
    

    public void Start()
    {
        var collider = GetComponent<BoxCollider2D>();

        if(m_rectPivot)
            collider.size = new Vector2(m_rectPivot.sizeDelta.x ,collider.size.y);
    }

	public void OnTriggerEnter2D(Collider2D coll)
	{
		if(coll.gameObject.ContainsTag("circle"))
		{
			var circle = coll.gameObject.GetComponent<Circle>();

			circle.KillCircle();

			if (!coll.gameObject.ContainsTag ("death")) 
			{
				if (m_mayLoseGame && m_gameManager.Kind == GameManager.kind.Survival)
					m_healthManager.RemoveLife ();					
			}
		}
    }
}
