using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class LeaderboardCard : MonoBehaviour 
{
	[SerializeField]
	Color[] colors;

	[SerializeField]
	Graphic[] coloredGraphics;

	[SerializeField]
	Image facebookImage;

	[SerializeField]
	Text facebookName;

	[SerializeField]
	Text scoreLabel;

	[SerializeField]
	Text rankLbel;

    [SerializeField]
    Sprite m_defaultPicture;

    [SerializeField]
    Image m_medal;

    [SerializeField]
    Sprite[] m_medalSprites;


    public void SetupCard(int index, string facebookId, string score, string customRank = "")
    {
        try
        {
            if (index < m_medalSprites.Length)
            {
                m_medal.gameObject.SetActive(true);
                m_medal.sprite = m_medalSprites[index];
            }
            else
                m_medal.gameObject.SetActive(false);

            var colorIndex = index % colors.Length;

            foreach (var item in coloredGraphics)
                item.color = colors[colorIndex];

            if (string.IsNullOrEmpty(customRank))
                rankLbel.text = (index + 1).ToString();
            else
                rankLbel.text = customRank;
            
            scoreLabel.text = score;

            facebookName.text = Localization.CurrentLanguage.GetString("LOADING");
            facebookImage.sprite = m_defaultPicture;

            FacebookPlugin.GetInstance().GetUserName(facebookId, this.addAction<string>((username) =>
                    {
                        if (facebookName)
                            facebookName.text = username;
                    }));
            
            FacebookPlugin.GetInstance().GetuserPicture(facebookId, this.addAction<Sprite>((sprite) =>
                    {
                        if (sprite != null && facebookImage)
                            facebookImage.sprite = sprite;
                    }));
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

	
}
