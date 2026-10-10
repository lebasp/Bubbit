using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
	[SerializeField]
	Text m_message;

    [SerializeField]
    GameObject m_cancelButton;


	Action m_confirm;
	Action m_cancel;

	public void Awake()
	{
		Close();
	}



	public void Show(string message, Action confirm, Action cancel)
	{
		gameObject.SetActive(true);

        if (cancel != null)
            m_cancelButton.SetActive(true);
        else
            m_cancelButton.SetActive(false);

		m_message.text = message;

		m_confirm = confirm;
		m_cancel = cancel;
	}

	public void Confirm()
	{
		if (m_confirm != null)
			m_confirm();

		Close();
	}

	public void Cancel()
	{
		if (m_cancel != null)
			m_cancel();

		Close();
	}

    public void Close()
	{
		m_confirm = null;
		m_cancel = null;

		gameObject.SetActive(false);
	}
}
