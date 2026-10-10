using UnityEngine;
using System.Collections;

public class Localization
{

    static ILanguage m_language = null;

    public static English English = new English();
    public static Portuguese Portuguese = new Portuguese();

    static int m_currentIndex = 0;
    static ILanguage[] m_loadedLanguages = new ILanguage[]{English, Portuguese};

    public static ILanguage CurrentLanguage
    {
        get
        {
            if (m_language == null)
                m_language = English;

            return m_language;
        }

        private set
        {
            m_language = value;
        }
    }

    public delegate void OnLanguageChanged(ILanguage language);
    public static  OnLanguageChanged onLanguageChanged;

    public static void SetLanguage(ILanguage newLanguage)
    {
        CurrentLanguage = newLanguage;

        SecureData.Save(SaveKeys.SK_LANGUAGE, CurrentLanguage.Prefix);

        if (onLanguageChanged != null)
            onLanguageChanged(CurrentLanguage);

        for (int i = 0; i < m_loadedLanguages.Length; i++)
        {
            if (m_loadedLanguages[i].Prefix.Equals(CurrentLanguage.Prefix))
            {
                m_currentIndex = i;
            }
        }
    }

    public static void NextLanguage()
    {
        m_currentIndex++;
        if (m_currentIndex >= m_loadedLanguages.Length)
            m_currentIndex = 0;

        SetLanguage(m_loadedLanguages[m_currentIndex]);
    }

	
}
