using UnityEngine;
using UnityEngine.Localization;

public class SharedAssets : SingleBehaviour<SharedAssets>
{
    protected override void InternalAwake() { }
    protected override void InternalOnDestroy() { }

    public SharedAssetsData data;
    public static GameObject tooltipCurrency => Instance.data.tooltipCurrency;
    public static AudioClip audioTooltip => Instance.data.audioTooltip;

}