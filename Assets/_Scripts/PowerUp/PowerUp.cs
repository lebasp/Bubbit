using UnityEngine;
using System.Collections;

public class PowerUp  : MonoBehaviour
{	
	#region STATICS
	protected GameManager m_gameManager {get { return GameManager.GetInstance(); } }
	protected CircleSpawner m_circleSpawner {get { return CircleSpawner.GetInstance(); } }
    protected BackgroundController m_backgroundController { get { return BackgroundController.GetInstance(); } }
	#endregion

	protected Transform m_transform;

	protected SpriteRenderer m_spriteRenderer;

    [SerializeField]
	protected Color m_color;

    [SerializeField]
    AudioSource m_source;

	protected bool m_pause;
    protected bool m_running;

    public bool IsRunning{ get { return m_running; } }

    public delegate void onPowerUpReset();
    public event onPowerUpReset OnPowerUpReset;  

	public Color Color
	{
		get{ return m_color; }
	}

    Transform m_parent;

	void Awake()
    {
        m_transform = transform;

        m_spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
		m_gameManager.OnGamePausedEvent += Pause;
        m_gameManager.OnGameEndedEvent += ForceStop;

        m_parent = transform.parent;

        ResetPowerUp();
    }

    public void ResetPowerUp()
    {
        m_running = false;

        if(m_source)
            m_source.Stop();

        m_pause = false;
        m_spriteRenderer.enabled = true;

        m_transform.localScale = new Vector3(100, 100, 100);
        m_transform.localPosition = Vector3.zero;
        m_transform.SetParent(m_parent, false);

        if (OnPowerUpReset != null)
            OnPowerUpReset();

        gameObject.SetActive(false);
    }

	void Pause(bool paused)
    {     
        if (IsRunning && m_source && m_source.loop)
        {
            if (paused)
                m_source.Pause();
            else
                m_source.Play();
        }

        m_pause = paused;
    }

	public void SetParent(Transform parent)
	{
        gameObject.SetActive(true);
		m_transform.SetParent(parent, false);
	}

	public virtual void PickUp()
	{	
		m_transform.SetParent(m_circleSpawner.transform, true);
        
		m_spriteRenderer.enabled = false;

        m_running = true;

        if(m_source)
            m_source.Play();
	}

    public virtual void ForceStop()
    {
        
    }

	void OnDestroy()
	{
		m_gameManager.OnGamePausedEvent -= Pause;
        m_gameManager.OnGameEndedEvent -= ForceStop;
    }
}