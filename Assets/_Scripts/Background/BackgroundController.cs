using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class BackgroundController : MonoBehaviour 
{

    static BackgroundController s_instance;
    public static BackgroundController GetInstance() { return s_instance; }

    public delegate void onColorChanged(Color color);
    public onColorChanged OnColorChanged;

    [SerializeField]
    Camera m_camera;

    [SerializeField]
    SimpleCircleSpawner[] m_simpleSpawners;

    List<SimpleCircle> m_activeCircles;

    bool m_isPainted = false;
    Color m_spriteColor;

    Color m_backgroundColor = Color.black;

    int index = 0;

    void Awake()
    {
        if (!s_instance)
        {
            s_instance = this;

            m_activeCircles = new List<SimpleCircle>();

            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void PaintBackgroundColor(Color color)
    {
        m_backgroundColor = color;

        m_camera.backgroundColor = (Color.black + (Color.black + color) * 0.5f) * 0.5f; 

        if (OnColorChanged != null)
            OnColorChanged(color);
    }

    public Color BackgroundColor()
    {
        return m_backgroundColor;
    }

    public void AddActiveCircle(SimpleCircle circle)
    {
        m_activeCircles.Add(circle);

        if (m_isPainted)
            circle.Paint(m_spriteColor);
        else
            circle.Unpaint();
    }

    public void RemoveActiveCircle(SimpleCircle circle)
    {
        m_activeCircles.Remove(circle);
    }

    public void Paint(Color color)
	{
        Unpaint();

        m_isPainted = true;

        m_spriteColor = color;

        foreach(var circle in m_activeCircles)
            circle.Paint(color);
	}

	public void Unpaint()
	{
        m_isPainted = false;

        foreach (var circle in m_activeCircles)
            circle.Unpaint();
	}

    public void Pause(bool pause)
    {
        foreach(var spawner in m_simpleSpawners)
            spawner.Pause(pause);

        if (pause)
        {
            PauseAnimation();
            foreach (var circle in m_activeCircles)
                circle.IsMoving = false;
        }
        else
        {
            PlayAnimation();
            foreach (var circle in m_activeCircles)
                circle.IsMoving = true;
        }
    }

    public void Flash()
    {
        m_camera.DOColor(Color.white, 0.3f).SetEase(Ease.InOutBounce).SetLoops(2, LoopType.Yoyo);
    }

	void PlayAnimation()
	{
        foreach (var circle in m_activeCircles)
            circle.PlayAnimation();
	}

	void PauseAnimation()
	{
        foreach (var circle in m_activeCircles)
            circle.PauseAnimation();
	}
}

