using UnityEngine;
using System.Collections;



public class SkinController : MonoBehaviour
{   
    BackgroundController m_backgroundController { get { return BackgroundController.GetInstance(); } }

    static SkinController s_instance;
    public static SkinController GetInstance() { return s_instance; }

    public enum spriteKind
    {
        Normal, Selected, Death, DeathSelected, Frozen, Simple
    }

    [System.Serializable]
    struct BubbleSprites
    {
        public Sprite Normal;
        public Sprite Selected;
        public Sprite Frozen;
        public Sprite Death;
        public Sprite DeathSelected;
        public Sprite Simple;
    }

    public delegate void onSpriteChanged();
    public event onSpriteChanged OnSpriteChanged;

    public delegate void onUnselect(string id);
    public event onUnselect OnUnselect;

    public delegate void onSelect(string id);
    public event onSelect OnSelect;


    [SerializeField]
    BubbleSprites m_regular;
    [SerializeField]
    BubbleSprites m_octagonal;
    [SerializeField]
    BubbleSprites m_star;
    [SerializeField]
    BubbleSprites m_square;

    BubbleSprites m_selectedSprites;

    void Awake()
    {
        if (s_instance != null && s_instance != this)
        {
            Destroy(gameObject);
            return;
        }

        s_instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    public void Start()
    {
        if (s_instance != this)
            return;

        SetupSkin(EquippedBubbleForm());

        SetupBackground(EquippedBG());
    }

    public string EquippedBG()
    {
        var equipped = SecureData.Get(SaveKeys.SK_BACKGROUND_EQUIPPED, Inventory.ID_CS_BG_BLACK);
        if (!Inventory.GetInstance().CheckItem(equipped))
            equipped = Inventory.ID_CS_BG_BLACK;

        return equipped;            
    }

    public string EquippedBubbleForm()
    {
        var equipped = SecureData.Get(SaveKeys.SK_BUBBLE_SPRITE, Inventory.ID_BS_NORMAL);
        if (!Inventory.GetInstance().CheckItem(equipped))
            equipped = Inventory.ID_BS_NORMAL;

        return equipped;
            
    }

    public void SetupSkin(string skinId, bool save = true)
    {
        if (OnUnselect != null)
            OnUnselect(EquippedBubbleForm());

        if (skinId == Inventory.ID_BS_NORMAL)
            m_selectedSprites = m_regular;
        else if (skinId == Inventory.ID_BS_OCT)
            m_selectedSprites = m_octagonal;
        else if (skinId == Inventory.ID_BS_SQUARE)
            m_selectedSprites = m_square;
        else if (skinId == Inventory.ID_BS_STAR)
            m_selectedSprites = m_star;
        
        if(save)
            SecureData.Save(SaveKeys.SK_BUBBLE_SPRITE, skinId);
        
        if (OnSpriteChanged != null)
            OnSpriteChanged();
        
        if (OnSelect != null)
            OnSelect(skinId);
    }

    public void SetupBackground(string backgroundId, bool save = true)
    {
        if (OnUnselect != null)
            OnUnselect(EquippedBG());

        m_backgroundController.PaintBackgroundColor(GetColorFromPurchase(backgroundId));

        if(save)
            SecureData.Save(SaveKeys.SK_BACKGROUND_EQUIPPED, backgroundId);
        
        if (OnSelect != null)
            OnSelect(backgroundId);
    }

    public Sprite GetSprite(spriteKind kind)
    {
        if (kind == spriteKind.Normal)
            return m_selectedSprites.Normal;
        if (kind == spriteKind.Selected)
            return m_selectedSprites.Selected;
        if (kind == spriteKind.Frozen)
            return m_selectedSprites.Frozen;
        if (kind == spriteKind.Death)
            return m_selectedSprites.Death;
        if (kind == spriteKind.DeathSelected)
            return m_selectedSprites.DeathSelected;
        if (kind == spriteKind.Simple)
            return m_selectedSprites.Simple;

        return m_selectedSprites.Normal;        
    }

    Color GetColorFromPurchase(string id)
    {
        if (id == Inventory.ID_CS_BG_BLACK)
            return Color.black;
        if (id == Inventory.ID_CS_BG_GREY)
            return Color.grey;
        if (id == Inventory.ID_CS_BG_RED)
            return Color.red;
        if (id == Inventory.ID_CS_BG_ORANGE)
            return new Color(1, 0.5f, 0);
        if (id == Inventory.ID_CS_BG_YELLOW)
            return Color.yellow;
        if (id == Inventory.ID_CS_BG_GREEN)
            return Color.green;
        if (id == Inventory.ID_CS_BG_CYAN)
            return Color.cyan;
        if (id == Inventory.ID_CS_BG_BLUE)
            return Color.blue;
        if (id == Inventory.ID_CS_BG_PURPLE)
            return new Color(0.5f, 0, 0.5f);;
        if (id == Inventory.ID_CS_BG_MAGENTA)
            return Color.magenta;

        return Color.black;
    }

}
