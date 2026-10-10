using UnityEngine;
using System.Collections;
//[MIGRATION] TODO: NEWWER VERSION
//using UnityEngine.SocialPlatforms.GameCenter;


#if UNITY_ANDROID
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif
using UnityEngine.SocialPlatforms;
using System;

public class GoogleGamePlugin : MonoBehaviour
{
    static GoogleGamePlugin s_instance;
    public static GoogleGamePlugin GetInstance()
    {
        if (s_instance == null)
        {
            var go = new GameObject("GoogleGamePlugin");
            go.AddComponent<GoogleGamePlugin>();
        }
        return s_instance;
    }

    bool looged = false;

    void Awake()
    {

        if (s_instance == null)
        {
            s_instance = this;

            DontDestroyOnLoad(gameObject);

            //[MIGRATION] TODO: NEWWER VERSION
            /*
			#if UNITY_IOS
			GameCenterPlatform.ShowDefaultAchievementCompletionBanner (true);
			#endif

			#if UNITY_ANDROID
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();

            PlayGamesPlatform.InitializeInstance(config);

            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
			#endif
            if (!SecureData.Get(SaveKeys.SK_GC_DONT_AUTO_LOGIN, false))
            {
                Login((success) => 
                    {
                        SecureData.Save(SaveKeys.SK_GC_DONT_AUTO_LOGIN, !success);
                    });
            }
            */
        }
    }

    public void Login(Action<bool> doAfterInit)
    {
        doAfterInit(false);
        return;

        //[MIGRATION] TODO: NEWWER VERSION
        /*
        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool success) =>
				{                    
                    SecureData.Save(SaveKeys.SK_GC_DONT_AUTO_LOGIN, !success);
                    #if UNITY_ANDROID
                    doAfterInit(success);
                    #endif
                });

            #if UNITY_IOS
            doAfterInit(true);
            #endif            
        }
        else
            doAfterInit(true);
        */
    }

    public void PostAchievement(string id, float amount = 100.0f)
    {
        //[MIGRATION] TODO: NEWWER VERSION
        /*
        if (Social.localUser.authenticated)
        {

            Social.ReportProgress(id, amount, (bool success) =>
                {
                    // handle success or failure
                });
        }
        */
    }

    public void PostLeaderboard(GameManager.kind gameKind, int score)
    {
         //[MIGRATION] TODO: NEWWER VERSION
        /*
        if (Social.localUser.authenticated)
        {
            var leaderboard = gameKind == GameManager.kind.Survival ? GooglePlayIds.leaderboard_survival : GooglePlayIds.leaderboard_timed;

            Social.ReportScore(score, leaderboard, (bool success) =>
                {
                    // handle success or failure
                });
        } 
        */       
    }   

    public void ShowLb()
    {
        //[MIGRATION] TODO: NEWWER VERSION
        /*
        #if UNITY_ANDROID
        Login((success) =>
            {
                if (success)
        #endif
                
            Social.ShowLeaderboardUI();

        #if UNITY_ANDROID
            }
        );
        #endif
        */

    }

    public void ShowAchiv()
    {
        //[MIGRATION] TODO: NEWWER VERSION
        /*
        #if UNITY_ANDROID
        Login((success) =>
            {
                if (success)
        #endif

                    Social.ShowAchievementsUI();

                #if UNITY_ANDROID
            }
        );
        #endif
		*/
    }
}
