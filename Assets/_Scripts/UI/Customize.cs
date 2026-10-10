using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Customize : MonoBehaviour
{
    #region STATICS
    BackgroundController m_backgroundController { get { return BackgroundController.GetInstance(); } }
    Inventory m_inventory { get { return Inventory.GetInstance(); } }
    #endregion

    [SerializeField]
    Store m_store;

    [SerializeField]
    GameObject m_root;

    [SerializeField]
    Text m_coinsAmount;

    [SerializeField]
    ColorBlock m_selectedColor;

    [SerializeField]
    public ColorBlock m_unselectedColor;

    public void Show()
    {        
        m_root.SetActive(true);

        UpdateCoins();
    }

    public void Dismiss()
    {
        m_root.SetActive(false);
    }

    public void ShowCoinStore()
    {
        m_store.Show();
        Dismiss();
    }

    public void UpdateCoins()
    {
        m_coinsAmount.text = m_inventory.ItemBalance(Inventory.ID_COINS).ToString();
    }

    public ColorBlock GetColor(bool selected)
    {
        return selected ? m_selectedColor : m_unselectedColor;
    }


}
