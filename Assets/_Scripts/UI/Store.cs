using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class Store : MonoBehaviour 
{
    #region STATICS
    //SoomlaPlugin m_soomlaPlugin { get { return SoomlaPlugin.GetInstance(); } }
    #endregion

	[SerializeField]
	GameObject m_store;

    [SerializeField]
    GameObject m_removeAds;

    [SerializeField]
    Text m_coinAmount;
   
    [SerializeField]
    Text m_noAdsPrice;

    [SerializeField]
    Text m_coinsPack1Price;

    [SerializeField]
    Text m_coinsPack2Price;

    [SerializeField]
    Text m_coinsPack3Price;

    [SerializeField]
    GameObject m_loading;

    [SerializeField]
    GameObject m_rewardVideoButton;

	public void Show()
	{
        /*
		m_store.SetActive (true);

        m_removeAds.SetActive(!m_soomlaPlugin.CheckItem(SoomlaPlugin.ID_REMOVE_ADS));

        m_loading.SetActive(false);

        UpdateAmount();

        m_coinsPack1Price.text = m_soomlaPlugin.GetStorePrice(SoomlaPlugin.ID_COINS_PACK1);
        m_coinsPack2Price.text = m_soomlaPlugin.GetStorePrice(SoomlaPlugin.ID_COINS_PACK2); 
        m_coinsPack3Price.text = m_soomlaPlugin.GetStorePrice(SoomlaPlugin.ID_COINS_PACK3);     

        CheckVideoReward();
        */
	}

    void UpdateAmount()
    {
        //m_coinAmount.text = m_soomlaPlugin.ItemBalance(SoomlaPlugin.ID_COINS).ToString();
    }

    public void BuyItem(string itemId)
    {
        /*
        m_loading.SetActive(true);

        m_soomlaPlugin.BuyItem(itemId, (result) =>
            {                
                if(itemId.Equals(SoomlaPlugin.ID_REMOVE_ADS))
                {
                    AppodealPlugin.GetInstance().HideBanner();
                    m_removeAds.SetActive(false);
                }
                else
                    UpdateAmount();
            }, (purchased) =>
            {
                
                UnityEngine.Analytics.Analytics.CustomEvent("Purchase", new Dictionary<string, object>
                    {
                        { SoomlaPlugin.ID_REMOVE_ADS, purchased.ToString() },
                    });

                m_loading.SetActive(false);
            });
            */
    }

	public void Dismiss()
	{
        StopAllCoroutines();
        m_store.SetActive (false);
	}

    void CheckVideoReward()
    {
        AppodealPlugin.GetInstance().CheckRewardVideo(this.addAction(() =>
                {
                    m_rewardVideoButton.SetActive(true);
                }));
    }

    public void RewardVideo()
    { 
        /*s
        AppodealPlugin.GetInstance().ShowRewardVideo(this.addAction(() =>
                {
                    m_rewardVideoButton.SetActive(false);

                    var coins = UnityEngine.Random.Range(400, 600);
                    m_soomlaPlugin.GiveItem(SoomlaPlugin.ID_COINS, coins); 

                    PopupController.GetInstance().Show(string.Format(Localization.CurrentLanguage.GetString("COINS_RECEIVED"), coins), () =>
                        {
                            UpdateAmount();

                            GoogleGamePlugin.GetInstance().PostAchievement(GooglePlayIds.achievement_video_viewer); 
                        });

                }));
                */
    }

}
