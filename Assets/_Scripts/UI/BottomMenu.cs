using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class BottomMenu : MonoBehaviour
{

    [SerializeField]
    GameObject m_blockScreen;

    [SerializeField]
    GameObject m_moreOptions;

    [SerializeField]
    Text m_symbol;

    [SerializeField]
    Text m_optionsText;

    [SerializeField]
    GameObject m_removeAdsButton;

    [SerializeField]
    Menu m_menu;

    bool m_open;

    public void Open()
    {
        m_blockScreen.SetActive(true);
        m_moreOptions.SetActive(true);
        m_symbol.text = "-";
        m_optionsText.text = Localization.CurrentLanguage.GetString("CLOSE");

        // Premium version: there are no ads to remove
        m_removeAdsButton.SetActive(false);       

        m_open = true;
    }

    public void Close()
    {
        m_blockScreen.SetActive(false);
        m_moreOptions.SetActive(false);
        m_symbol.text = "+";
        m_optionsText.text = Localization.CurrentLanguage.GetString("MORE");

        m_open = false;
    }

    public void OpenCloseButton()
    {
        if (m_open)
            Close();
        else
            Open();
    }

    public void OpenTutorial()
    {
        m_menu.DismissAll();
        m_menu.Tutorial();
        Close();
    }

    public void OpenCustomization()
    {
        m_menu.DismissAll();
        m_menu.Customize();
        Close();
    }

    public void OpenLeaderboard()
    {
        m_menu.DismissAll();
        m_menu.Leaderboards();
        Close();
    }

    public void OpenSettings()
    {
        m_menu.DismissAll();
        m_menu.Options();
        Close();
    }

    public void RemoveAds()
    {
        // Premium version: the button is hidden in Open()
    }
}
