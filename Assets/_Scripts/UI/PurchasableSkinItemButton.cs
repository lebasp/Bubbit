using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PurchasableSkinItemButton : MonoBehaviour
{
    #region STATICS
    SkinController m_skinController { get { return SkinController.GetInstance(); } }
    Inventory m_inventory { get { return Inventory.GetInstance(); } }
    PopupController m_popupController { get { return PopupController.GetInstance(); } }
    #endregion

    [SerializeField]
    Customize m_customize;

    [SerializeField]
    string itemId;

    [SerializeField]
    GameObject m_lock;

    [SerializeField]
    GameObject m_coins;

    [SerializeField]
    bool isSkin;

    [SerializeField]
    Button m_button;

    void Start()
    {
        if (m_inventory.CheckItem(itemId))
            UnlockColor();

        m_skinController.OnSelect += SelectButton;
        m_skinController.OnUnselect += UnselectButton;

        SelectButton(m_skinController.EquippedBG());
        SelectButton(m_skinController.EquippedBubbleForm());
    }

    void SelectButton(string id)
    {        
        if (itemId.Equals(id))
            m_button.colors =   m_customize.GetColor(true);
    }

    void UnselectButton(string id)
    {        
        if (itemId.Equals(id))
            m_button.colors =   m_customize.GetColor(false);
    }

    public void SelectColor()
    {
        if (m_inventory.CheckItem(itemId))
        {
            if (isSkin)
                m_skinController.SetupSkin(itemId);
            else
                m_skinController.SetupBackground(itemId);          
        }
        else if (!m_inventory.CanAfford(itemId))
        {
            PopupController.GetInstance().Show(Localization.CurrentLanguage.GetString("NOT_ENOUGH"), () => { });
        }
        else
        {
            var confirmMessage = string.Format(Localization.CurrentLanguage.GetString("CONFIRM_PURCHASE"), m_inventory.Price(itemId));

            m_popupController.Show(confirmMessage, () =>
                {
                    m_inventory.BuyItem(itemId, (payload) =>
                        {
                            UnlockColor();
                            m_customize.UpdateCoins();
                            SelectColor();
                        });

                }, () =>
                {
                });     
        }
    }

    public void UnlockColor()
    {
        m_lock.SetActive(false);
        m_coins.SetActive(false);
    }

    void OnDestroy()
    {
        m_skinController.OnSelect -= SelectButton;
        m_skinController.OnUnselect -= UnselectButton;
    }

}
