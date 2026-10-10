using UnityEngine;
using System.Collections;
//[MIGRATION] TODO: NEWWER VERSION
//using Facebook;
using UnityEngine.UI;
using System.Collections.Generic;

public class Menu : MonoBehaviour 
{

	#region STATICS
    TransitionEffect m_transitionEffect { get { return TransitionEffect.GetInstance(); } }
    #endregion

    [SerializeField]
    GameObject m_mainMenu;

	[SerializeField]
	GameObject m_loadingScreen;

    [SerializeField]
    TutorialView m_tutorial;

    [SerializeField]
    Options m_options;

    [SerializeField]
    LeaderboardMode m_leaderboardMode;

    [SerializeField]
    Store m_store;

    [SerializeField]
    GameCenterView m_gc;

    [SerializeField]
    Customize m_customize;

    void Start()
    {
       // AppodealPlugin.GetInstance().CheckRewardVideo(this.addAction(()=>{}));       

       // MainMenu();

       // yield return null;


        if (!SecureData.Get(SaveKeys.SK_TUTORIAL_WATCHED, false))
        {
            m_tutorial.ShowTutorial();
            SecureData.Save(SaveKeys.SK_TUTORIAL_WATCHED, true);
        }

        m_transitionEffect.Dismiss(this.addAction(() =>
                {
                    if (SecureData.Get(SaveKeys.SK_MENU_FROM_GAME, false))
                    {
                        AppodealPlugin.GetInstance().ShowInterstitial(this.addAction(() =>
                                {
                                }));
                        SecureData.Save(SaveKeys.SK_MENU_FROM_GAME, false);
                    }
                    else
                        RateMyApp.IncreasePlayerValue();

                    AppodealPlugin.GetInstance().ShowBanner(false);

                }));       
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) { MainMenu(); }
    }

    public void MainMenu()
    {  
        m_mainMenu.SetActive(true);
        DismissAll();
    }

    public void DismissAll()
    {
        m_tutorial.HideTutorial();
        m_options.CloseOptions();
        m_leaderboardMode.HideLeaderboards();
        m_store.Dismiss();
        m_gc.Hide();
        m_customize.Dismiss();
    }

    public void TimedGame()
    {
        m_transitionEffect.Blackout(() => SceneController.LoadGameplay(GameManager.kind.Timed));
    }

	public void SurvivalGame()
	{
        m_transitionEffect.Blackout(() => SceneController.LoadGameplay(GameManager.kind.Survival));
	}

    public void Leaderboards()
    {
        m_leaderboardMode.ShowLeaderboardMode();
    }

    public void Options()
    {
        m_options.ShowOptions();
    }

    public void Tutorial()
    {
        m_tutorial.ShowTutorial();
    }

    public void ShowLoadingScreen()
    {
        m_loadingScreen.SetActive (true);
    }

    public void DismissLoadingScreen()
    {
        m_loadingScreen.SetActive (false);
    }

    public void FacebookInvite()
    {
        FacebookPlugin.GetInstance().FacebookInvite();
    }

    public void Store()
    {
        m_store.Show();
    }

    public void Customize()
    {
        m_customize.Show();
    }


    public void GC()
    {
        GoogleGamePlugin.GetInstance().Login((success) =>
            {
				#if UNITY_ANDROID
                if(success)
				#endif
                    m_gc.Show();                
            });

    }

    public void ShowFacebookPage()
    {
        PopupController.GetInstance().Show(Localization.CurrentLanguage.GetString("EXTERNAL_LINK"), () =>
        {
            Application.OpenURL("http://facebook.com/bubbitgame");
        }, () => { });

    }
}
