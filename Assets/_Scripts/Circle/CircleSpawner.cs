using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using TimeSpan = System.TimeSpan;

public class CircleSpawner : MonoBehaviour 
{
	#region STATICS
    GameManager m_gameManager {  get { return GameManager.GetInstance(); } }
    DifficultyManager m_difficultManager {  get { return DifficultyManager.GetInstance(); } }
    GameStats m_gameStats { get { return GameStats.GetInstance(); } }
	#endregion

    static CircleSpawner s_instance;
    public static CircleSpawner GetInstance(){ return s_instance; }

	[SerializeField]
    PowerUp[] m_powerUps;

    [SerializeField]
    PowerUp m_survivalSpecialPowerUp;

    [SerializeField]
    PowerUp m_timedSpecialPowerUp;

	List<Circle> m_circles = new List<Circle>();
	public List<Circle> Circles
	{
		get { return m_circles; }
	}

	[SerializeField]
	GameObject m_circlePrefab;

    [SerializeField]
    List<Circle> m_circlePreList;


    [SerializeField]
    GameObject m_deathCirclePrefab;

    [SerializeField]
    List<Circle> m_deathCirclePreList;

    [SerializeField]
	RectTransform m_spawnArea;

    [SerializeField]
    Transform m_powerUpParent;

    Pool<Circle> m_circlePool;
    Pool<Circle> m_deathCirclePool;
    Pool<Circle> m_lifeCirclePool;

    Vector2 m_spawnPointStart;
    Vector2 m_spawnPointEnd;

    PowerUp m_activePowerUp;

    bool m_specialPowerUpNext = false;

	Transform m_transform;

	bool m_paused = false;
    public bool Paused {  get { return m_paused; } }

    PowerUp SpecialPowerUp
    {
        get
        {
            if (m_gameManager.Kind == GameManager.kind.Timed)
                return m_timedSpecialPowerUp;
            else
                return m_survivalSpecialPowerUp;
        }
    }

	void Awake ()
    {
        if (!s_instance)
            s_instance = this;		
        m_transform = transform;

        m_circlePool = Pool<Circle>.CreatePool(m_circlePreList, m_circlePrefab, m_transform);
        m_deathCirclePool = Pool<Circle>.CreatePool(m_deathCirclePreList, m_deathCirclePrefab, m_transform);
    }

    void Start()
    {
        m_gameStats.onSpawnBarFilled += SetToSpawnLifeBubble;

        foreach (var pw in m_powerUps)
            pw.OnPowerUpReset += DisableActivePowerUp;

        m_gameManager.OnGamePausedEvent += Pause;
	}

    public bool CheckIfPowerUpActive(System.Type type)
    {
        var result = m_activePowerUp != null && m_activePowerUp.IsRunning && m_activePowerUp.GetType().Equals(type);
        return result;
    }

	public void StartSpawn()
	{
        var y = m_spawnArea.transform.localPosition.y + m_spawnArea.rect.yMax;

		m_spawnPointStart = new Vector2( m_spawnArea.rect.xMin, y);
		m_spawnPointEnd = new Vector2(m_spawnArea.rect.xMax,  y);

        m_paused = false;

        m_specialPowerUpNext = false;

        StartCoroutine(Spawner());
	}

	void Pause(bool pause)
	{
		m_paused = pause;
	}

    public void ClearActiveCircles()
    {
        for(int i=m_circles.Count -1; i >= 0; i--)
        {
            m_circles[i].KillCircle();
        }

        m_circles.Clear();
    }

	public void StopSpawn()
	{     

        StopAllCoroutines ();

        ClearActiveCircles();
	}

    void SetToSpawnLifeBubble()
    {
        m_specialPowerUpNext = true;
    }

	IEnumerator Spawner()
	{
		yield return null;

		var nexSpawnTime = new TimeSpan (0);
		var timeNow = new TimeSpan(0);

		var powerUpStart =  new TimeSpan(0,0,0, m_difficultManager.SecondsBeforeStartingPowerUps);
		var deathCircleStart =  new TimeSpan(0,0,0, m_difficultManager.SecondsBeforeStartingDeathCircles);

        int prevPowerUp = -1;

        float previousX = 0;

		while(true)
		{

            if (!Paused)
            {				
                var deltaTime = new TimeSpan(0, 0, 0, 0, (int)((1000 * Time.fixedDeltaTime) * (m_difficultManager.OverallGameSpeed * m_gameManager.SpeedModifier)));
                timeNow = timeNow.Add(deltaTime);

                if (timeNow >= nexSpawnTime)
                {
                    float x = 0;
                    do
                    {
                        x =  Random.Range(m_spawnPointStart.x, m_spawnPointEnd.x);
                    } while(x >= previousX -100 && x <= previousX + 100 ) ;

                    previousX = x;

                    var y = Random.Range(m_spawnPointStart.y, m_spawnPointEnd.y);
				
                    var spawnPosition = new Vector3(x, y);

                    var doublePoints = CheckIfPowerUpActive(typeof(DoublePoints));

                    var maySpawnDeathCircle = (timeNow >= deathCircleStart) && (Random.Range(0, 100) <= m_difficultManager.ChanceOfDeathCircle);
                    var maySpawnPoweUp = (timeNow >= powerUpStart) && (Random.Range(0, 100) <= m_difficultManager.ChanceOfPowerUp(m_gameManager.Kind)) && m_activePowerUp == null;

                    if (m_specialPowerUpNext)
                    {
                        SpawnCirclePowerUpAtLocation(spawnPosition, SpecialPowerUp);
                        m_specialPowerUpNext = false;
                    }
                    else if (maySpawnDeathCircle)
                        SpawnDeathCircleAtLocation(spawnPosition);
                    else if (maySpawnPoweUp)
                    {
                        var powerUpIndex = -1;
                        do
                        {
                            powerUpIndex = Random.Range(0, m_powerUps.Length);
                        } while(powerUpIndex == prevPowerUp);

                        prevPowerUp = powerUpIndex;

                        m_activePowerUp = m_powerUps[powerUpIndex];

                        SpawnCirclePowerUpAtLocation(spawnPosition, m_activePowerUp);
                    }
                    else
                        SpawnCircleAtLocation(spawnPosition, doublePoints ? 2 : 1);			

                    nexSpawnTime = timeNow.Add(new TimeSpan(0, 0, 0, 0, m_difficultManager.MilliecondsBetweenSpawns));
                }	

            }



			yield return new WaitForFixedUpdate();
		}
	}



    void DisableActivePowerUp()
    {
        m_activePowerUp = null;
    }

    public void SpawnCircleAtLocation(Vector2 spawnPosition, int points, bool shake = false)
	{
        var circle = SpawnCircle(m_circlePool, spawnPosition, points);

        if (shake)
            circle.Shake();
	}

	public void SpawnCirclePowerUpAtLocation(Vector2 spawnPosition, PowerUp powerUp)
	{
        SpawnCircle(m_circlePool, spawnPosition, 0).SetupPowerUp(powerUp);
	}

    public void SpawnDeathCircleAtLocation(Vector2 spawnPosition)
    {
        SpawnCircle (m_deathCirclePool, spawnPosition, 0);
	}

    Circle SpawnCircle(Pool<Circle> pool, Vector2 spawnPosition, int points)
	{
        var circle = pool.GetObject();

		m_circles.Add(circle);

        circle.Setup(points, spawnPosition);	

        var doublePoints = false;
        if (m_activePowerUp != null && m_activePowerUp.IsRunning && m_activePowerUp is DoublePoints)
            doublePoints = true;

		circle.SetDoublePointsEffect (doublePoints);

		return circle;
	}

	public void Remove(Circle circle)
	{
		if(m_circles.Contains(circle))
			m_circles.Remove(circle);

        // A circle only belongs to one pool; the other call is a no-op
        m_circlePool.DeactiveFromPool(circle);
        m_deathCirclePool.DeactiveFromPool(circle);
	}

    public void OnDestroy()
    {
        m_gameStats.onSpawnBarFilled -= SetToSpawnLifeBubble;

        foreach (var pw in m_powerUps)
            pw.OnPowerUpReset -= DisableActivePowerUp;

        m_gameManager.OnGamePausedEvent -= Pause;
    }
}
