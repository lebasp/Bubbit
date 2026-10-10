using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : MonoBehaviour 
{
	#region STATICS
	CircleSpawner m_circleSpawner {get { return CircleSpawner.GetInstance(); } }
    GameStats m_gameStats { get { return GameStats.GetInstance(); } }
    DifficultyManager m_difficultyManager { get { return DifficultyManager.GetInstance(); } }
    BackgroundController m_backgroundController { get { return BackgroundController.GetInstance(); } }
	HealthManager m_healthManager { get { return HealthManager.GetInstance (); } }
    SoundManager m_soundManager { get { return SoundManager.GetInstance(); } }
    CanvasSettings m_canvasSettings {get { return CanvasSettings.GetInstance(); } }
    LeaderboardController m_leaderboardController { get { return LeaderboardController.GetInstance(); } }
    TransitionEffect m_transitionEffect { get { return TransitionEffect.GetInstance(); } }
    #endregion

    public enum kind
	{
		Survival,
		Timed
	}
    
	kind m_gamekind;

	public kind Kind{ get{ return m_gamekind; } }
    
	float m_speedModifier = 1;
	public float SpeedModifier
	{
		get { return m_speedModifier; }
		set 
		{
			m_speedModifier = value;

            //m_backgroundController.ModifySpeed(m_speedModifier);

            m_soundManager.SetPitch(m_speedModifier);
        }
	}
	
    [SerializeField]
	GameObject m_gameplayGroup;

    [SerializeField]
    Pause m_pause;

    [SerializeField]
    EndGame m_endGame;

    [SerializeField]
    PopupAskContinue m_askContinue;

    [SerializeField]
    AudioSource m_explosionSource;

    [SerializeField]
    GameTimer m_gametimer;	
    public GameTimer Gametimer{ get { return m_gametimer; } }

    bool m_pauseBool = false;
    bool m_recording = false;

    public bool GamePaused { get { return m_pauseBool; } }

    static GameManager s_instance;
	public static GameManager GetInstance(){ return s_instance; }   
  
    int m_timesAsked = 0;

    public delegate void onGamePaused(bool paused);
    public event onGamePaused OnGamePausedEvent;

    public delegate void onGameEnded();
    public event onGameEnded OnGameEndedEvent;

    public delegate void OnSetupGame(GameManager.kind gameKind);
    public event OnSetupGame onSetupGame;

    bool m_gameStarted = false;

	void Awake () 
	{
		if(!s_instance) s_instance = this;
	}

	IEnumerator Start()
    {
		m_healthManager.onLifeChanged += LifeChanged;

		yield return null;

        m_endGame.HardHideGame();

        m_transitionEffect.Dismiss();

        StartGame(SceneController.GameKind);
    }

    public void ReturnMenu()
    {
       // EveryplayPlugin.GetInstance().StopRecording();

        StopGame();

        m_gameplayGroup.SetActive(false);

        SecureData.Save(SaveKeys.SK_MENU_FROM_GAME, true);

        SceneController.LoadMenu();
    }

    public void StartGame(kind gameKind)
	{
        if (m_gameStarted)
            return;

       // EveryplayPlugin.GetInstance().ResetRecording();

        m_gameStarted = true;

        m_gamekind = gameKind;

        FacebookPlugin.GetInstance().CheckLogin(this.addAction<bool>((logged) =>
                {
                    if (logged)
                    {
                        m_leaderboardController.ResetLeaderboards();
                        m_leaderboardController.GetLeaderboard(m_gamekind, null);
                    }
                }));

        AppodealPlugin.GetInstance().HideBanner();

        m_gameplayGroup.SetActive(true);        

        m_gameStats.ResetStats();

		m_healthManager.ResetLives ();

        m_difficultyManager.Begin();

        PauseGame(false);

        m_timesAsked = 0;

        m_circleSpawner.StartSpawn();

        if (onSetupGame != null)
            onSetupGame(m_gamekind);

        if (Kind == kind.Timed)
            m_gametimer.StartTimer();
    }

    public void StopGame()
    {
        m_gameStarted = false;       

        m_circleSpawner.StopSpawn();

        m_difficultyManager.End();

        if (Kind == kind.Timed)
            m_gametimer.StopTimer();

        StopAllCoroutines();

        if (OnGameEndedEvent != null)
            OnGameEndedEvent();
    }

    // Premium version: reviving is only paid with coins (no rewarded ads), so the
    // offer only shows up when the player can afford the next revive
    bool CanAskContinue()
    {
        return Inventory.GetInstance().ItemBalance(Inventory.ID_COINS) >= PopupAskContinue.RevivePrice(m_timesAsked + 1);
    }

    public void TimeEnded()
    {
        if (CanAskContinue())
        {
            m_timesAsked++;

            PauseGame(true);

            m_askContinue.ShowAskContinue(() =>
                {
                    m_gametimer.AddTime(30);
                    m_gametimer.RestartTimer();
                    ReviveGame();

                }, () => EndGame(), true, m_timesAsked);
        }
        else
            EndGame();
    }

    public void LivesEnded()
    {     
        if (CanAskContinue())
        {
            m_timesAsked++;

            PauseGame(true);

            m_askContinue.ShowAskContinue(() =>
                {
                    m_healthManager.AddLife();
                    m_healthManager.AddLife();
                    m_healthManager.AddLife();
                    ReviveGame();

                }, () => EndGame(), false, m_timesAsked);
        }
        else
            EndGame();
    }

    public void ReviveGame()
    {
        GoogleGamePlugin.GetInstance().PostAchievement(GooglePlayIds.achievement_not_dead_yet);

        m_explosionSource.Play();
        m_backgroundController.Flash();
        m_circleSpawner.ClearActiveCircles();
        PauseGame(false);
    }

    public void EndGame()
    {
        var metadata = new Dictionary<string, object>
        {
            { "gamekind", m_gamekind.ToString() },
            { "score", m_gameStats.Score },
            { "score_range", m_gameStats.ScoreRange() },
            { "level", m_difficultyManager.CurrentLevel.ToString() },
        };

        UnityEngine.Analytics.Analytics.CustomEvent("GameOver",metadata);

       // EveryplayPlugin.GetInstance().SetMetaData(metadata);
       
        StopGame();       

        m_endGame.ShowEndGame();
    }

    #if UNITY_EDITOR
    void Update()
    {
        if (m_gameStarted)
            if (Input.GetKeyDown(KeyCode.Escape))
                PauseButton();
    }
    #endif
    
    /*
    public void StartRecording()
    {
        if (!EveryplayPlugin.GetInstance().IsRecording())
        {
            m_recording = true;
            EveryplayPlugin.GetInstance().StartRecording();
        }
        else
        {
            m_recording = !m_recording;
            EveryplayPlugin.GetInstance().Pause(m_recording);
        }
    }
    */

	public void PauseButton()
	{
        PauseGame(!m_pauseBool);

        if (m_pauseBool)
            m_pause.ShowPause();
        else
            m_pause.DismissPause();
    }

    void PauseGame(bool pause)
    {
        ConnectionManager.GetInstance().FreeConnection();

        m_pauseBool = pause;

        if(OnGamePausedEvent != null)
        {
            OnGamePausedEvent(m_pauseBool);
        }
    }

	void LifeChanged(int lives)
	{
		if (lives <= 0)
            LivesEnded ();
	}

    public void Explode()
    {
        if (m_circleSpawner.CheckIfPowerUpActive(typeof(BlackHole)))
            return;

        m_explosionSource.Play();

        m_backgroundController.Flash();

        var circles = m_circleSpawner.Circles;        
        Circle powerUpCircle = null;

        if (circles.Count >= 7)
            GoogleGamePlugin.GetInstance().PostAchievement(GooglePlayIds.achievement_boom);

        for (int i = circles.Count - 1; i >= 0; i--)
        {
            if (circles[i].PowerUp != null)
            {
                powerUpCircle = circles[i];
                continue;
            }

            m_gameStats.AddScore(circles[i].Points);
            FloatingScoreController.GetInstance().ShowPoints(m_canvasSettings.Convert(circles[i].Position), circles[i].Points);
            circles[i].KillCircle();
        }

        //powerup circle must be the last one to explode, so the power up effect doesn't affect the ones to be exploded
        if (powerUpCircle)
        {
            powerUpCircle.UsePowerUp();
            powerUpCircle.KillCircle();
        }
        
    }

	public void OnDestroy()
	{
		m_healthManager.onLifeChanged -= LifeChanged;
	}




}
