using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class English : ILanguage
{
    public string Prefix { get { return "usa"; } }

    public Dictionary<string, string> strings = new Dictionary<string, string>
    {
        { "PLAY","Play"},
        { "SURVIVAL_GAME" , "Survival" },
        { "TIMED_GAME" , "Timed" },
        { "SCORE" , "SCORES" },
        { "OPTIONS" , "options" },
        { "TUTORIAL" , "Tutorial" },
        { "INVITE" , "Invite more friends" },
        { "REMOVE_ADS" , "Remove Ads" },
        { "SOUND" , "Sound" },
        { "NOTIFICATION" , "Notification" },
        { "LANGUAGE" , "Language" },
        { "RESTORE_PURCHASES" , "Restore\nPurchases" },
        { "RECORD_ON" , "Replay\nOn" },       
        { "RECORD_OFF" , "Replay\nOff" },
        { "LOGIN" , "Login" },
        { "LOGOUT" , "Logout" },
        { "FB_ASK_LOGIN","Click below to connect your Facebook and see your friends scores."},
        { "FRIENDS" , "Friends" },
        { "WORLD" , "World" },
        { "LOADING" , "Loading" },
        {"CUSTOMIZE", "CUSTOMIZE"},
        {"BG_COLOR", "BACKGROUND COLOR"},
        {"BUBBLE_FORM", "BUBBLE SHAPE"},
        {"CLOSE", "CLOSE"},
        {"MORE","MORE"},
        {"EXTERNAL_LINK", "It will redirect to an external link. Confirm?" },


        {"TUTORIAL_1", "Connect to be able to move the bubbles around"},
        {"TUTORIAL_2", "Merge 2 bubbles to increase your score"},
        {"TUTORIAL_3", "Avoid the skull bubbles, or you will lose a heart"},
        {"TUTORIAL_4", "Special bubbles cause power ups to activate"},
        {"PRO_TIP_1", "TIP: Connected bubbles have their speed slowed!"},
        {"PRO_TIP_2", "TIP: Bubbles higher than 10 points will clear the board!"},
        {"PRO_TIP_3", "TIP: You can collide two skull bubbles safely!"},
        {"PRO_TIP_4", "TIP: Bubbles thrown out of sight (sides) are discarded!"},

        { "TIME" , "TIME" },
        { "LEVEL" , "Level" },
        { "PAUSED" , "PAUSED" },
        { "MENU" , "Menu" },
        { "RESUME" , "Resume" },
        { "GAMEOVER" , "GAME OVER" },
        { "RETRY" , "Retry" },
        { "SHARE" , "Share" },
        { "REPLAY" , "Replay" },

        { "USE_LIFE" , "Use a life?" },

        { "FB_FRIEND_GOT_YOUR_SCORE" , "Hey! {0} got your leaderboard position in the {1} mode!" },
        { "REMOVED_FROM_TOP" , "Someone beat your world best score in the {0} mode :( Go there and get your throne back!" },
        { "ASK_SEND_NOTIFICATION_FRIEND","You beat {0} ! Would you like to send a notification?"},

        {"RATE_MY_APP","Nice score!\nCould you, please, take some seconds to give us your rating?"},

        {"SCREENSHOT_TEXT", "Check this out! Playing Bubbit I scored {0} on {1} mode!"},

        {"ABOUT", "About"},
        {"CONTACT_US","Contact Us"},

        {"SUPPORT", "Support"},
        {"EVERYPLAY_COM","Everyplay\nCommunity"},
        {"FACEBOOK_PAGE","Bubbit on\nFacebook"},

        {"SHOP", "Shop"},
        {"FREE_COINS", "Free Coins"},
        {"COINS_RECEIVED", "{0} coins received!"},

        {"REVIVE", "Revive"},
        {"EXTRA_TIME", "Extra Time (30 secs)"},
        {"BUY_FOR", "Buy for {0}"},
        {"NOT_ENOUGH", "Not enough coins.\nKeep playing to earn more!"},
        {"CONFIRM_PURCHASE", "Buy for {0} coins?"},

        {"GAME", "Game"},
        {"ACHIEVEMENTS", "Achievements"},
        {"LEADERBOARDS", "Leaderboards"},
    };

    public string GetString(string key)
    {
        if (strings.ContainsKey(key))
            return strings[key];

        return "NF: " + key;
    }
}
