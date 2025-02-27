using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ViewManager : SingleBehaviour<ViewManager>
{
    #region INSPECTOR

    public TooltipManager tooltip;
    public DialogMessageController message;
    public NotifyController notify;
    public GameObject spinner;
    public TMP_Text spinnerMessage;

    #endregion

    #region BEHAVIOURS

    protected override void InternalAwake() { }

    protected override void InternalOnDestroy() 
    {
        //var test = Assets.audioTest;
    }

    #endregion

    #region METHODS

    /*public static void DefaultMessage(string _title, string _body, string _label)
    {
        Instance.message.Show(new MessageData()
        {
            title = _title,
            body = _body,
            buttons = new List<MessageButtonData>()
            {
                new MessageButtonData()
                {
                    label = _label,
                    soundClick = ResourcesAsset.Instance.audioClick,
                },
            },
            closable = true,
            soundClose = ResourcesAsset.Instance.audioClose,
        });
    }*/

    public static void ConfimationMessage(string _title, string _body, Action _onClick)
    {
        ViewManager.Instance.message.Show(new MessageData()
        {
            title = _title,
            body = _body,
            closable = true,
            //soundClose = ResourcesAsset.Instance.audioClose,
            buttons = new List<MessageButtonData>()
            {
                new MessageButtonData()
                {
                    label = "NO", //Localization.TranslateDefault(Const.LS_MENU_BUTTON_NO),
                    //soundClick = ResourcesAsset.Instance.audioClick,
                },
                new MessageButtonData()
                {
                    label = "YES",//Localization.TranslateDefault(Const.LS_MENU_BUTTON_YES),
                    //soundClick = ResourcesAsset.Instance.audioClick,
                    onClick = _onClick,
                },
            },
        });
    }

    /*public static void DefaultNotify(string _title, string _body, Action onClick = null, float duration = 5)
    {
        if (!SettingsData.gameNotification)
            return;

        NotifyData notification = new NotifyData()
        {
            code = HashCode.Combine(_title, _body).ToString(),
            title = _title,
            body = _body,
            closable = true,
            duration = duration,
            soundShow = ResourcesAsset.Instance.audioClick,
            soundHide = ResourcesAsset.Instance.audioClose,
            onClick = onClick,
        };

        Instance.notify.Show(notification);
    }*/

    public static void LoadingSpinner(bool state, string message = null)
    {
        Instance.spinnerMessage.text = message.IsNullOrEmpty() ? "Default TODO" : message;
        Instance.spinner.SetActive(state);
    }

    #endregion
}
