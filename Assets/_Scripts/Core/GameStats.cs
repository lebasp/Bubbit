using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameStats : MonoBehaviour 
{
    #region STATICS
    GameManager m_gameManager { get { return GameManager.GetInstance(); } }
    #endregion

    static GameStats s_instance;
	public static GameStats GetInstance(){ return s_instance; }

    int m_totalPointsToSpawnBubble;
    int m_currentPointsToSpawnBubble = 0;

	public delegate void OnScoreChanged(int score);
	public OnScoreChanged onScoreChanged;

    public delegate void OnScoreSpawnChanged(int currentScore, int necessaryScore);
    public OnScoreSpawnChanged onScoreSpawnChanged;

    public delegate void OnSpawnBarFilled();
    public OnSpawnBarFilled onSpawnBarFilled;
  

	public int Score
    {
        get;
        private set;
	}

	public int HighestCircle
	{
		get;
		private set;
	}

	void Awake () 
	{
		if(!s_instance) s_instance = this;
    }

    void Start()
    {
        m_gameManager.onSetupGame += SetupSpecialPowerUp;
    }

    public void SetupSpecialPowerUp(GameManager.kind gameKind)
    {
        if (gameKind == GameManager.kind.Survival)
        {
            m_totalPointsToSpawnBubble = 80;
        }
        else if (gameKind == GameManager.kind.Timed)
        {          
            m_totalPointsToSpawnBubble = 50;
        }
    }

	public void AddScore(int points)
	{
		Score += points;		

		if(HighestCircle < points)
		{
			HighestCircle = points;			
		}

		if (onScoreChanged != null)
			onScoreChanged (Score);


        m_currentPointsToSpawnBubble += points;
        if (m_currentPointsToSpawnBubble >= m_totalPointsToSpawnBubble)
        {
            m_currentPointsToSpawnBubble = 0;

            if (onSpawnBarFilled != null)
                onSpawnBarFilled();            
        }

        if (onScoreSpawnChanged != null)
            onScoreSpawnChanged(m_currentPointsToSpawnBubble, m_totalPointsToSpawnBubble);
    }


    public void ResetStats()
    {
        Score = 0;
        HighestCircle = 0;
        m_currentPointsToSpawnBubble = 0;

		if (onScoreChanged != null) 
			onScoreChanged (Score);

        if (onScoreSpawnChanged != null)
            onScoreSpawnChanged(m_currentPointsToSpawnBubble, m_totalPointsToSpawnBubble);
    }

    void OnDestroy()
    {
        m_gameManager.onSetupGame -= SetupSpecialPowerUp;
    }

    public string ScoreRange()
    {
        int division = ((int)(Score / 50))*50;

        return (division.ToString()) + "-" + (division + 50).ToString();
    }
}
