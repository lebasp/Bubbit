using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Options : MonoBehaviour 
{
    #region STATICS
    OneSignalPlugin m_oneSignalPlugin { get { return OneSignalPlugin.GetInstance(); } }
    FacebookPlugin m_facebookPlugin { get { return FacebookPlugin.GetInstance(); } }
    #endregion

    [SerializeField]
    GameObject m_optionsMenu;

    [SerializeField]
    Image m_soundSprite;

    [SerializeField]
    Sprite m_soundOn;

    [SerializeField]
    Sprite m_soundOff;


    [SerializeField]
    Image m_flagSprite;

    [SerializeField]
    Sprite m_brFlag;

    [SerializeField]
    Sprite m_usaFlag;

    [SerializeField]
    Image m_notificationSprite;

    [SerializeField]
    Sprite m_notificationOn;

    [SerializeField]
    Sprite m_notificationOff;

    [SerializeField]
    Text m_facebookText;

    void Awake()
    {
        Localization.onLanguageChanged += OnLanguageSwitched;
    }

    void OnLanguageSwitched(ILanguage currentLanguage)
    {
        if (currentLanguage is Portuguese)
        {
            m_flagSprite.sprite = m_brFlag;
        }
        if (currentLanguage is English)
        {
            m_flagSprite.sprite = m_usaFlag;
        }
    }

    public void ShowOptions()
    {       
        m_optionsMenu.SetActive(true);

        AdjustdSound();
        AdjustNotification();
        
        OnLanguageSwitched(Localization.CurrentLanguage);

        m_facebookPlugin.CheckLogin(this.addAction<bool>((logged) =>
                {
                    AdjustLogin(logged);
                }));
    }

    public void CloseOptions()
    {
        m_optionsMenu.SetActive(false);   
    }

    public void SoundOnOff()
    {
        var sound = SecureData.Get(SaveKeys.SK_SOUND, true);

        sound = !sound;

        SecureData.Save(SaveKeys.SK_SOUND, sound);    

        AdjustdSound();
    }

    void AdjustdSound()
    {
        var sound = SecureData.Get(SaveKeys.SK_SOUND, true);

        AudioListener.volume = sound ? 1 : 0;

        m_soundSprite.sprite = sound ? m_soundOn : m_soundOff;
    }

    public void NotificatoinOnOff()
    {
        var notification = SecureData.Get(SaveKeys.SK_NOTIFICATION, true);

        notification = !notification;

        SecureData.Save(SaveKeys.SK_NOTIFICATION, notification);  

        AdjustNotification();
            
    }

    void AdjustNotification()
    {
        var notification = SecureData.Get(SaveKeys.SK_NOTIFICATION, true);

        m_notificationSprite.sprite = notification ? m_notificationOn : m_notificationOff;

        m_oneSignalPlugin.SetSubscription(notification);  
    }


    public void RestorePurchases()
    {
    }

    public void LoginFacebook()
    {
        m_facebookPlugin.CheckLogin(this.addAction<bool>((logged) =>
                {
                    if (logged)
                    {
                        m_facebookPlugin.Logout();
                        AdjustLogin(false);
                    }
                    else
                        m_facebookPlugin.Login(this.addAction<bool>((result) =>
                                {
                                    AdjustLogin(result);
                                }));


                }));
    }

    void AdjustLogin(bool logged)
    {

        if (logged)
            m_facebookText.SetLocalizationKey("LOGOUT");
        else
            m_facebookText.SetLocalizationKey("LOGIN");
    }

    public void SwitchLanguage()
    {
        Localization.NextLanguage();
    }

    public void ShowFacebookPage()
    {
        PopupController.GetInstance().Show(Localization.CurrentLanguage.GetString("EXTERNAL_LINK"), () =>
        {
            Application.OpenURL("http://facebook.com/bubbitgame");
        }, ()=> { });
    }

    public void ShowSupportPage()
    {
        PopupController.GetInstance().Show(Localization.CurrentLanguage.GetString("EXTERNAL_LINK"), () =>
        {
            Application.OpenURL("http://bubbit.gameprogrammer.com.br/");
        }, () => { });
    }
    void OnDestroy()
    {
        Localization.onLanguageChanged -= OnLanguageSwitched;
    }

}
