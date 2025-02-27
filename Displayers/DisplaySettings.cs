using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class DisplaySettings : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private Slider sldMusic;
    [SerializeField] private Slider sldSound;
    [SerializeField] private Toggle tglMusic, tglSound;
    [SerializeField] private TMP_Text lblVersion;
    [SerializeField] private TMP_Dropdown drpLanguage, drpOrientation, drpScreenMode, drpResolution, drpNotation;
    [SerializeField] private Toggle tglMusicMinimized, tglSoundMinimized;
    [Header("Display")]
    [SerializeField] private Toggle tglInGameNotif;
    [SerializeField] private Toggle tglAchNotif, tglTooltip, tglDim;
    [Header("Currency")]
    [SerializeField] private Toggle tglMass;
    [SerializeField] private Toggle tglRage, tglBlackHole;

    private SettingsData Settings => Data.Settings;

    private void Start()
    {
        List<Slider> sliders = new() { sldMusic, sldSound };
        List<Toggle> toggles = new() { tglMusicMinimized, tglSoundMinimized };

        sliders.ForEach(args => args.onValueChanged.AddListener(val => Save()));
        toggles.ForEach(args => args.onValueChanged.AddListener(val => Save()));

        lblVersion.text = Application.version.ToString();

        InitValues();
    }

    private void InitValues()
    {
        sldMusic.SetValueWithoutNotify(Settings.VolumeMusic);
        sldSound.SetValueWithoutNotify(Settings.VolumeSound);
        tglMusicMinimized.SetIsOnWithoutNotify(Settings.musicMinimized);
        tglMusicMinimized.SetIsOnWithoutNotify(Settings.soundMinimized);
    }

    public void ResetValues()
    {
        ViewManager.ConfimationMessage("Reset the settings", "Do you really want to reset the settings?", () =>
        {
            Data.Settings = new SettingsData();
            Data.Save();
            InitValues();
        });
    }

    /*private void InitDropdowns()
    {
#if UNITY_MOBILE
        drpOrientation.PopulateDropdown(Const.LS_OPT_SCREEN_ORIENTATION.Values.ToList());
        drpOrientation.value = Const.LS_OPT_SCREEN_ORIENTATION.Keys.ToList().IndexOf(Settings.screenOrientation);
#else
        drpResolution.PopulateDropdown(Const.SCREEN_RESOLUTIONS.Select(arg => $"{arg.width}x{arg.height}").ToList(), false);
        drpScreenMode.PopulateDropdown(Const.LS_OPT_SCREEN_MODE.Values.ToList());
        drpResolution.value = Settings.screenResolutionId;
        drpScreenMode.value = Const.LS_OPT_SCREEN_MODE.Keys.ToList().IndexOf(Screen.fullScreenMode);
#endif
        drpNotation.PopulateDropdown(Const.LS_OPT_NOTATIONS.Values.ToList());
        drpNotation.value = Const.LS_OPT_NOTATIONS.Keys.ToList().IndexOf(Settings.notation);
    }*/

    public void Save()
    {
        AudioController.VolumeMusic = Settings.VolumeMusic = sldMusic.value;
        AudioController.VolumeSound = Settings.VolumeSound = sldSound.value;

        tglMusic.isOn = Settings.VolumeMusic >= -75f;
        tglSound.isOn = Settings.VolumeSound >= -75f;

        Settings.musicMinimized = tglMusicMinimized.isOn;
        Settings.soundMinimized = tglSoundMinimized.isOn;

        Data.Save();
    }
}
