using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class Const
{
    #region CONFIG

    public const string ENCRYPTION_KEY = "TODO";

    public const int LOCAL_VERSION = 220;
    public const int NEWS_VERSION = 220;

    public const uint STEAM_APP_ID = 2763740;
    public const string STEAM_APP_URL = "https://store.steampowered.com/app/2763740";


#if UNITY_ANDROID
    public const string CURRENT_STORE = "android";
    public const string CURRENT_URL = "https://play.google.com/store/apps/details?id=com.oninou.revolution";
#elif UNITY_IOS
    public const string CURRENT_STORE = "ios";
    public const string CURRENT_URL = "https://apps.apple.com/us/app/revolution-idle/id6475233313";
#else
    public const string CURRENT_STORE = "steam";
    public const string CURRENT_URL = "https://store.steampowered.com/app/2763740/Revolution_Idle/";
#endif

#if UNITY_WEBGL
    public const string KEY_PLAYER_PREF_GAME_DATA = "game_data_dev";
    public const string KEY_PLAYER_PREF_INVENTORY = "inventory_dev";
    public const string KEY_PLAYER_PREF_CONFIG = "config_dev";
#else
    public const string KEY_PLAYER_PREF_GAME_DATA = "game_data";
    public const string KEY_PLAYER_PREF_INVENTORY = "inventory";
    public const string KEY_PLAYER_PREF_CONFIG = "config";
    #endif

    public static readonly NumberFormatInfo NFI = new NumberFormatInfo() { NumberDecimalSeparator = "." };

    public static readonly List<Resolution> SCREEN_RESOLUTIONS = new List<Resolution>()
    {
        new Resolution() { width = 1280, height = 720, refreshRateRatio = new RefreshRate() { numerator = 60, denominator = 1 } },
        new Resolution() { width = 1600, height = 1200, refreshRateRatio = new RefreshRate() { numerator = 60, denominator = 1 } },
        new Resolution() { width = 1920, height = 1080, refreshRateRatio = new RefreshRate() { numerator = 60, denominator = 1 } },
        new Resolution() { width = 2560, height = 1440, refreshRateRatio = new RefreshRate() { numerator = 60, denominator = 1 } },
        new Resolution() { width = 3840, height = 2160, refreshRateRatio = new RefreshRate() { numerator = 60, denominator = 1 } },
    };

    public const double START_PACK_DURATION = 172800d; // in seconds
    public static double[] THRESHOLD_ASK_START_PACK = new double[] { 1440, 2160, 2880 }; // in minutes
#if USE_STEAM
    public static List<double> THRESHOLD_ASK_REVIEW = new List<double> { 240d, 360d, 480d };
#else
    public static List<double> THRESHOLD_ASK_REVIEW = new List<double> { 1d, 10d, 48d };
#endif

    #endregion

    #region LINKS

    //public const string LINK_SIDEBAR_MAIN = "SIDEBAR_MAIN";

    #endregion

#if UNITY_ANDROID || UNITY_IOS
    public const float DOUBLE_CLICK_DELAY = 0.4f;
#else
    public const float DOUBLE_CLICK_DELAY = 0.2f;
    #endif
}