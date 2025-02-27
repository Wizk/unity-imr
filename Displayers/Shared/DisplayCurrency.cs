using TMPro;
using UnityEngine;

public class DisplayCurrency : MonoBehaviour, ITooltip
{
    [SerializeField] private TMP_Text lblAmount, lblGainPerSec;

    private TooltipCurrency tooltipInstance;

    public bool EnableTooltip { get; set; } = true;

    public Tooltip Tooltip
    {
        get
        {
            AudioController.PlaySoundBuffer(SharedAssets.audioTooltip);
            tooltipInstance = Instantiate(SharedAssets.tooltipCurrency, transform).GetComponent<TooltipCurrency>();
            return tooltipInstance;
        }
    }

    private void Update()
    {

    }
}
