using UnityEngine;
using System.Collections;

public class AdsMiddleware : MonoBehaviour
{
    static AdsMiddleware s_instance;

    AppodealPlugin plugin;

    public static AdsMiddleware GetInstance()
    {
        if (s_instance == null)
        {

            var go = new GameObject();
           var middleware = go.AddComponent<AdsMiddleware>();

            #if UNITY_IPHONE || UNITY_ANDROID
            var name = " AdsMiddleware - Appodeal";
            middleware.plugin = go.AddComponent<AppodealPlugin>();
            #else
            var name = "AdsMiddleware";
            #endif
            go.name = name;

        }
        return s_instance;
    }

    void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;

            DontDestroyOnLoad(gameObject);
        }
    }

    public void ShowBanner(bool bottom)
    {
        #if UNITY_IPHONE || UNITY_ANDROID
        plugin.ShowBanner(bottom);
        #endif
    }

    public void HideBanner()
    {
        #if UNITY_IPHONE || UNITY_ANDROID
        plugin.HideBanner();
        #endif
    }

    public void CheckRewardVideo(DisposableAction loadedVideo)
    {
        #if UNITY_IPHONE || UNITY_ANDROID
        plugin.CheckRewardVideo(this.addAction(() => loadedVideo.Run()));
        #endif
    }

    public void ShowInterstitial(DisposableAction completeAction)
    {
        #if UNITY_IPHONE || UNITY_ANDROID
        plugin.ShowInterstitial(this.addAction(() => completeAction.Run()));
        #endif
    }

    public void ShowRewardVideo(DisposableAction completeAction)
    {
        #if UNITY_IPHONE || UNITY_ANDROID
        plugin.ShowRewardVideo(this.addAction(() => completeAction.Run()));
        #endif
    }
}
