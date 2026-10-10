using UnityEngine;
using System.Collections;

public class GameCenterView : MonoBehaviour 
{
    [SerializeField]
    GameObject m_gameCenterRoot;

    public void Show()
    {
        m_gameCenterRoot.SetActive(true);
    }

    public void Hide()
    {
        m_gameCenterRoot.SetActive(false);
    }

    public void ShowGCLeaderboard()
    {
        GoogleGamePlugin.GetInstance().ShowLb();
    }

    public void ShowGCAchievment()
    {
        GoogleGamePlugin.GetInstance().ShowAchiv();
    }

}
