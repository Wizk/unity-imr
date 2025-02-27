using UnityEngine;
using TMPro;

public class TooltipCurrency : Tooltip
{
    [SerializeField] private TMP_Text lblName;
    [SerializeField] private TMP_Text lblDesc;

    public override void Initialize()
    {
        lblName.text = "test";
        lblDesc.text = "desc";
    }
}
