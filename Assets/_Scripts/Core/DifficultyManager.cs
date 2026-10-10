using UnityEngine;
using System.Collections;
using System;

public class DifficultyManager : MonoBehaviour
{
    #region STATICS
    protected GameManager m_gameManager { get { return GameManager.GetInstance(); } }
    #endregion

    static DifficultyManager s_instance;
    public static DifficultyManager GetInstance() { return s_instance; }

    public int LevelMSTickWait { get { return 8; } }

    //control spawn time
    int DefaultMillisecondsBetweenSpawns { get { return 2500; } }
    int MinMillisecondsBetweenSpawns { get { return 1200; } }
    public int SpawnMSPerTick { get { return 50; } }

    //control power up
    public int SecondsBeforeStartingPowerUps { get { return 5; } }

    //controle death circles
	public int SecondsBeforeStartingDeathCircles { get { return 5; } }
    float DefaultChanceOfDeathCircle { get { return 5; } }
    float MaxDeathCircleChance { get { return 15; } }
    public float DeathPerTick { get { return 1f; } }

    //control game speed
    float DefaultOverallGameSpeed = 0.9f;
    public float SpeedPerTick { get { return 0.15f; } }

    public int CurrentLevel{ get { return m_currentLevel; } }

    //temporary values
    int m_millisecondsBetweenSpawns;
    int m_currentLevel;

    bool m_pause = false;

    public delegate void OnLevelChanged(int level);
    public OnLevelChanged onLevelChanged;

    public int MillisecondsBetweenSpawns
    {
        get { return m_millisecondsBetweenSpawns; }
        private set { m_millisecondsBetweenSpawns = value; }
    }

    float m_chanceOfDeathCircle;
    public float ChanceOfDeathCircle
    {
        get { return m_chanceOfDeathCircle; }
        private set { m_chanceOfDeathCircle = value; }
    }

    float m_overallGameSpeed;
    public float OverallGameSpeed
    {
        get { return m_overallGameSpeed; }
        private set { m_overallGameSpeed = value; }
    }

    void Awake()
    {
        if (!s_instance) s_instance = this;
    }

    void Start()
    {
        m_gameManager.OnGamePausedEvent += Pause;
    }

    public void Begin()
    { 
        MillisecondsBetweenSpawns = DefaultMillisecondsBetweenSpawns;
        ChanceOfDeathCircle = DefaultChanceOfDeathCircle;
        OverallGameSpeed = DefaultOverallGameSpeed;

        m_currentLevel = 1;

        if (onLevelChanged != null)
            onLevelChanged(m_currentLevel);       
        
        StartCoroutine(IncreaseDifficulty());
    }

    public void End()
    {
        StopAllCoroutines();
    }

    IEnumerator IncreaseDifficulty()
    {
		var timeNow = new TimeSpan(0);
		var nextLevelTime = timeNow.Add( new TimeSpan (0, 0, 0, LevelMSTickWait));

        while (true)
        {
            if (!m_pause)
            {
				var deltaTime = new TimeSpan (0, 0, 0, 0, (int)(1000 * Time.fixedDeltaTime));
				timeNow = timeNow.Add( deltaTime );

                if (timeNow >= nextLevelTime)
                {
                    MillisecondsBetweenSpawns -= SpawnMSPerTick;
                    if (MillisecondsBetweenSpawns < MinMillisecondsBetweenSpawns)
                        MillisecondsBetweenSpawns = MinMillisecondsBetweenSpawns;

                    ChanceOfDeathCircle += DeathPerTick;
                    if (ChanceOfDeathCircle > MaxDeathCircleChance)
                        ChanceOfDeathCircle = MaxDeathCircleChance;

                    OverallGameSpeed += SpeedPerTick;

                    nextLevelTime = timeNow.Add( new TimeSpan (0, 0, 0, LevelMSTickWait));

                    m_currentLevel++;

                    if(m_gameManager.Kind == GameManager.kind.Timed && m_currentLevel >= 25)
                        GoogleGamePlugin.GetInstance().PostAchievement(GooglePlayIds.achievement_time_lord); 

                    if(m_gameManager.Kind == GameManager.kind.Survival && m_currentLevel >= 20)
                        GoogleGamePlugin.GetInstance().PostAchievement(GooglePlayIds.achievement_will_you_ever_die); 


                    if (onLevelChanged != null)
                        onLevelChanged(m_currentLevel);

                }
            }

			yield return new WaitForFixedUpdate ();;
        }
    }

    void Pause(bool paused)
    {
        m_pause = paused;
    }

    public float ChanceOfPowerUp(GameManager.kind gameKind)
    {
        if (gameKind == GameManager.kind.Timed)
            return 20;
        else
            return 20;  
    }

    public void OnDestroy()
    {
        m_gameManager.OnGamePausedEvent -= Pause;
    }

}
