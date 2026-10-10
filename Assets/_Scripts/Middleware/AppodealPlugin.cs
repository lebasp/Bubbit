using UnityEngine;
using System.Collections;
//[MIGRATION] TODO: NEWWER VERSION
//using AppodealAds.Unity.Api;
using System;
using System.Globalization;

public class AppodealPlugin : MonoBehaviour
{
    static AppodealPlugin s_instance;
    public static AppodealPlugin GetInstance()
    {
        if (s_instance == null)
        {
            var go = new GameObject("AppodealPlugin");
            go.AddComponent<AppodealPlugin>();
        }
        return s_instance;
    }

    //[MIGRATION] TODO: NEWWER VERSION
    /*
    Interstital m_interstital = new Interstital();
    RewardVideo m_rewardVideo = new RewardVideo();

    UserSettings m_userSettings;
    */

    bool m_rewardVideoEnabled = false;

    bool NoAds { get { return false; } }//(SoomlaPlugin.GetInstance().CheckItem(SoomlaPlugin.ID_REMOVE_ADS)); } }

	void Awake ()
    {
        if (s_instance == null)
        {
            s_instance = this;

            DontDestroyOnLoad(gameObject);

            //[MIGRATION] TODO: NEWWER VERSION
            /*
            m_userSettings = new UserSettings();

            #if UNITY_ANDROID
            var appKey = "";
            Appodeal.disableNetwork ( "inmobi");
            Appodeal.disableNetwork ( "unity_ads");
            Appodeal.disableNetwork ( "yandex");
            #elif UNITY_IPHONE
            var appKey = "";
            #else
            var appKey = "";
            #endif

            Appodeal.disableLocationPermissionCheck();
            Appodeal.initialize(appKey, Appodeal.BANNER | Appodeal.INTERSTITIAL | Appodeal.REWARDED_VIDEO);
            Appodeal.setAutoCache(Appodeal.INTERSTITIAL, false);
            Appodeal.setAutoCache(Appodeal.BANNER, !NoAds);
            Appodeal.setAutoCache(Appodeal.REWARDED_VIDEO, false);
            Appodeal.setInterstitialCallbacks(m_interstital);
            Appodeal.setRewardedVideoCallbacks(m_rewardVideo);

            FacebookPlugin.GetInstance().CheckLogin(this.addAction<bool>((logged) =>
                    {
                        if (logged)
                            RegisterUserdId(FacebookPlugin.GetInstance().UserId);
                        else
                            FacebookPlugin.GetInstance().OnUserIdSet += RegisterUserdId;
                    }));
            */
        }
    }

    public void RegisterUserdId(string userId)
    {
        //[MIGRATION] TODO: NEWWER VERSION
        /*
        m_userSettings.setFacebookId(userId);
        */
    }

    public void ShowInterstital (DisposableAction completeAction)
    {
        completeAction.Run();

        //[MIGRATION] TODO: NEWWER VERSION
        /*
        if (!NoAds)            
            m_interstital.ShowInterstital(completeAction);
        */
	}

    public void ShowBanner(bool bottom)
    {
        //[MIGRATION] TODO: NEWWER VERSION
        /*
        if (!NoAds)
        {
            if (bottom)
                Appodeal.show(Appodeal.BANNER_BOTTOM);
            else
                Appodeal.show(Appodeal.BANNER_TOP);
        }
        */
    }

    public void HideBanner()
    {
        //[MIGRATION] TODO: NEWWER VERSION
        /*
        Appodeal.hide(Appodeal.BANNER_TOP);
        */
    }

    public void CheckRewardVideo(DisposableAction m_loadeadVideo)
    {   
        //[MIGRATION] TODO: NEWWER VERSION
        /*
        DateTime lastRewardedVideo;
        var dateString = SecureData.Get(SaveKeys.SK_LAST_REWARDED_VIDEO_SHOWED, string.Empty);

        if (m_rewardVideoEnabled)
        {            
            #if UNITY_EDITOR
            m_loadeadVideo.Run();
            #else
            if (Appodeal.isLoaded(Appodeal.REWARDED_VIDEO))
            {
                m_loadeadVideo.Run();
            }
            else
            {
                Appodeal.cache(Appodeal.REWARDED_VIDEO);
            }
            #endif

        }
        else if (!string.IsNullOrEmpty(dateString) && DateTime.TryParse(dateString, out lastRewardedVideo))
        {
            if ((DateTime.UtcNow - lastRewardedVideo).TotalMinutes >= 5)
            {
                StartCoroutine(ServerTime((date) =>
                    {
                        if ((date - lastRewardedVideo).TotalMinutes >= 5)
                        {
                            m_rewardVideoEnabled = true;
                        }
                    }));
            }
        }
        else
        {
            UpdateLastRewardVideo();
        }        

        */
    }

    void UpdateLastRewardVideo()
    {
        //[MIGRATION] TODO: NEWWER VERSION
        /*
        m_rewardVideoEnabled = false;

        StartCoroutine(ServerTime((date) =>
            {
                SecureData.Save(SaveKeys.SK_LAST_REWARDED_VIDEO_SHOWED, date.ToString()); 
            }));
        */
    }

    IEnumerator ServerTime(Action<DateTime> completed)
    {
        yield break;

        //[MIGRATION] TODO: NEWWER VERSION
        /*
        var form = new WWWForm();
        form.AddField("action", "getservertime");

        var www = new WWW("https://secure682.hostgator.com.br/~gamep938/bubbit/time.php", form);

        yield return www;

        DateTime date;
        if(DateTime.TryParseExact(www.text, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            completed(date);       
        */
    }

    public void ShowRewardVideo(DisposableAction completeAction)
    {
        //[MIGRATION] TODO: NEWWER VERSION
        /*
        m_rewardVideo.ShowRewardVideo(completeAction);
        UpdateLastRewardVideo();
        */
    }

    void OnDestroy()
    {
        FacebookPlugin.GetInstance().OnUserIdSet -= RegisterUserdId;
    }

    //[MIGRATION] TODO: NEWWER VERSION
    /*
    class RewardVideo : AppodealAds.Unity.Common.IRewardedVideoAdListener
    {
        DisposableAction m_actionCompleted;

        public void ShowRewardVideo(DisposableAction action)
        {         
            m_actionCompleted = action;

            #if UNITY_EDITOR
            onRewardedVideoFinished(1, "life");
            #else
            Appodeal.show(Appodeal.REWARDED_VIDEO);
            #endif
        }

        public void onRewardedVideoLoaded()
        {   
            Debug.Log("onRewardedVideoLoaded");
        }

        public void onRewardedVideoFailedToLoad()
        {
            Debug.Log("RewardedVideoFailedToLoad");
        }

        public void onRewardedVideoShown()
        {            
        }

        public void onRewardedVideoFinished(int amount, string name)
        {            
            if (m_actionCompleted != null)
            {   
                m_actionCompleted.Run();
                m_actionCompleted = null;
            }
        }

        public void onRewardedVideoClosed()
        {
            
        }        
    }

    class Interstital : AppodealAds.Unity.Common.IInterstitialAdListener
    {
        DisposableAction m_actionCompleted;

        public void ShowInterstital(DisposableAction action)
        {
            var dateString = SecureData.Get(SaveKeys.SK_LAST_INTERSTITAL_SHOWED, string.Empty);

            var lastInterstital = DateTime.UtcNow;

            if (DateTime.TryParse(dateString, out lastInterstital))
            {
                if ((DateTime.UtcNow - lastInterstital).TotalMinutes >= 2)
                {
                    #if UNITY_EDITOR
                    onInterstitialShown();
                    #else
                    if (Appodeal.isLoaded(Appodeal.INTERSTITIAL))
                    {
                        m_actionCompleted = action;

                        Appodeal.show(Appodeal.INTERSTITIAL);
                    }
                    else
                        Appodeal.cache(Appodeal.INTERSTITIAL);
                    #endif
                }
            }
            else
            {
                SecureData.Save(SaveKeys.SK_LAST_INTERSTITAL_SHOWED, DateTime.UtcNow.ToString()); 
            }
        }

        public void onInterstitialLoaded()
        {
        }

        public void onInterstitialFailedToLoad()
        {
        }

        public void onInterstitialShown()
        {
            SecureData.Save(SaveKeys.SK_LAST_INTERSTITAL_SHOWED, DateTime.UtcNow.ToString()); 

            if (m_actionCompleted != null)
            {   
                m_actionCompleted.Run();
                m_actionCompleted = null;
            }
        }

        public void onInterstitialClosed()
        {
			Appodeal.cache(Appodeal.INTERSTITIAL);
        }

        public void onInterstitialClicked()
        {
        }
        
    }
    */

       
}


