using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Pause : MonoBehaviour {

    #region STATICS
    GameManager m_gameManager { get { return GameManager.GetInstance(); } }
    TransitionEffect m_transitionEffect { get { return TransitionEffect.GetInstance(); } }
    #endregion

    [SerializeField]
    GameObject m_pauseGroup;

    [SerializeField]
    Image m_soundSprite;

    [SerializeField]
    Sprite m_soundOn;

    [SerializeField]
    Sprite m_soundOff;

    public void ShowPause()
    {
        m_pauseGroup.SetActive(true);

        AdjustdSound();
    }

    public void DismissPause()
    {
        m_pauseGroup.SetActive(false);
    }

    public void ResumeButton()
    {
        m_gameManager.PauseButton();
    }

    public void MenuButton()
    {
        m_transitionEffect.Blackout(() =>
            {
                m_gameManager.ReturnMenu();
            });
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

        if (m_soundSprite != null)
            m_soundSprite.sprite = sound ? m_soundOn : m_soundOff;
    }

}
