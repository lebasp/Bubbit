using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScreenshotView : MonoBehaviour
{
    #region STATICS
    FacebookPlugin m_facebookPlugin { get { return FacebookPlugin.GetInstance(); } }
    #endregion

    [SerializeField]
    Text m_gameNameText;

    [SerializeField]
    Color m_defaulScoreColor;

    [SerializeField]
    Text m_scoreText;


    [SerializeField]
    GameObject m_facebookGroup;

    [SerializeField]
    Image m_facebookImage;

    [SerializeField]
    Image m_medal;

    [SerializeField]
    Sprite[] m_medalSprites;

    [SerializeField]
    Color[] colors;

    [SerializeField]
    Graphic[] coloredGraphics;


    public void Setup(GameManager.kind gameKind, int score)
    {
        m_gameNameText.text = gameKind == GameManager.kind.Survival ? Localization.CurrentLanguage.GetString("SURVIVAL_GAME") : Localization.CurrentLanguage.GetString("TIMED_GAME");

        m_facebookGroup.SetActive(false);
        m_scoreText.color = m_defaulScoreColor;
        m_scoreText.text = score.ToString();

        m_facebookPlugin.CheckLogin(this.addAction<bool>((logged) =>
            {
                if (logged)
                {    
                    UpdatePicture(m_facebookPlugin.UserId);                   
                }
                else
                    m_facebookPlugin.OnUserIdSet += UpdatePicture;
            }));
    }

    void UpdatePicture(string userId)
    {
        m_medal.gameObject.SetActive(false);

        FacebookPlugin.GetInstance().GetuserPicture(userId, this.addAction<Sprite>((sprite) =>
            {
                if (sprite != null)
                {
                    m_facebookImage.sprite = sprite;
                    m_facebookGroup.SetActive(true);
                }
            })); 
    }

    public void AdjustRank(int index)
    {              
        if (index < m_medalSprites.Length)
        {
            m_medal.gameObject.SetActive(true);
            m_medal.sprite = m_medalSprites[index];
        }

        var colorIndex = index % colors.Length;

        foreach (var item in coloredGraphics)
            item.color = colors[colorIndex];
    }

}
