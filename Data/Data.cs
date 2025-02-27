using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using UnityEngine;
using System;

public abstract class SingleObject<T> where T : new()
{
    public static T Instance;
    public abstract void SetInstance();

    public SingleObject() => SetInstance();
}

[Serializable]
public class ScriptableWrapper<T> : ScriptableObject where T : new()
{
    public T data;
}

[Serializable]
public class Data : SingleObject<Data>
{
    #region FIELDS

    public SettingsData settings = new();
    public RanksData ranks = new();

    #endregion

    #region PROXY

    public static SettingsData Settings
    {
        get => Instance.settings;
        set => Instance.settings = value;
    }
    public static RanksData Ranks => Instance.ranks;

    #endregion

    #region METHODS

    public override void SetInstance() => Instance = this;

    public static void Save()
    {
        string payload = JsonConvert.SerializeObject(Instance, Formatting.Indented);
        PlayerPrefs.SetString("data", payload);
        Debug.Log(payload);
    }

    public static void Load()
    {
        if (!PlayerPrefs.HasKey("data"))
            return;

        string payload = PlayerPrefs.GetString("data");
        JsonConvert.PopulateObject(PlayerPrefs.GetString("data"), Instance);
        Debug.Log(payload);
    }

    #endregion
}

[Serializable]
public class SettingsData
{
    #region FIELDS

    // Audio
    [Range(-80f, 20f)] public float volumeMusic = -40f, volumeSound = -40f;
    public bool musicMinimized = true, soundMinimized = true;
    // Screen
    [JsonConverter(typeof(StringEnumConverter))]
    public ScreenOrientation screenOrientation = ScreenOrientation.AutoRotation;
    public int screenResolutionId = 0;
    // Mobile
    public bool notifications = true;
    // Display
    public bool showTooltip = true, gameNotifications = true;
    // Currencies
    public bool currencyMass, currencyRage, currencyBlackHole; // TODO Hidden currencies

    #endregion

    #region PROPERTIES

    [JsonIgnore]
    public float VolumeMusic
    {
        get => volumeMusic;
        set => volumeMusic = Mathf.Clamp(value, -80f, 0f);
    }

    [JsonIgnore]
    public float VolumeSound
    {
        get => volumeSound;
        set => volumeSound = Mathf.Clamp(value, -80f, 0f);
    }

    [JsonIgnore]
    public bool MuteMusic
    {
        get => volumeMusic == -80;
        set => volumeMusic = value ? -80 : -40;
    }

    [JsonIgnore]
    public bool MuteSound
    {
        get => volumeSound == -80;
        set => volumeSound = value ? -80 : -40;
    }

    #endregion
}
