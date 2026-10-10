using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GameTimer : MonoBehaviour
{
    #region STATICS
    GameManager m_gameManager { get { return GameManager.GetInstance(); } }
    #endregion

    [SerializeField]
    Text timerText;

    [SerializeField]
    PulsingAnimation m_timePulsing;

    bool m_pause = false;

    TimeSpan m_timeUp;

    int m_timeToPulse = 10;

    void Start()
    {
        m_gameManager.OnGamePausedEvent += Pause;
    }

    public void StartTimer()
    {
        m_timeUp= new TimeSpan(0, 1, 0);

        StartCoroutine(TimerRoutine());
    }

    public void  StopTimer()
    {
        StopAllCoroutines();       
        StopPulsing();
    }

    void Pause(bool pause)
    {
        m_pause = pause;
    }

    public void AddTime(int seconds)
    {
        if(m_timeUp.TotalSeconds <= 1)
            GoogleGamePlugin.GetInstance().PostAchievement(GooglePlayIds.achievement_cheating_time);
            

        m_timeUp += new TimeSpan(0,0, seconds);

        if (m_timeUp.TotalSeconds > m_timeToPulse)
            StopPulsing();
    }

    public void RestartTimer()
    {
        StartCoroutine(TimerRoutine());
    }

    IEnumerator TimerRoutine()
    {
        timerText.text = Mathf.Max((int)m_timeUp.TotalSeconds, 0).ToString() + "\n" + Localization.CurrentLanguage.GetString("TIME");

        while (m_timeUp.TotalSeconds > 0)
        {
			if (!m_pause)
			{ 
                if (m_timeUp.TotalSeconds <= m_timeToPulse)
                    StartPulsing();

				var deltaTime = new TimeSpan (0, 0, 0, 0, (int)(1000 * Time.fixedDeltaTime));

                m_timeUp -= deltaTime;

                timerText.text = Mathf.Max ((int)m_timeUp.TotalSeconds, 0).ToString () + "\n" + Localization.CurrentLanguage.GetString("TIME");

			}

			yield return new WaitForFixedUpdate ();
        }

       
        m_gameManager.TimeEnded();
    }

    void StartPulsing()
    {
        timerText.color = Color.red;
        m_timePulsing.Pulse();
    }

    void StopPulsing()
    {
        timerText.color = Color.white;
        m_timePulsing.StopPulse();
    }

    public void OnDestroy()
    {
        m_gameManager.OnGamePausedEvent -= Pause;
    }

}
