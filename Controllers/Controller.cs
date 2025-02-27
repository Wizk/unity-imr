using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;

[CreateAssetMenu]
public class Controller : ScriptableObject
{
    #region VIEWS

    //public void ShowPromote() => GameController.Instance.viewPromote.Select();

    #endregion

    #region APPLICATION

    public void OpenURL(string url) => Application.OpenURL(url);
    
    public static void CloseApplication()
    {
        ViewManager.ConfimationMessage("Quit the game", "Do you really, really want to leave?", () =>
        {
            ViewManager.LoadingSpinner(true, "Save in progress...");
            Application.Quit();
        });
    }

    public static void Save()
    {
        Data.Save();
    }

    public static void Load()
    {
        Data.Load();
    }

    public void AskReview()
    {
        ViewManager.ConfimationMessage("Rate the game", "Do you want to rate IMR?", () => RequestStoreReview());
    }

    public void RequestStoreReview()
    {
#if UNITY_STANDALONE
        OpenURL(Const.STEAM_APP_URL);
#elif UNITY_ANDROID || UNITY_IOS
        Review.RequestStoreReview();
#endif
    }

    #endregion

    #region IAP

    //public void BuyRemoveAds() => IAPManager.Instance.BuyRemoveAds();

    public void RestorePurchase()
    {
        //ViewManager.LoadingSpinner(true, Localization.TranslateDefault(Const.LS_SPINNER_MSG_RESTORE_PURCHASE));
        //IAPManager.RestorePurchases();
    }

    #endregion
}
