using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OffsetScroller : MonoBehaviour 
{
	public float m_scrollSpeed;
	
	private Vector3 m_startPosition;

	[SerializeField]
	float m_tileSize = 0;

	[SerializeField]
	RectTransform m_canvasTransform;

	RectTransform m_transform;

	float m_sizeConverted;
	public float m_speedModifier = 1;

	void Start ()
	{
		m_transform = GetComponent<RectTransform>();

		m_sizeConverted= (m_tileSize * m_canvasTransform.localScale.y);

		m_startPosition = new Vector3( m_transform.position.x, - m_sizeConverted, m_transform.position.z );
	}
	
    public void Move(float deltaTime)
    {		
        var finalSpeed = m_scrollSpeed * m_speedModifier * deltaTime;

        if (m_transform.position.y >= m_sizeConverted)
            m_transform.position = m_startPosition;
        else
            m_transform.position += Vector3.up * finalSpeed;     
    }

	public void SetSpeedModifier(float speed)
	{
		m_speedModifier = speed;
	}
}