/*
using UnityEngine;
using System.Collections;
using Soomla;
using Soomla.Store;
using System;
using System.Collections.Generic;

public class SoomlaPlugin
{
    public const string ID_COINS = "coins";
    public const string ID_COINS_PACK1 = "coins_pack1";
    public const string ID_COINS_PACK2 = "coins_pack2";
    public const string ID_COINS_PACK3 = "coins_pack3";

    public const string ID_CS_BG_BLACK = "cs_bg_black";
    public const string ID_CS_BG_GREY = "cs_bg_grey";
    public const string ID_CS_BG_RED = "cs_bg_red";
    public const string ID_CS_BG_ORANGE = "cs_bg_orange";
    public const string ID_CS_BG_YELLOW = "cs_bg_yellow";
    public const string ID_CS_BG_GREEN = "cs_bg_green";
    public const string ID_CS_BG_CYAN = "cs_bg_cyan";
    public const string ID_CS_BG_BLUE = "cs_bg_blue";
    public const string ID_CS_BG_PURPLE = "cs_bg_purple";
    public const string ID_CS_BG_MAGENTA = "cs_bg_magenta";

    public const string ID_BS_NORMAL = "cs_bs_normal";
    public const string ID_BS_OCT = "cs_bs_octagonal";
    public const string ID_BS_STAR = "cs_bs_star";
    public const string ID_BS_SQUARE = "cs_bs_square";

    public const string ID_REMOVE_ADS = "remove_ads";


    class BubbitSoomlaStore : IStoreAssets
    {
        public VirtualCategory[] GetCategories()
        {
            return new VirtualCategory[]{ };
        }

        public VirtualCurrency[] GetCurrencies()
        {
            return new VirtualCurrency[] { Coins };
        }

        public VirtualCurrencyPack[] GetCurrencyPacks()
        {
            return new VirtualCurrencyPack[] { CoinsPack1, CoinsPack2, CoinsPack3 };
        }

        public VirtualGood[] GetGoods()
        {
            return new VirtualGood[] { RemoveAdsVG, BlackBackground, GreyBackground, RedBackground, OrangeBackground, YellowBackground, GreenBackground, CyanBackground, BlueBackground, PurpleBackground, MagentaBackground, NormalBubble, OctagonalBubble, StarBubble, SquareBubble };
        }

        public int GetVersion()
        {
            return 6;
        }

        public static VirtualCurrency Coins = new VirtualCurrency(ID_COINS, "coins", ID_COINS);

        
        public static MarketItem MarketCoinPack1 = new MarketItem(ID_COINS_PACK1, 0);
        public static MarketItem MarketCoinPack2 = new MarketItem(ID_COINS_PACK2, 0);
        public static MarketItem MarketCoinPack3 = new MarketItem(ID_COINS_PACK3, 0);

        public static VirtualCurrencyPack CoinsPack1 = new VirtualCurrencyPack(ID_COINS_PACK1, "Coin Pack 1", ID_COINS_PACK1, 5000, ID_COINS, new PurchaseWithMarket(MarketCoinPack1));
        public static VirtualCurrencyPack CoinsPack2 = new VirtualCurrencyPack(ID_COINS_PACK2, "Coin Pack 2", ID_COINS_PACK2, 11000, ID_COINS, new PurchaseWithMarket(MarketCoinPack2));
        public static VirtualCurrencyPack CoinsPack3 = new VirtualCurrencyPack(ID_COINS_PACK3, "Coin Pack 3", ID_COINS_PACK3, 18000, ID_COINS, new PurchaseWithMarket(MarketCoinPack3));

        public static LifetimeVG BlackBackground = new LifetimeVG( ID_CS_BG_BLACK, "Background Color", ID_CS_BG_BLACK, new PurchaseWithVirtualItem(ID_COINS, 0));
        public static LifetimeVG GreyBackground = new LifetimeVG( ID_CS_BG_GREY, "Background Color", ID_CS_BG_GREY, new PurchaseWithVirtualItem(ID_COINS, 4000));
        public static LifetimeVG RedBackground = new LifetimeVG( ID_CS_BG_RED, "Background Color", ID_CS_BG_RED, new PurchaseWithVirtualItem(ID_COINS, 4000));
        public static LifetimeVG OrangeBackground = new LifetimeVG( ID_CS_BG_ORANGE, "Background Color", ID_CS_BG_ORANGE, new PurchaseWithVirtualItem(ID_COINS, 4000));
        public static LifetimeVG YellowBackground = new LifetimeVG( ID_CS_BG_YELLOW, "Background Color", ID_CS_BG_YELLOW, new PurchaseWithVirtualItem(ID_COINS, 4000));
        public static LifetimeVG GreenBackground = new LifetimeVG( ID_CS_BG_GREEN, "Background Color", ID_CS_BG_GREEN, new PurchaseWithVirtualItem(ID_COINS, 4000));
        public static LifetimeVG CyanBackground = new LifetimeVG( ID_CS_BG_CYAN, "Background Color", ID_CS_BG_CYAN, new PurchaseWithVirtualItem(ID_COINS, 4000));
        public static LifetimeVG BlueBackground = new LifetimeVG( ID_CS_BG_BLUE, "Background Color", ID_CS_BG_BLUE, new PurchaseWithVirtualItem(ID_COINS, 4000));
        public static LifetimeVG PurpleBackground = new LifetimeVG(ID_CS_BG_PURPLE, "Background Color", ID_CS_BG_PURPLE, new PurchaseWithVirtualItem(ID_COINS, 4000));
        public static LifetimeVG MagentaBackground = new LifetimeVG(ID_CS_BG_MAGENTA, "Background Color", ID_CS_BG_MAGENTA, new PurchaseWithVirtualItem(ID_COINS, 4000));

        public static LifetimeVG NormalBubble = new LifetimeVG(ID_BS_NORMAL, "Bubble Skin", ID_BS_NORMAL, new PurchaseWithVirtualItem(ID_COINS, 0));
        public static LifetimeVG OctagonalBubble = new LifetimeVG(ID_BS_OCT, "Bubble Skin", ID_BS_OCT, new PurchaseWithVirtualItem(ID_COINS, 8000));
        public static LifetimeVG SquareBubble = new LifetimeVG(ID_BS_SQUARE, "Bubble Skin", ID_BS_SQUARE, new PurchaseWithVirtualItem(ID_COINS, 8000));

        public static LifetimeVG StarBubble = new LifetimeVG(ID_BS_STAR, "Bubble Skin", ID_BS_STAR, new PurchaseWithVirtualItem(ID_COINS, 16000));

        public static LifetimeVG RemoveAdsVG = new LifetimeVG(ID_REMOVE_ADS, "Remove all ads", ID_REMOVE_ADS, new PurchaseWithMarket(ID_REMOVE_ADS, 1));
    }

    static SoomlaPlugin s_instance;
    public static SoomlaPlugin GetInstance()
    {
        if (s_instance == null)
        {
            s_instance = new SoomlaPlugin();
        }

        return s_instance;
    }

    Action<string> m_result;
    Action<bool> m_onFinished;
    string m_curId;

    SoomlaPlugin()
    {
        SoomlaStore.Initialize(new BubbitSoomlaStore());        
        StoreEvents.OnUnexpectedStoreError += onUnexpectedStoreError;
        StoreEvents.OnMarketPurchaseCancelled += onMarketPurchaseCanceled;
        StoreEvents.OnMarketPurchaseDeferred += onMarketPurchaseDeferred; 
        StoreEvents.OnItemPurchased += onItemPurchased;

        if (!CheckItem(SoomlaPlugin.ID_CS_BG_BLACK))
        {
            GiveItem(SoomlaPlugin.ID_CS_BG_BLACK, 1);
        }

        if (!CheckItem(SoomlaPlugin.ID_BS_NORMAL))
        {
            GiveItem(SoomlaPlugin.ID_BS_NORMAL, 1);
        }
    }

    public void BuyItem(string id, Action<string> result, Action<bool> onFinished = null)
    {
        m_result = result;
        m_onFinished = onFinished;
        m_curId = id;
        StoreInventory.BuyItem(id, id);
    }


    public void GiveItem(string id, int amount)
    {
        StoreInventory.GiveItem(id, amount);
    }

    public void TakeItem(string id, int amount)
    {
        StoreInventory.TakeItem(id, amount);
    }

    public bool CheckItem(string itemId)
    {
        return StoreInventory.GetItemBalance(itemId) > 0;
    }

    public int ItemBalance(string itemId)
    {
        return StoreInventory.GetItemBalance(itemId);
    }

    public string GetStorePrice(string itemId)
    {
        var item = ((PurchaseWithMarket)StoreInfo.GetPurchasableItemWithProductId(itemId).PurchaseType).MarketItem;

        return item.MarketPriceAndCurrency;
    }

    public bool CanAfford(string itemId)
    {
        return StoreInventory.CanAfford(itemId);
    }

    public void onItemPurchased(PurchasableVirtualItem pvi, string payload)
    {
        if (pvi.ItemId.Equals(SoomlaPlugin.ID_REMOVE_ADS))
            AppodealPlugin.GetInstance().HideBanner();

        if(m_result != null)
            m_result(pvi.ItemId);

        if(m_onFinished != null)
            m_onFinished(true);
    }

    public void onUnexpectedStoreError(int code)
    {
        if(code == 3)
        {
            if (StoreInventory.GetItemBalance(m_curId) >= 1 && m_result != null)
                m_result(m_curId);

			if (StoreInventory.GetItemBalance(SoomlaPlugin.ID_REMOVE_ADS) >= 1)
				AppodealPlugin.GetInstance().HideBanner();
        }

        if(m_onFinished != null)
            m_onFinished(false);

    }

    public void onMarketPurchaseCanceled(PurchasableVirtualItem pvi)
    {
        if(m_onFinished != null)
            m_onFinished(false);
    }

    public void onMarketPurchaseDeferred(PurchasableVirtualItem pvi, string payload)
    {       
        if(m_onFinished != null)
            m_onFinished(false);
    }

    public void Restore()
    {
        SoomlaStore.RestoreTransactions();
    }


}
*/