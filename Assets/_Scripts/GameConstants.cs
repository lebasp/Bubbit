using UnityEngine;
using System.Collections;

public class GameConstants
{
    public const bool IsBeta = false;

    public static string StoreLink
    {
        get
        {
            #if UNITY_IOS
            return "https://itunes.apple.com/app/bubbit/id1086183753";
            #else
            return "https://play.google.com/store/apps/details?id=com.seraphic.bubbler";
            #endif
        }
    }
}
