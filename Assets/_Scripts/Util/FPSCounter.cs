using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour {
    
	public Text m_text;
    float deltaTime = 0.0f;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

	void Update ()
	{
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        m_text.text = text;
	
	}
}
