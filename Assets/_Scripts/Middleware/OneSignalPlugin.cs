using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
//[MIGRATION] TODO: NEWWER VERSION
//using OneSignalPush.MiniJSON;

public class OneSignalPlugin : MonoBehaviour
{
    #region STATICS
    public static FacebookPlugin m_facebookPlugin { get { return FacebookPlugin.GetInstance(); } }
    #endregion

    public static OneSignalPlugin s_instance;
    public static OneSignalPlugin GetInstance()
    {
        if (s_instance == null)
        {
            var go = new GameObject("OneSignalPlugin");
            go.AddComponent<OneSignalPlugin>();
        }
        return s_instance;
    }

    bool m_initialized = false;

    private static void HandleNotification(string message, Dictionary<string, object> additionalData, bool isActive) 
    {
        
    }

    void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void RegisterPush()
    {
        //[MIGRATION] TODO: NEWWER VERSION
        /*
        if (m_initialized)
            return;

        m_initialized = true;
        
        OneSignal.Init("", "", HandleNotification);

        m_facebookPlugin.CheckLogin(this.addAction<bool>((logged) =>
                {
                    if (logged)
                        RegisterFbId(m_facebookPlugin.UserId);
                    else
                    {
                        m_facebookPlugin.OnUserIdSet += RegisterFbId;
                    }
                }));
        */
    }

    void RegisterFbId(string fbid)
    {
        //[MIGRATION] TODO: NEWWER VERSION
        /*
        #if UNITY_EDITOR
        Debug.LogWarning("ONESIGNAL: RegisterPush " + fbid);
        #endif

        OneSignal.SendTag("fbid", fbid);
        OneSignal.GetIdsAvailable((deviceId, token) =>
            {
                OneSignalRunner.CreateRegisterPushRunner(fbid, deviceId); 
            });
        */
    }   

    public void SendNotification(string fbId, Dictionary<string, string> messages)
    {
        //[MIGRATION] TODO: NEWWER VERSION
        /*
        if (!m_initialized)
            return;

        #if UNITY_EDITOR
        Debug.LogWarning("ONESIGNAL: SendNotification " + fbId + " message: " + messages.ToString());
        #endif

        OneSignalRunner.GetDevicesFor(fbId, (devices) =>
        {
            var notification = new Dictionary<string, object>();
            notification["contents"] = messages;
            notification["include_player_ids"] = devices;


            OneSignal.PostNotification(notification);
        });
        */
    }

    public void SetSubscription(bool enable)
    {
        //[MIGRATION] TODO: NEWWER VERSION
        /*
        if (!m_initialized)
            return;

        #if UNITY_EDITOR
        Debug.LogWarning("ONESIGNAL: SetSubscription " + enable);
        #endif

        OneSignal.SetSubscription(enable);
        */
    }

    class OneSignalRunner : MonoBehaviour
    {
        public static void CreateRegisterPushRunner(string fbid, string deviceId)
        {
            //[MIGRATION] TODO: NEWWER VERSION
            /*
            var go = new GameObject("__oneSignal_runner");
            var runner = go.AddComponent<OneSignalRunner>();
            runner.StartCoroutine(runner.RegisterForPush(fbid, deviceId));
            */
        }

        public static void GetDevicesFor(string fbid, Action<List<string>> result)
        {
            //[MIGRATION] TODO: NEWWER VERSION
            /*
            var go = new GameObject("__oneSignal_runner");
            var runner = go.AddComponent<OneSignalRunner>();
            runner.StartCoroutine(runner.GetUserdDevices(fbid, runner.addAction<List<string>>(result)));  
            */
        }

        IEnumerator RegisterForPush(string fbid, string deviceId)
        {
            yield break;

            //[MIGRATION] TODO: NEWWER VERSION
            /*
            DontDestroyOnLoad(this.gameObject);

            var form = new WWWForm();
            form.AddField("action", "post");
            form.AddField("fbid", fbid);
            form.AddField("device", deviceId);

            var www = new WWW("<legacy-backend-url>/bubbit/push/bd.php", form);

            yield return www;

            if (www.error != null)
                Debug.Log(www.error);

            Destroy(gameObject);
            */
        }

        IEnumerator GetUserdDevices(string fbid, DisposableAction<List<string>> result)
        {        
            yield break;

            //[MIGRATION] TODO: NEWWER VERSION
            /*
            DontDestroyOnLoad(this.gameObject);

            var form = new WWWForm();
            form.AddField("action", "get");
            form.AddField("fbid", fbid);

            var www = new WWW("<legacy-backend-url>/bubbit/push/bd.php", form);

            yield return www;

            if (www.error != null)
                Debug.Log(www.error);
            else
            {
                var list = (List<object>)Json.Deserialize(www.text);
                List<string> devices = new List<string>();
                if (list != null)
                {
                    devices = list.ConvertAll(input => input as string);
                    result.Run(devices);
                }
            }

            Destroy(gameObject);
            */
        }
    }
}
