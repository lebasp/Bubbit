using UnityEngine;
using System.Collections;

public class SimpleCircle : MonoBehaviour {


    #region STATICS
    CanvasSettings m_canvasSettings {get { return CanvasSettings.GetInstance(); } }
    SkinController m_skincontroller { get { return SkinController.GetInstance(); } }
    #endregion

    [SerializeField]
    float m_speed = 1;

    [SerializeField]
    SpriteRenderer m_renderer;

    [SerializeField]
    BubbleAnimation m_bubbleAnimation;

    Color m_originalColor;

    public float Speed
    {
        get 
        {
            var finalSpeed = m_canvasSettings.Convert( m_speed );

            return finalSpeed;
        }
    }

    Transform m_transform;

    SimpleCircleSpawner m_circleSpawner;

    public Vector3 Position{ get { return m_transform.position; } }

    bool m_moving = false;
    public bool IsMoving
    {
        get
        {
            return m_moving;
        }
        set
        {
            m_moving = value;
        }
    }

    void Awake()
    {
        m_transform = transform;

        m_bubbleAnimation.Setup();

        m_originalColor = m_renderer.color;
    }

    void Start()
    {
        m_skincontroller.OnSpriteChanged += ChangeSprite;
    }

    public void Setup(Vector3 spawnPosition, SimpleCircleSpawner circleSpanwer)
    {
        gameObject.SetActive(true);

        ChangeSprite();

        PlayAnimation();

        m_circleSpawner = circleSpanwer;

        m_moving = true;

        m_transform.localPosition = spawnPosition;

        StartCoroutine(MoveCircle());
    }

    IEnumerator MoveCircle()
    {
        while(true)
        {
            if (IsMoving) 
            {
                var direction = new Vector3 (0, 0.1f, 0);

                var newPosition = Position + (direction * Speed);

                SetPosition (newPosition);
            }

            yield return new WaitForFixedUpdate ();
        }
    }


    public void SetPosition(Vector2 position)
    {
        m_transform.position = position;
    }

    public void KillCircle()
    {
        gameObject.SetActive(false);

        PauseAnimation();

        m_circleSpawner.Remove(this);

        StopAllCoroutines();
    }

    void ChangeSprite()
    {
        m_renderer.sprite = m_skincontroller.GetSprite(SkinController.spriteKind.Simple);
    }

    public void PlayAnimation()
    {
        if(!m_bubbleAnimation.IsPlaying())
            m_bubbleAnimation.Play();
    }

    public void PauseAnimation()
    {
        m_bubbleAnimation.Pause();
    }

    public void Paint(Color color)
    {
        m_renderer.color = (m_originalColor + color) * 0.5f;
    }

    public void Unpaint ()
    {
        m_renderer.color = m_originalColor;
    }

    void OnDestroy()
    {
        m_skincontroller.OnSpriteChanged -= ChangeSprite;
    }
}
