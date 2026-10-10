using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CircleView : MonoBehaviour 
{

    SkinController m_skinController{ get { return SkinController.GetInstance(); } }

    [SerializeField]
    bool m_isDeath;

	[SerializeField]
	protected SpriteRenderer m_spriteRenderer;
	public SpriteRenderer SpriteRender { get { return m_spriteRenderer; } }

    [SerializeField]
    BubbleAnimation m_bubbleAnimation;

	protected Circle m_circle;

	protected virtual void Awake()
    {
        m_skinController.OnSpriteChanged += ChangeSprite;

        m_circle = GetComponent<Circle>();

        m_bubbleAnimation.Setup();

        m_circle.OnStateChanged += (currentState) =>
        {
            switch (currentState)
            {
                case Circle.state.Free:
                    PlayAnimation();
                    SetDefaultSprite();
                    break;
                case Circle.state.Locked:
                    PlayAnimation();
                    SetSelectedSprite();
                    break;
                case Circle.state.Controlling:
                    PlayAnimation();
                    SetSelectedSprite();
                    break;
                case Circle.state.Frozen:
                    PauseAnimation();
                    SetFrozenSprite();
                    break;
                case Circle.state.Dead:
                    PauseAnimation();
                    break;
            }
        };

        m_circle.OnShake += () =>  m_bubbleAnimation.Shake();

    }

    void ChangeSprite()
    {

        m_spriteRenderer.sprite = m_skinController.GetSprite(m_isDeath? SkinController.spriteKind.Death : SkinController.spriteKind.Normal);
    }

    void Start()
    {
        GameManager.GetInstance().OnGamePausedEvent += PausedGame;        
    }

	void SetDefaultSprite()
	{
        m_spriteRenderer.sprite = m_isDeath?m_skinController.GetSprite(SkinController.spriteKind.Death):m_skinController.GetSprite(SkinController.spriteKind.Normal);
    }
	
	void SetSelectedSprite()
	{
        m_spriteRenderer.sprite = m_isDeath?m_skinController.GetSprite(SkinController.spriteKind.DeathSelected):m_skinController.GetSprite(SkinController.spriteKind.Selected);
	}
	
	void SetFrozenSprite()
	{
        m_spriteRenderer.sprite = m_skinController.GetSprite(SkinController.spriteKind.Frozen);
	}

    void PlayAnimation()
    {
        if(!m_bubbleAnimation.IsPlaying())
            m_bubbleAnimation.Play();
    }

    void PauseAnimation()
    {
        m_bubbleAnimation.StopShake();
        m_bubbleAnimation.Pause();
    }

    void PausedGame(bool paused)
    {
        if (paused)
            PauseAnimation();
        else
            PlayAnimation();
    }

    void OnDestroy()
    {
        GameManager.GetInstance().OnGamePausedEvent -= PausedGame;
    }
}
