using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class LeaderboardView : MonoBehaviour 
{
    enum KIND
    {
        NONE = 0,
        FRIENDS = 1,
        GLOBAL = 2,
    }

    [SerializeField]
    ColorBlock m_activeColor;

    [SerializeField]
    ColorBlock m_defaultColor;

    [SerializeField]
    Button m_friendsButton;

    [SerializeField]
    Button m_globalButton;

    [SerializeField]
    GameObject m_leaderboardRoot;

    [SerializeField]
    Text m_gameNameText;

    [SerializeField]
    Text m_titleLeaderbordText;

    [SerializeField]
    ScrollRect m_cardsScrollRect;

    [SerializeField]
    Transform m_cardParent;

    [SerializeField]
    GameObject m_cardPrefab;

    [SerializeField]
    List<LeaderboardCard> m_cards;
    
    Pool<LeaderboardCard> m_cardsPool;

    GameManager.kind m_leaderboardKind;

    Leaderboard m_leaderboard;

    KIND m_leaderboardLoaded = KIND.NONE;

    public void ShowLeaderboard(GameManager.kind gameKind, Leaderboard leaaderboard)
	{      
        if(m_cardsPool == null)
            m_cardsPool = Pool<LeaderboardCard>.CreatePool(m_cards, m_cardPrefab, m_cardParent);

        m_leaderboardKind = gameKind;

        m_leaderboard = leaaderboard;

        m_leaderboardRoot.SetActive(true);

        m_leaderboardLoaded = KIND.NONE;

        FriendsLeaderboards();
    }

    public void HideLeaderboard()
    {
        m_leaderboardRoot.SetActive(false);
    }

    public void FriendsLeaderboards()
    {
        if (m_leaderboardLoaded == KIND.FRIENDS)
            return;

        m_leaderboardLoaded = KIND.FRIENDS;

        AdjustText();

        m_friendsButton.colors = m_activeColor;
        m_globalButton.colors = m_defaultColor;       

        ClearCards();

        SetupCards(m_leaderboard.FriendsLeaderboard);

        m_cardsScrollRect.verticalNormalizedPosition = 1;
    }

    public void GlobalLeaderboards()
    {
        if (m_leaderboardLoaded == KIND.GLOBAL)
            return;
        
        m_leaderboardLoaded = KIND.GLOBAL;

        AdjustText();

        m_friendsButton.colors = m_defaultColor;
        m_globalButton.colors = m_activeColor;

        ClearCards();


        SetupCards(m_leaderboard.WorldLeaderboard);

        m_cardsScrollRect.verticalNormalizedPosition = 1;
    }

    void SetupCards(List<Score> leaderboard)
    {
        for(int i=0; i<leaderboard.Count; i++)
        {
            var card = m_cardsPool.GetObject();
            var score = leaderboard[i];
            card.gameObject.SetActive(true);
            card.SetupCard(leaderboard.IndexOf(score), score.UserName, score.Value.ToString());
        }       
    }

    void ClearCards()
    {
        m_cardsPool.DeactivateWholePool();

        foreach(var card in m_cards)
        {
            card.gameObject.SetActive(false);
        }
    }

    void AdjustText()
    {
        if (m_gameNameText != null)
            m_gameNameText.text = m_leaderboardKind == GameManager.kind.Survival ? Localization.CurrentLanguage.GetString("SURVIVAL_GAME") : Localization.CurrentLanguage.GetString("TIMED_GAME");

        if (m_titleLeaderbordText != null)
            m_titleLeaderbordText.text = m_leaderboardLoaded == KIND.FRIENDS ? Localization.CurrentLanguage.GetString("FRIENDS") : Localization.CurrentLanguage.GetString("WORLD");
    }

    public void InviteFriends()
    {
        FacebookPlugin.GetInstance().FacebookInvite();
    }
}

