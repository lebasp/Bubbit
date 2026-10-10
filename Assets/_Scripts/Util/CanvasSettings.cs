using UnityEngine;
using System.Collections;

public class CanvasSettings : MonoBehaviour 
{
	static CanvasSettings s_instance;
	public static CanvasSettings GetInstance(){ return s_instance; }

    float m_canvasScaleFactor = 0;

    float CanvasScaleFactor
    {
        get
        {
            if (m_canvasScaleFactor == 0)
            {
                m_canvasScaleFactor = GetComponent<RectTransform>().localScale.x;
            }

            return m_canvasScaleFactor;
        }
    }

	void Awake () 
	{
        if (s_instance == null)
            s_instance = this;
        else
            Destroy(this);
	}

	public float Convert(float point)
	{
		return point/ CanvasScaleFactor;
	}

	public Vector3 Convert(Vector3 position)
	{
		return position/ CanvasScaleFactor;
	}



}
