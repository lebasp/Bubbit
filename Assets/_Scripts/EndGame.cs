using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using System;

public class EndGame : MonoBehaviour
{
    #region STATICS
    GameManager m_gameManager { get { return GameManager.GetInstance(); } }
    GameStats m_gameStats { get { return GameStats.GetInstance(); } }
    Inventory m_inventory { get { return Inventory.GetInstance(); } }
    FacebookPlugin m_facebookPlugin { get { return FacebookPlugin.GetInstance(); } }
    OneSignalPlugin m_oneSignalPlugin { get { return OneSignalPlugin.GetInstance(); } }
    LeaderboardController m_leaderboardController {get { return LeaderboardController.GetInstance(); } }
    TransitionEffect m_transitionEffect { get { return TransitionEffect.GetInstance(); } }
    GoogleGamePlugin m_googleGamePlugin { get { return  GoogleGamePlugin.GetInstance(); } }
    #endregion

    [SerializeField]
    GameObject m_endGameGroup;



    [SerializeField]
    Image m_blockScreen;

    [SerializeField]
    Transform m_contentArea;

    [SerializeField]
    GameObject m_rewardVideoButton;

    [SerializeField]
    GameObject m_loading;

    [SerializeField]
    GameObject m_facebookGroup;

    [SerializeField]
    LeaderboardView m_leaderboard;

    [SerializeField]
    TakeScreenshot m_takeScreenshot;
    
    [SerializeField]
    GameObject m_loadingCanvas;

    [SerializeField]
    Text m_gameModeText;

    [SerializeField]
    Text m_score;

    [SerializeField]
    Text m_receivedCoinsText;

    [SerializeField]
    Text m_totalCoinsText;


    Vector2 m_defaultContentPosition;

    Sequence m_fadeInSequence;
    Sequence m_fadeOutSequence;

    int m_numberOfRetries = 0;

    void Awake()
    {
        m_defaultContentPosition = m_contentArea.localPosition;

        m_fadeInSequence = DOTween.Sequence();
        m_fadeInSequence.Append(m_contentArea.DOLocalMove(Vector2.zero, 0.5f, true).SetEase(Ease.InCirc).OnStepComplete(() =>
            {
                StartCoroutine( ShowEndGameAnimationEnded() );
            }));
        m_fadeInSequence.Join(m_blockScreen.DOFade(0.79f, 0.2f).SetEase(Ease.InCirc));
        m_fadeInSequence.Pause();
        m_fadeInSequence.SetAutoKill(false);


        m_fadeOutSequence = DOTween.Sequence();
        m_fadeOutSequence.Append( DOTween.To(() => Vector2.zero, (newPos) => m_contentArea.localPosition = newPos, m_defaultContentPosition, 0.5f).SetEase(Ease.OutCirc).OnComplete(() =>
            {
                HardHideGame();
            }));       
        m_fadeOutSequence.Join(m_blockScreen.DOFade(0, 0.2f).SetEase(Ease.OutCirc));
        m_fadeOutSequence.Pause();
        m_fadeOutSequence.SetAutoKill(false);            
    }

    public void ShowEndGame()
    {
        CheckAchievements();

        m_numberOfRetries++;

        m_rewardVideoButton.SetActive(false);

        m_endGameGroup.SetActive(true);

        var score = m_gameStats.Score;

        m_score.text = score.ToString();

        if (m_gameModeText != null)
            m_gameModeText.text = m_gameManager.Kind == GameManager.kind.Survival ? Localization.CurrentLanguage.GetString("SURVIVAL_GAME") : Localization.CurrentLanguage.GetString("TIMED_GAME");

        var coins = 0;
        if (score > 0)
        {
            coins = Mathf.Min(score - 1, (int)(score * (Mathf.Pow(score, -0.5f) + 0.6f)));
        }

        if (m_totalCoinsText != null)
            m_totalCoinsText.text = (m_inventory.ItemBalance(Inventory.ID_COINS) + coins).ToString();
        if (m_receivedCoinsText != null)
            m_receivedCoinsText.text = "+" + coins.ToString();

        m_inventory.GiveItem(Inventory.ID_COINS, coins);

        ShowLoading();

        m_fadeInSequence.Rewind();
        m_fadeInSequence.Play();              
    }

    IEnumerator ShowEndGameAnimationEnded()
    {   
        yield return null;

        AppodealPlugin.GetInstance().CheckRewardVideo(this.addAction(() =>
            {
                m_rewardVideoButton.SetActive(true);
            }));

        m_facebookPlugin.CheckLogin( this.addAction<bool>( (logged) => FacebookLoginCallback(logged)));      

        if (m_numberOfRetries >= 2)
        {           
            AppodealPlugin.GetInstance().ShowInterstitial(this.addAction(() => m_numberOfRetries = 0));         
        }

        AppodealPlugin.GetInstance().ShowBanner(false);

        m_googleGamePlugin.PostLeaderboard(m_gameManager.Kind, m_gameStats.Score);

    }

    void CheckAchievements()
    {
        var score = m_gameStats.Score;

        if (m_gameManager.Kind == GameManager.kind.Survival)
            m_googleGamePlugin.PostAchievement(GooglePlayIds.achievement_survival_bubbit);
        else
            m_googleGamePlugin.PostAchievement(GooglePlayIds.achievement_timed_bubbit);

        if (score == 123)
            m_googleGamePlugin.PostAchievement(GooglePlayIds.achievement_1_2_3_score);

        if (score >= 50)
            m_googleGamePlugin.PostAchievement(GooglePlayIds.achievement_newbie);
        
        if (score >= 150)
            m_googleGamePlugin.PostAchievement(GooglePlayIds.achievement_professional);

        if (score >= 300)
            m_googleGamePlugin.PostAchievement(GooglePlayIds.achievement_expert);
        
        if (score >= 450)
            m_googleGamePlugin.PostAchievement(GooglePlayIds.achievement_master);
        
        if (score >= 600)
            m_googleGamePlugin.PostAchievement(GooglePlayIds.achievement_asian);

    }

    public void HardHideGame()
    {
        m_endGameGroup.SetActive(false);
    }

    public void HideEndGame()
    {
        m_fadeOutSequence.Rewind();
        m_fadeOutSequence.Play();
    }

    public void LoginFacebook()
    {
        ShowLoading();

        m_facebookPlugin.Login(this.addAction<bool>((logged) =>
                {
                    if (logged)
                        m_leaderboardController.ResetLocalScores();
                
                    FacebookLoginCallback(logged);
                }));
    }   

    void FacebookLoginCallback(bool logged)
    {
        StartCoroutine(StopRecording());

        if (logged)
        {
            m_facebookGroup.SetActive(false); 

            m_leaderboardController.GetLeaderboard(m_gameManager.Kind, this.addAction<Leaderboard>((leaderboards) =>
                    {
                        m_loading.SetActive(false);
                        ComputeScore(leaderboards);
                    }));

        }
        else
        {
            m_loading.SetActive(false);
            m_facebookGroup.SetActive(true);
            m_leaderboard.HideLeaderboard();
        }
    }

    IEnumerator StopRecording()
    {
        yield return new WaitForSeconds(1);
    }

    void ShowLoading()
    {
        m_loading.SetActive(true);
        m_facebookGroup.SetActive(false);          
        m_leaderboard.HideLeaderboard();
    }

    void ComputeScore(Leaderboard leaderboards)
    {
        var gameKind = m_gameManager.Kind;
        var score = m_gameStats.Score;          

        if (m_leaderboardController.HigherThanLocalScore(gameKind, score))
        {
            m_leaderboardController.SendHighscore(gameKind, m_facebookPlugin.UserId, score);

            var friendsLb = leaderboards.FriendsLeaderboard;
            var worldLB = leaderboards.WorldLeaderboard;

            HigherThanFriend(friendsLb, score);
            FirstOnScores(worldLB, score);

            m_leaderboardController.UpdatePlayerScore(gameKind, score);

            m_leaderboard.ShowLeaderboard(gameKind, leaderboards);
        }
        else
            m_leaderboard.ShowLeaderboard(gameKind, leaderboards);
    }

    void HigherThanFriend(List<Score> friends_lb, int localScore)
    {
        if (friends_lb.Count > 0)
        {
            Score found = null;

            for (int i = 0; i < friends_lb.Count; i++)
            {
                if (friends_lb[i].UserName == m_facebookPlugin.UserId)
                    break;

                if (localScore > friends_lb[i].Value)
                {
                    found = friends_lb[i];
                    break;
                }
            }

            if (found != null)
            {
                AskNotificationPopup(found.UserName);             
            }
        }
    }

    void FirstOnScores(List<Score> leaderboard, int localScore)
    {
        var gameKind = m_gameManager.Kind;

        if (leaderboard.Count > 0)
        {
            if (!leaderboard[0].UserName.Equals(m_facebookPlugin.UserId) && localScore > leaderboard[0].Value)
            {
                var message = new Dictionary<string, string>()
                {
                    { "en",string.Format(Localization.English.GetString("REMOVED_FROM_TOP"), gameKind) },
                    { "pt",string.Format(Localization.Portuguese.GetString("REMOVED_FROM_TOP"), gameKind) }
                };

                m_oneSignalPlugin.SendNotification(leaderboard[0].UserName, message);                      
            }
        }
    }

    void AskNotificationPopup(string fbId)
    {
        var gameKind = m_gameManager.Kind;

        m_facebookPlugin.GetUserName(fbId, this.addAction<string>((userName) =>
            {
                var colorName = "<color=#00ff00ff>" + userName + "</color>";

                var askMsg = Localization.CurrentLanguage.GetString("ASK_SEND_NOTIFICATION_FRIEND");
                askMsg = string.Format(askMsg, colorName);

                PopupController.GetInstance().Show(askMsg, () =>
                    {
                        RateMyApp.IncreasePlayerValue();

                        m_facebookPlugin.GetUserName(m_facebookPlugin.UserId, this.addAction<string>((myName) =>
                            {
                                var message = new Dictionary<string, string>()
                                {
                                    { "en",string.Format(Localization.English.GetString("FB_FRIEND_GOT_YOUR_SCORE"), myName, gameKind) },
                                    { "pt",string.Format(Localization.Portuguese.GetString("FB_FRIEND_GOT_YOUR_SCORE"), myName, gameKind) }
                                };

                                m_oneSignalPlugin.SendNotification(fbId, message);
                            }));
                    }, ()=>{ /*dont send message*/ });

            }));
    }
   

    public void RetryButton()
    {
        UnityEngine.Analytics.Analytics.CustomEvent("EndGame", new Dictionary<string, object>
            {
                { "navigation.button", "retry" },
            });

        HideEndGame();

        m_gameManager.StartGame(m_gameManager.Kind);       
    }

    public void MenuButton()
    {
        UnityEngine.Analytics.Analytics.CustomEvent("EndGame", new Dictionary<string, object>
            {
                { "navigation.button", "menu" },
            });

        m_transitionEffect.Blackout(() =>
            {
                m_gameManager.ReturnMenu();
            }
        );
    }

    public void TakeScreenshot()
    {
        var gameModeText = m_gameManager.Kind == GameManager.kind.Survival ? Localization.CurrentLanguage.GetString("SURVIVAL_GAME") : Localization.CurrentLanguage.GetString("TIMED_GAME");
        var text = string.Format(Localization.CurrentLanguage.GetString("SCREENSHOT_TEXT"), m_gameStats.Score, gameModeText);

        m_takeScreenshot.Screenshot(text, this.addAction(() =>
                {
                    RateMyApp.IncreasePlayerValue();

                    if (m_gameStats.Score > 10)
                        RateMyApp.ShowRateMyapp();
                }));
    }


    public void WatchVideo()
    {
        // Premium version: no rewarded video ads
    }


}
