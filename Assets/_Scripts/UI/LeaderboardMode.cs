using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LeaderboardMode : MonoBehaviour {

    #region STATICS
    FacebookPlugin m_facebookPlugin {get { return FacebookPlugin.GetInstance(); } }
    LeaderboardController m_leaderboardController {get { return LeaderboardController.GetInstance(); } }
    #endregion

    [SerializeField]
    GameObject m_leaderboardMode;

    [SerializeField]
    GameObject m_loginFacebookGroup;

    [SerializeField]
    GameObject m_loading;

    [SerializeField]
    GameObject m_buttonsGroup;

    [SerializeField]
    Text m_timedScore;

    [SerializeField]
    Text m_survivalScore;

    [SerializeField]
    LeaderboardView m_leaderboardView;

    public void ShowLeaderboardMode()
    {
        m_leaderboardMode.SetActive(true);

        ShowLoading();

        m_facebookPlugin.CheckLogin(this.addAction<bool>(DismissLoading));
    }

    public void HideLeaderboards()
    {
        m_leaderboardView.HideLeaderboard();
        HideLeaderboardMode();
    }

    public void HideLeaderboardMode()
    {
        m_leaderboardMode.SetActive(false);
    }

    void ShowLoading()
    {
        m_buttonsGroup.SetActive(false);
        m_loginFacebookGroup.SetActive(false);   

        m_loading.SetActive(true);
    }

    void DismissLoading(bool loggedIn)
    {        
        m_loginFacebookGroup.SetActive(false);  
        m_buttonsGroup.SetActive(false);

        if (!loggedIn)
        {
            m_loading.SetActive(false);
            m_loginFacebookGroup.SetActive(true);   
        }
        else
        {
            if(m_leaderboardMode.activeSelf)
                StartCoroutine(LoadScore());
        }
    }

    public void LoginFacebook()
    {
        ShowLoading();

        m_facebookPlugin.Login(this.addAction<bool>(DismissLoading));
    }

    IEnumerator LoadScore()
    {
        var routine1 = StartCoroutine(LoadSurvivalLB());
        var routine2 = StartCoroutine(LoadTimedLB());

        yield return routine1;
        yield return routine2;

        m_loading.SetActive(false);
        m_buttonsGroup.SetActive(true);
    }

    IEnumerator LoadSurvivalLB()
    {
        bool finished = false;

        m_leaderboardController.GetLeaderboard(GameManager.kind.Survival, this.addAction<Leaderboard>((lb) =>
                {
                    m_survivalScore.text = lb.PlayerScore.Value.ToString();

                    finished = true;

                }));

        while (!finished)
            yield return null;
    }


    IEnumerator LoadTimedLB()
    {
        bool finished = false;

        m_leaderboardController.GetLeaderboard(GameManager.kind.Timed, this.addAction<Leaderboard>((lb) =>
                {
                    m_timedScore.text = lb.PlayerScore.Value.ToString();

                    finished = true;
                }));
        
        while (!finished)
            yield return null;
    }

    public void TimedScoreButton()
    {
        m_leaderboardController.GetLeaderboard(GameManager.kind.Timed, this.addAction<Leaderboard>((lb) =>
                {
                    HideLeaderboardMode();

                    m_leaderboardView.ShowLeaderboard(GameManager.kind.Timed, lb);

                }));
    }

    public void SurvivalScoreButton()
    {
        m_leaderboardController.GetLeaderboard(GameManager.kind.Survival, this.addAction<Leaderboard>((lb) =>
            {
                HideLeaderboardMode();

                m_leaderboardView.ShowLeaderboard(GameManager.kind.Survival, lb);

            }));
    }
}
