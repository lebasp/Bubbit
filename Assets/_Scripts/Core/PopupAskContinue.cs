using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class PopupAskContinue : MonoBehaviour 
{
    #region STATICS
    Inventory m_inventory { get { return Inventory.GetInstance(); } }
    #endregion

    Action m_confirm;
    Action m_cancel;

    [SerializeField]
    GameObject m_askConfirmationGroup;

    [SerializeField]
    Image m_bar;

    bool m_paused = false;

    [SerializeField]
    Text m_title;

    [SerializeField]
    Text m_description;

    [SerializeField]
    Image m_icon;

    [SerializeField]
    Sprite m_timeSprite;

    [SerializeField]
    Sprite m_continueSprite;

    int m_times = 1;

    public void ShowAskContinue(Action confirm, Action cancel, bool timed, int times)
    {
        m_askConfirmationGroup.SetActive(true);

        m_times = times;
        m_confirm = confirm;
        m_cancel = cancel;

        if (m_title != null)
            m_title.text = timed ? Localization.CurrentLanguage.GetString("EXTRA_TIME") : Localization.CurrentLanguage.GetString("REVIVE");
        if (m_description != null)
            m_description.text = string.Format(Localization.CurrentLanguage.GetString("BUY_FOR"), m_times * 250);
        if (m_icon != null)
            m_icon.sprite = timed ? m_timeSprite : m_continueSprite;

        StartCoroutine(AutoDismiss());
    }

    IEnumerator AutoDismiss()
    {
        m_paused = false;

        var mSeconds = 5000f;

        var timePassed = 0f;

        while(timePassed < mSeconds)
        {
            if (!m_paused)
            {
                var deltaTime = 1000 * Time.deltaTime;
                
                timePassed += deltaTime;

                if (m_bar != null)
                    m_bar.fillAmount = 1 - (timePassed / mSeconds);
            }

            yield return null;
        }

        Cancel();
    }

    void Pause(bool pause)
    {
        m_paused = pause;
    }

    public void Confirm()
    {
        int price = (m_times * 250);

        if (m_inventory.ItemBalance(Inventory.ID_COINS) >= price)
        {
            m_inventory.TakeItem(Inventory.ID_COINS, price);

            if (m_confirm != null)
                m_confirm();

            Close();
        }
        else
        {
            Pause(true);

            PopupController.GetInstance().Show(Localization.CurrentLanguage.GetString("NOT_ENOUGH"), () => Pause(false));
        }
    }

    public void RestartTimer()
    {
        StopAllCoroutines();
        StartCoroutine(AutoDismiss());
    }

    public void Cancel()
    {
        if (m_cancel != null)
            m_cancel();

        Close();
    }

    void Close()
    {
        m_confirm = null;
        m_cancel = null;

        m_askConfirmationGroup.SetActive(false);
        StopAllCoroutines();
    }
}
