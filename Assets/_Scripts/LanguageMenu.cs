using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LanguageMenu : MonoBehaviour {
    
    [SerializeField]
    GameObject m_languageMenu;


    public void OpenLanguageMenu()
    {
        m_languageMenu.SetActive(true);
    }

    public void CloseLanguageMenu()
    {
        m_languageMenu.SetActive(false);
    }

    public void SwitchLanguage(string language)
    {        
        if (language.Equals(Localization.Portuguese.Prefix))
        {
            Localization.SetLanguage(Localization.Portuguese);
        }
        else if (language.Equals(Localization.English.Prefix))
        {
            Localization.SetLanguage(Localization.English);
        }

        CloseLanguageMenu();
    }

    
}
