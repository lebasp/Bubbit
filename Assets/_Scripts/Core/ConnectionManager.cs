using UnityEngine;
using System.Collections;

public class ConnectionManager : MonoBehaviour 
{
	#region STATICS
	CircleIndicator m_circleIndicator { get { return CircleIndicator.GetInstance(); } }
	#endregion

	public Camera m_camera;

    [SerializeField]
	ConnectionLine m_line;
	public ConnectionLine Line{ get { return m_line; } }

	Circle m_centerCircle;
	public Circle CenterCircle{ get { return m_centerCircle; } }
	
	Circle m_targetCircle;
	public Circle TargetCircle{	get { return m_targetCircle; } }

	bool m_update;
	
	bool m_locked = false;
	public bool IsLocked{ get { return m_locked; } }

	static ConnectionManager s_instance;
	public static ConnectionManager GetInstance(){ return s_instance; }
	
	void Awake () 
	{
		if(!s_instance) s_instance = this;
	}

	public void SetConnectionCenter(Circle circle)
	{
		m_centerCircle = circle;
        m_line.gameObject.SetActive(true);
		m_line.Setup();
		StartCoroutine(UpdateConnection(m_line));

	}

	public void SetConnectionTarget(Circle circle)
	{

		m_targetCircle = circle;

		m_targetCircle.ControlState();

		m_locked = true;

		var radius = Mathf.Abs( Vector2.Distance (m_targetCircle.Position, CenterCircle.Position) );

		m_circleIndicator.Show (m_centerCircle.transform, radius);

	}

	IEnumerator UpdateConnection(ConnectionLine line)
	{
		m_update = Input.GetMouseButton(0);


		while(m_update)
		{

			line.SetStartPointAt(CenterCircle.Position);
			
			var mPos = InputHelper.MousePosition();
			
			if(!IsLocked)
			{				
				line.SetEndPointAt(mPos);
			}
			else 
			{	
				line.SetEndPointAt(TargetCircle.Position);
				
				var distance = Vector3.Distance(TargetCircle.Position, CenterCircle.Position);
				
				var normalize = (mPos - CenterCircle.Position).normalized;
				
				var angle = Mathf.Atan2(normalize.y, normalize.x);
				
				var targetNewPosition =  CenterCircle.Position + new Vector3(Mathf.Cos(angle) * distance, Mathf.Sin(angle) * distance);
				
				
				TargetCircle.SetPosition(targetNewPosition);
				
			}
			
			yield return null; 
			
			if(!Input.GetMouseButton(0))
			{
				m_update = false;
				FreeConnection();
			}
		}
	}
	
	public void StopUpdate()
	{
		m_update = false;
	}

	public void FreeConnection()
	{
        if (m_line) m_line.gameObject.SetActive(false);        
		if(CenterCircle) CenterCircle.FreeState();
		if(TargetCircle) TargetCircle.FreeState();

		m_circleIndicator.Hide ();

		m_centerCircle = null;
		m_targetCircle = null;

		m_locked = false;
		m_update = false;
	}
}
