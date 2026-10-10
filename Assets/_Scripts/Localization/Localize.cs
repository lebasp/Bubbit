using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Localize : MonoBehaviour {

    [SerializeField]
    string key;

    Text m_text;

	void Awake ()
    {
        m_text = GetComponent<Text>();

        LocalizeMe(Localization.CurrentLanguage);

        Localization.onLanguageChanged += LocalizeMe;

    }

   public void ChangeKey(string newKey)
    {
        key = newKey;

        LocalizeMe(Localization.CurrentLanguage);
    }

    void LocalizeMe(ILanguage language)
    {
        if (!string.IsNullOrEmpty(key))
            m_text.text = language.GetString(key);
    }

    void OnDestroy()
    {
        Localization.onLanguageChanged -= LocalizeMe;
    }
}

static class LocalizeExtension
{
    public static void SetLocalizationKey(this Text text, string key)
    {
        var localize = text.gameObject.GetComponent<Localize>();

        if (localize == null)
            localize = text.gameObject.AddComponent<Localize>();

        localize.ChangeKey(key);
    }
}
