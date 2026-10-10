using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RateMyApp 
{
    public static void IncreasePlayerValue()
    {
        if (SecureData.Get(SaveKeys.SK_MAY_ASK_RATE, true))
        {
            var value = 0;
            int.TryParse(SecureData.Get(SaveKeys.SK_PLAYER_RATE_VALUE, "0"), out value);
            value++;
            SecureData.Save(SaveKeys.SK_PLAYER_RATE_VALUE, value.ToString());
        }
    }


    public static void ShowRateMyapp()
    {
        if (SecureData.Get(SaveKeys.SK_MAY_ASK_RATE, true))
        {
            var value = 0;
           
            int.TryParse(SecureData.Get(SaveKeys.SK_PLAYER_RATE_VALUE, "0"), out value);
            if (value > 50)
            {
                PopupController.GetInstance().Show(Localization.CurrentLanguage.GetString("RATE_MY_APP"), 
                    () =>
                    {
                        UnityEngine.Analytics.Analytics.CustomEvent("RateMyApp", new Dictionary<string, object>
                            {
                                { "rated", "true" },
                            });

                        SecureData.Save(SaveKeys.SK_MAY_ASK_RATE, false);

                        Application.OpenURL(GameConstants.StoreLink);
                    }, 
                    () =>
                    {

                        UnityEngine.Analytics.Analytics.CustomEvent("RateMyApp", new Dictionary<string, object>
                            {
                                { "rated", "false" },
                            });

                        SecureData.Save(SaveKeys.SK_MAY_ASK_RATE, false);
                    });
            }
        }
    }
}

