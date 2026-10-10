using UnityEngine;
using System.Collections;
using System;

public class PopupController : MonoBehaviour 
{

    static PopupController s_instance;
    public static PopupController GetInstance(){ return s_instance; }

    [SerializeField]
    Popup m_confirmationPopup;

    void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
            DontDestroyOnLoad(this.gameObject);
        }       
    }

    public void Show(string message, Action confirm)
    {
        m_confirmationPopup.Show(message, confirm, null);
    }

    public void Show(string message, Action confirm, Action cancel)
    {
        m_confirmationPopup.Show(message, confirm, cancel);
    }

}
