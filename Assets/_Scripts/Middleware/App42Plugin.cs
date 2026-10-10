using UnityEngine;
using System.Collections;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.game;
using System;
using com.shephertz.app42.paas.sdk.csharp.pushNotification;

public class App42Plugin
{

    public int WOLRD_LIMIT = 10;
    public int FRIENDS_LIMIT = 25;

    public static App42Plugin s_instance;
	public static App42Plugin GetInstance()
    { 
        if (s_instance == null) s_instance = new App42Plugin();
        return s_instance; 
    }

	string m_apiKey = "";
	string m_secret = "";	

    ScoreBoardService m_scoreBoardService;			

    App42Plugin()
	{				
        //[MIGRATION] TODO: NEWWER VERSION
        /*
		App42API.Initialize (m_apiKey, m_secret);
		
		m_scoreBoardService = App42API.BuildScoreBoardService ();
        */
	}	

	public void SendhighScore(string level, string user, int points)
	{ 
        //[MIGRATION] TODO: NEWWER VERSION
        /*
		m_scoreBoardService.SaveUserScore(level, user, points, new App42DefaultCallBack<Game>(null, null));   
        */
	}

	public void GetHighScore(string level, App42DefaultCallBack<Game>.OnSuccessDelegate success, App42DefaultCallBack<Game>.OnExceptionDelegate exception)
	{
        exception(new Exception("App42 disabled"));

        //[MIGRATION] TODO: NEWWER VERSION
        /*
        m_scoreBoardService.GetTopNRankers(level, WOLRD_LIMIT, new App42DefaultCallBack<Game>(success, exception));   
        */
	}

	public void GetHighScoreFacebookFriends(string level, string fbAccessToken, App42DefaultCallBack<Game>.OnSuccessDelegate success, App42DefaultCallBack<Game>.OnExceptionDelegate exception)
    {
        exception(new Exception("App42 disabled"));

        //[MIGRATION] TODO: NEWWER VERSION
        /*
        m_scoreBoardService.GetTopNRankersFromFacebook(level, fbAccessToken, FRIENDS_LIMIT, new App42DefaultCallBack<Game>(success, exception));
        */
    }

	public void GetMyHighscore(string level, string userName, App42DefaultCallBack<Game>.OnSuccessDelegate success, App42DefaultCallBack<Game>.OnExceptionDelegate exception)
	{
        exception(new Exception("App42 disabled"));

        //[MIGRATION] TODO: NEWWER VERSION
        /*
		m_scoreBoardService.GetHighestScoreByUser(level, userName, new App42DefaultCallBack<Game>(success, exception));
        */
	}	 
}

public class App42DefaultCallBack<T> : App42CallBack  
{  
    public delegate void OnSuccessDelegate(T game);
    public event OnSuccessDelegate OnSuccessEvent;
    public delegate void OnExceptionDelegate(Exception e);
    public event OnExceptionDelegate OnExceptionEvent;

    public App42DefaultCallBack(OnSuccessDelegate success, OnExceptionDelegate exception)
    {
        OnSuccessEvent += success;
        OnExceptionEvent += exception;
    }

    public void OnSuccess(object response)  
    {  
        if(OnSuccessEvent != null)
            OnSuccessEvent((T)response);
    }  

    public void OnException(Exception e)  
    {
        if (OnExceptionEvent != null)
            OnExceptionEvent(e);
        Debug.Log("Exception : " + e);  
    }  
} 

