using UnityEngine;
using System;
using System.Collections.Generic;

// Local replacement for the old SoomlaPlugin: keeps the same item ids and the
// same API used by the game (coins earned by playing, customization bought
// with coins), saved on the device through SecureData. No real-money store.
public class Inventory
{
    public const string ID_COINS = "coins";

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

    // Price in coins of each item that can be bought (same values as the 2016 store)
    static readonly Dictionary<string, int> s_prices = new Dictionary<string, int>()
    {
        { ID_CS_BG_BLACK, 0 },
        { ID_CS_BG_GREY, 4000 },
        { ID_CS_BG_RED, 4000 },
        { ID_CS_BG_ORANGE, 4000 },
        { ID_CS_BG_YELLOW, 4000 },
        { ID_CS_BG_GREEN, 4000 },
        { ID_CS_BG_CYAN, 4000 },
        { ID_CS_BG_BLUE, 4000 },
        { ID_CS_BG_PURPLE, 4000 },
        { ID_CS_BG_MAGENTA, 4000 },

        { ID_BS_NORMAL, 0 },
        { ID_BS_OCT, 8000 },
        { ID_BS_SQUARE, 8000 },
        { ID_BS_STAR, 16000 },
    };

    static Inventory s_instance;
    public static Inventory GetInstance()
    {
        if (s_instance == null)
            s_instance = new Inventory();

        return s_instance;
    }

    Inventory()
    {
        if (!CheckItem(ID_CS_BG_BLACK))
            GiveItem(ID_CS_BG_BLACK, 1);

        if (!CheckItem(ID_BS_NORMAL))
            GiveItem(ID_BS_NORMAL, 1);
    }

    public int Price(string itemId)
    {
        int price;
        return s_prices.TryGetValue(itemId, out price) ? price : 0;
    }

    public void GiveItem(string itemId, int amount)
    {
        SetBalance(itemId, ItemBalance(itemId) + amount);
    }

    public void TakeItem(string itemId, int amount)
    {
        SetBalance(itemId, Mathf.Max(0, ItemBalance(itemId) - amount));
    }

    public bool CheckItem(string itemId)
    {
        return ItemBalance(itemId) > 0;
    }

    public int ItemBalance(string itemId)
    {
        int balance;
        int.TryParse(SecureData.Get(SaveKeys.SK_INVENTORY_PREFIX + itemId, "0"), out balance);
        return balance;
    }

    public bool CanAfford(string itemId)
    {
        return ItemBalance(ID_COINS) >= Price(itemId);
    }

    // Buys an item with coins. result receives the item id on success; onFinished reports if it was bought.
    public void BuyItem(string itemId, Action<string> result, Action<bool> onFinished = null)
    {
        var bought = !CheckItem(itemId) && CanAfford(itemId);

        if (bought)
        {
            TakeItem(ID_COINS, Price(itemId));
            GiveItem(itemId, 1);

            if (result != null)
                result(itemId);
        }

        if (onFinished != null)
            onFinished(bought);
    }

    void SetBalance(string itemId, int balance)
    {
        SecureData.Save(SaveKeys.SK_INVENTORY_PREFIX + itemId, balance.ToString());
    }
}
