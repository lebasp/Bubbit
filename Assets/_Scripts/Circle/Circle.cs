using UnityEngine;
using System.Collections;

public class Circle : MonoBehaviour 
{
	#region STATICS
	CanvasSettings m_canvasSettings {get { return CanvasSettings.GetInstance(); } }
	protected GameManager m_gameManager {get { return GameManager.GetInstance(); } }
    protected ConnectionManager m_connectionManager {get { return ConnectionManager.GetInstance(); } }
	CircleSpawner m_circleSpawner {get { return CircleSpawner.GetInstance(); } }
	CircleCollision m_circleCollision {get { return CircleCollision.GetInstance(); } }
    DifficultyManager m_difficultManager { get { return DifficultyManager.GetInstance(); } }
    GameStats m_gameStats { get { return GameStats.GetInstance(); } }
    #endregion

    public enum state
	{
		Free, 
		Locked,
		Controlling,
		Frozen,
        Dead
	}

	public delegate void onStateChanged(state currentState);
	public event onStateChanged OnStateChanged;

	public delegate void onPointsChanged(int points);
	public event onPointsChanged OnPointsChanged;

	public delegate void onColision();
	public event onColision OnColision;

    public delegate void onPointsDoubledStarted();
    public event onPointsDoubledStarted OnPointsDoubledStarted;

    public delegate void onPointsDoubledEnded();
    public event onPointsDoubledEnded OnPointsDoubledEnded;

    public delegate void onShake();
    public event onShake OnShake;


	bool m_moving = false;
	public bool IsMoving
	{
		get
		{
			return m_moving;
		}
		set
		{
			m_moving = value;
		}
	}

	private state m_state = state.Free;
	public state State
	{
		get
		{
			return m_state; 
		}
		set
		{
			m_state = value;
		}
	}


    [SerializeField]
	PowerUp m_powerUp;

	public PowerUp PowerUp
	{
		get{ return m_powerUp; }
	}

	
	public Vector3 Position{ get { return m_transform.position; } }

    bool m_pause = false;

	[SerializeField]
	float m_speed;
	public float Speed
	{
		get 
		{
			var finalSpeed = ((m_speed + (Points / 20f)) * m_gameManager.SpeedModifier) * m_difficultManager.OverallGameSpeed;

			var slowDown = State == state.Locked || State == state.Controlling;

			finalSpeed = slowDown?finalSpeed/2:finalSpeed;

			finalSpeed = m_canvasSettings.Convert( finalSpeed );
            
            return finalSpeed;
		}
	}

	int m_points = 1;
	public int Points
    {
        get { return m_points; }
		private set{ m_points = value; }  
	}

    Transform m_transform;
    Collider2D m_collider;


	void Awake()
    {
        m_transform = transform;
        m_collider = GetComponent<Collider2D>();
    }

    void Start()
    {
        m_gameManager.OnGamePausedEvent += Pause;
    }

    public void Setup(int points, Vector3 spawnPosition)
    {
        gameObject.SetActive(true);

        m_moving = true;

        m_pause = m_gameManager.GamePaused;

        m_transform.localPosition = spawnPosition;

        UpdatePointsTo(points);

        FreeState();

        StartCoroutine(MoveCircle());
        
        EnableCollider();
    }

	void OnMouseDown()
	{
        if (m_pause)
            return;
        
        if(State == state.Free)
			LockState();
        if(State == state.Frozen)
            PickUpCircle();
	}

	public void UpdatePointsTo (int points)
	{		
		Points = points;

		if(OnPointsChanged != null) OnPointsChanged(Points);
	}

    public void SetDoublePointsEffect(bool doublePoints)
    {
        if(doublePoints)
        {
            if (OnPointsDoubledStarted != null)
                OnPointsDoubledStarted();
        }
        else if (OnPointsDoubledEnded != null)
            OnPointsDoubledEnded();
    }

    public void Shake()
    {
        if (OnShake != null)
            OnShake();
    }

    //States	
    public void FreeState()
	{
		m_state = state.Free;

		IsMoving = true;

        if (OnStateChanged != null)
        {            
            OnStateChanged(State);
        }
	}

	void LockState()
	{
		m_state = state.Locked;

		m_connectionManager.SetConnectionCenter(this);

		if(OnStateChanged != null) OnStateChanged(State);
	}

	public void ControlState()
	{
		m_state = state.Controlling;

		if(OnStateChanged != null) OnStateChanged(State);
	}

	public void FrozenState()
	{
		m_state = state.Frozen;

		IsMoving = false;

		if(OnStateChanged != null) OnStateChanged(State);
	}

    public void DeadState()
    {
        m_state = state.Dead;

        IsMoving = false;

        if(OnStateChanged != null) OnStateChanged(State);
    }

	//Movement
	void Pause(bool paused)
	{
        m_pause = paused;
	}

	IEnumerator MoveCircle()
	{
		while(true)
		{
            if (IsMoving && !m_pause) 
			{
				var direction = new Vector3 (0, 0.1f, 0);

				var newPosition = Position + (direction * Speed);

				SetPosition (newPosition);
			}

			yield return new WaitForFixedUpdate ();
		}
	}

	public void SetPosition(Vector2 position)
	{
		m_transform.position = position;
	}

    public void EnableCollider()
    {
        m_collider.enabled = true;
    }

    public void DisableCollider()
    {
        m_collider.enabled = false;
    }

	//Collision
	public virtual void OnTriggerStay2D(Collider2D coll)
	{  
		if (coll.gameObject.ContainsTag("circle"))
		{
            var circle = coll.gameObject.GetComponent<Circle>();

			if(State == state.Controlling || circle.State == state.Controlling)
				m_connectionManager.FreeConnection();

			if(circle.State == Circle.state.Free)
			{
				if(OnColision != null) OnColision();

				m_circleCollision.CollideCircles(this, circle);
			}

		}
	}

    public void PickUpCircle()
    {
        if (Points > 0)
        {
            m_gameStats.AddScore(Points);
            FloatingScoreController.GetInstance().ShowPoints(m_canvasSettings.Convert(Position), Points);
        }

        UserPoweUp();
        
        KillCircle();
    }

	public void KillCircle()
	{
		if(State != state.Free)
		{
			m_connectionManager.FreeConnection();
		}

        if(m_powerUp)
        {
            m_powerUp.ResetPowerUp();
            m_powerUp = null;
        }

        DeadState();

        gameObject.SetActive(false);

		m_circleSpawner.Remove(this);

		StopAllCoroutines();
    }

	public void SetupPowerUp(PowerUp powerUp)
	{		
		m_powerUp = powerUp;
		m_powerUp.SetParent(m_transform);	
	}

	public void UserPoweUp()
	{
		if (m_powerUp) 
		{
			m_powerUp.PickUp ();
			m_powerUp = null;
		}
	}

	void OnDestroy()
	{
		m_gameManager.OnGamePausedEvent -= Pause;
	}
}
