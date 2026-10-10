using UnityEngine;
using System.Collections;

public class Preload : MonoBehaviour
{
    #region STATICS
    TransitionEffect m_transitionEffect { get { return TransitionEffect.GetInstance(); } }
    #endregion

    [SerializeField]
    LanguageMenu m_languageMenu;

	[SerializeField]
	AudioSource m_mainAudioSource;

	IEnumerator Start()
    {
        m_transitionEffect.InstantBlackout();

        Localization.onLanguageChanged += OnLanguageSwitched;

        var sound = SecureData.Get(SaveKeys.SK_SOUND, true);
        AudioListener.volume = sound ? 1 : 0;
        
        string curLanguage = SecureData.Get(SaveKeys.SK_LANGUAGE, "");
        if (string.IsNullOrEmpty(curLanguage))
        {
            m_languageMenu.OpenLanguageMenu();
            m_transitionEffect.Dismiss(null);
        }
        else
        {
            // On WebGL the screen has no real size during the first frames. Loading
            // the menu then builds its texts with a near-zero canvas scale, which
            // corrupts the dynamic font glyphs and leaves the labels invisible.
            while (Screen.width <= 1 || Screen.height <= 1)
                yield return null;
            yield return null;

            m_languageMenu.SwitchLanguage(curLanguage);
        }

		//(Application.platform == RuntimePlatform.Android)
    }

    void RegisterPush()
    {        
        OneSignalPlugin.GetInstance().RegisterPush();
    }

	void StartSound()
	{
		m_mainAudioSource.Play ();
	}

    void OnLanguageSwitched(ILanguage currentLanguage)
    {
		StartSound ();
        RegisterPush();

        m_transitionEffect.Blackout(() => SceneController.LoadMenu());
    }

    void OnDestroy()
    {
        Localization.onLanguageChanged -= OnLanguageSwitched;
    }
}
