using UnityEngine;
using System.Collections;

public class ConnectionLine : MonoBehaviour 
{
	#region STATICS
	CanvasSettings m_canvasSettings {get { return CanvasSettings.GetInstance(); } }
    ConnectionManager m_connectionManager { get { return ConnectionManager.GetInstance(); } }
    #endregion


    RectTransform m_rectTransform;
	BoxCollider2D m_collider;

	

	public void Setup()
	{
		m_rectTransform = GetComponent<RectTransform>();

		m_collider = GetComponent<BoxCollider2D>();

		m_rectTransform.SetParent(m_connectionManager.transform, false);
	}

	public void SetStartPointAt(Vector3 position)
	{
		m_rectTransform.position = position;

	}

	public void SetEndPointAt(Vector3 position)
	{

		var distance = m_canvasSettings.Convert( Vector2.Distance(position, m_rectTransform.position) );


		m_rectTransform.sizeDelta = new Vector2((distance / m_rectTransform.localScale.x), m_rectTransform.rect.height);
		
		var norm = (position - m_rectTransform.position);
		m_rectTransform.localRotation =Quaternion.Euler(0,0, Mathf.Atan2(norm.y, norm.x) * Mathf.Rad2Deg);

		m_collider.size = new Vector2(m_rectTransform.rect.width, m_rectTransform.rect.height);
		m_collider.offset = new Vector2(m_rectTransform.rect.width/2, 0);

	}

	public void OnTriggerEnter2D(Collider2D coll)
	{
		if(m_connectionManager.IsLocked)
			return;

		if(coll.gameObject.ContainsTag("circle"))
		{
			var circle = coll.gameObject.GetComponent<Circle>();

			if(circle.State == Circle.state.Free || circle.State == Circle.state.Frozen)
			{
				SetEndPointAt(circle.Position);

                m_connectionManager.SetConnectionTarget(circle);
			}
		}
	}

}
