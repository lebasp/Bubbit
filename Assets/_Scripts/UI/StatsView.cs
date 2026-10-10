using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatsView : MonoBehaviour 
{
	#region STATICS
    GameManager m_gameManager { get { return GameManager.GetInstance(); } }
	GameStats m_gameStats { get { return GameStats.GetInstance(); } }
	HealthManager m_healthManager { get { return HealthManager.GetInstance(); } }
    DifficultyManager m_difficultyManager { get { return DifficultyManager.GetInstance(); } }
	#endregion

	[SerializeField]
	Text m_score;

	[SerializeField]
	Sprite m_fullHeart;

	[SerializeField]
	Sprite m_emptyHeart;

	[SerializeField]
	Image[] m_hearts;

    [SerializeField]
    FilledTiledBar m_heartSpawnBar;

    [SerializeField]
    Text m_levelLabel;

    [SerializeField]
    GameObject m_timerGroup;

    [SerializeField]
    GameObject m_levelGroup;

    [SerializeField]
    GameObject m_timedPowerUpImage;

    [SerializeField]
    GameObject m_heartPowerUpImage;

    [SerializeField]
    FlashAnimation m_scoreFlashAnimation;

    [SerializeField]
    FlashAnimation m_barFlashAnimation;

    [SerializeField]
    Image m_bubbleSprite;

	void Start()
	{
		m_gameStats.onScoreChanged += UpdateScore;
        m_gameStats.onScoreSpawnChanged += UpdateSpawnBar;
        m_healthManager.onLifeChanged += UpdateLives;
        m_difficultyManager.onLevelChanged += UpdateLevelLabel;
        m_gameManager.onSetupGame += SetupView;
	}

    void SetupView(GameManager.kind gameKind)
    {       
        m_bubbleSprite.sprite = SkinController.GetInstance().GetSprite(SkinController.spriteKind.Normal);

        UpdateCenter(gameKind);
    }

    void UpdateCenter(GameManager.kind gameKind)
    {
        if (gameKind == GameManager.kind.Survival)
        {
            m_timerGroup.SetActive(false);
            m_levelGroup.SetActive(true);
            m_heartPowerUpImage.SetActive(true);
            m_timedPowerUpImage.SetActive(false);
        }
        else if (gameKind == GameManager.kind.Timed)
        {
            m_timerGroup.SetActive(true);
            m_levelGroup.SetActive(false);
            m_heartPowerUpImage.SetActive(false);
            m_timedPowerUpImage.SetActive(true);
        }
    }

	void UpdateScore(int score)
	{
        m_scoreFlashAnimation.Flash();
		m_score.text = score.ToString();
	}

	void UpdateLives(int lives)
	{
		for (int i = 0; i <= m_hearts.Length - 1; i++)
			if (i <= lives - 1) 
			{
				m_hearts [i].sprite = m_fullHeart;
			} else 
			{
				m_hearts [i].sprite = m_emptyHeart;
			}
	}

    void UpdateSpawnBar(int currentPoints, int totalPoints)
    {
        m_barFlashAnimation.Flash();
        m_heartSpawnBar.Amount = currentPoints / (float)totalPoints;
    }

    void UpdateLevelLabel(int level)
    {
        m_levelLabel.text = level.ToString();
    }
    
	void OnDestroy()
	{
		m_gameStats.onScoreChanged -= UpdateScore;
        m_gameStats.onScoreSpawnChanged -= UpdateSpawnBar;
        m_healthManager.onLifeChanged -= UpdateLives;
        m_difficultyManager.onLevelChanged -= UpdateLevelLabel;
        m_gameManager.onSetupGame -= SetupView;
	}

}
