using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
//[MIGRATION] TODO: NEWWER VERSION
//using Facebook.Unity;

public class FacebookPlugin
{

    class FacebookUser
    {
        public string Name;
        public Sprite Picture;
    }

    Dictionary<string, FacebookUser> m_cache = new Dictionary<string, FacebookUser>();

    static FacebookPlugin s_instance;
    public static FacebookPlugin GetInstance()
    {
        if (s_instance == null)
        {
            s_instance = new FacebookPlugin();
        }
        return s_instance;
    }

    string m_userId = "";
    public string UserId
    {
        get { return m_userId; }
        private set { m_userId = value; }
    }

    string m_accessToken = "";
    public string AccessToken
    {
        get { return m_accessToken; }
        private set { m_accessToken = value; }
    }

    public delegate void onUserIdSet(string userId);
    public onUserIdSet OnUserIdSet;

    bool m_initCalled = false;
    List<Action> m_doAfterInit = new List<Action>();

    void InitFacebook(Action doAfterInit)
    {
        //[MIGRATION] TODO: NEWWER VERSION
        /*
        if (!FB.IsInitialized)
        {
            m_doAfterInit.Add(doAfterInit);

            if (!m_initCalled)
            {
                m_initCalled = true;                

                FB.Init(() =>
                    {
                        if (FB.IsLoggedIn)
                        {
                            var accessToken = Facebook.Unity.AccessToken.CurrentAccessToken;

                            SetCredentials(accessToken.UserId, accessToken.TokenString);
                       
                        }

                        foreach(var action in m_doAfterInit)
                            action();

                        m_doAfterInit.Clear();
                    });
            }
        }
        else
            doAfterInit();
        */
    }

    void SetCredentials(string userId, string accessToken)
    {
        UserId = userId;
        AccessToken = accessToken;

        if (OnUserIdSet != null)
        {
            OnUserIdSet(userId);
            OnUserIdSet = null;
        }
    }

    public void CheckLogin(DisposableAction<bool> loggedIn)
    {
        loggedIn.Run(false);

        //[MIGRATION] TODO: NEWWER VERSION
        /*
        InitFacebook(() =>
            {
                loggedIn.Run(FB.IsLoggedIn);
            });
        */
    } 

    public void Login(DisposableAction<bool> loggedIn)
    {
        loggedIn.Run(false);

        //[MIGRATION] TODO: NEWWER VERSION
        /*
        InitFacebook(() =>
            {
                if (!FB.IsLoggedIn)
                {
                    FB.LogInWithReadPermissions(new[] { "user_friends" }, (result) =>
                        {
                            if (FB.IsLoggedIn)
                            {
                                SetCredentials(result.AccessToken.UserId, result.AccessToken.TokenString);
                            }

                            loggedIn.Run(FB.IsLoggedIn);
                        });

                }
                else
                    loggedIn.Run(FB.IsLoggedIn);
            });
        */
    }

    public void Logout()
    {
        //[MIGRATION] TODO: NEWWER VERSION
        /*
        InitFacebook(() =>
            {
                FB.LogOut();
            });
        */
    }

    public void GetUserName (string fbID, DisposableAction<string> userName)
    {
        userName.Run("");

        //[MIGRATION] TODO: NEWWER VERSION
        /*
        if (m_cache.ContainsKey(fbID) && !string.IsNullOrEmpty(m_cache[fbID].Name))
            userName.Run(m_cache[fbID].Name);
        else
        {
            
            InitFacebook(() =>
                {


                    FB.API("/" + fbID, HttpMethod.GET, (result) =>
                        {
                            try
                            {
                                if (string.IsNullOrEmpty(result.Error))
                                {
                                    if(!m_cache.ContainsKey(fbID))
                                        m_cache[fbID] = new FacebookUser();

                                    var name = (string)result.ResultDictionary["name"];

                                    m_cache[fbID].Name = name;

                                    userName.Run(name);
                                }
                                else
                                    userName.Run("");
                            }
                            catch
                            {
                                userName.Run("");
                            }

                        });
                });
        }
        */
    }

    public void GetuserPicture(string fbID, DisposableAction<Sprite> sprite)
    {
        sprite.Run(null);

        //[MIGRATION] TODO: NEWWER VERSION
        /*
        if (m_cache.ContainsKey(fbID) && m_cache[fbID].Picture != null)
            sprite.Run(m_cache[fbID].Picture);
        else
        {

            InitFacebook(() =>
                {
                    FB.API("/" + fbID + "/picture?type=square", HttpMethod.GET, (result) =>
                        {
                            try
                            {
                                if (string.IsNullOrEmpty(result.Error))
                                {

                                    if(!m_cache.ContainsKey(fbID))
                                        m_cache[fbID] = new FacebookUser();

                                    var tex = result.Texture;

                                    var picture = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));

                                    m_cache[fbID].Picture = picture;

                                    sprite.Run(picture);
                                }
                                else
                                    sprite.Run(null);

                            }
                            catch
                            {
                                sprite.Run(null);
                            }

                        });
                });
        }
        */
    }

    public void FacebookInvite()
    {
        //[MIGRATION] TODO: NEWWER VERSION
        /*
        InitFacebook(() =>
        {
                FB.Mobile.AppInvite(new Uri( "https://fb.me/1678345672434626")); //https://developers.facebook.com/quickstarts/1614260142176513/?platform=app-links-host
        });
        */
    }
            
    

}
  