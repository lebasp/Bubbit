using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CircleViewWithText: CircleView 
{	
    [SerializeField]
    Color m_doublePointsColor;

	[SerializeField]
    TextMesh m_text;

    [SerializeField]
    PulsingAnimation m_pulseTexting;

    Color m_defaultColor;

	protected override void Awake()
	{	
		base.Awake ();

        m_defaultColor = m_text.color;

		m_circle.OnPointsChanged += (points) => 
		{
			m_text.text = points<1?"":points.ToString();
		};

        m_circle.OnPointsDoubledStarted += StartDoublePoint;
        m_circle.OnPointsDoubledEnded += StopDoublePoint;

    }

	void StartDoublePoint()
	{
        m_text.color = m_doublePointsColor;
        m_pulseTexting.Pulse();
    }

    void StopDoublePoint()
    {
        m_text.color = m_defaultColor;
        m_pulseTexting.StopPulse();
    }

    void OnDestroy()
    {
        m_circle.OnPointsDoubledStarted -= StartDoublePoint;
        m_circle.OnPointsDoubledEnded -= StopDoublePoint;
    }
}
