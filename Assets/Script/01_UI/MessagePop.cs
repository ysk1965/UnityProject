using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessagePop : UEPopup
{

    /////////////////////////////////////////////////////////////
    /// public static

    public static MessagePop ShowPop(string msg, string leftBtnText = null, DelOnClickButton leftBtnCallback = null,
                                             string rightBtnText = null, DelOnClickButton rightBtnCallback = null, bool canClose = true)
    {
        MessagePop popup = UEPopup.GetInstantiateComponent<MessagePop>("Prefabs/UI/Popups/MessagePop");
        popup.Show(msg, leftBtnText, leftBtnCallback, rightBtnText, rightBtnCallback, canClose);
        return popup;
    }

    /////////////////////////////////////////////////////////////
    /// public

    public delegate void DelOnClickButton();

    public void Show(string msg, string button1Text, DelOnClickButton button1Callback, string button2Text, DelOnClickButton button2Callback, bool canClose)
    {
        // SoundManager.Instance.PlaySFX(SoundFX.rd_sfx_popup);
        base.Show();

        LanguageManager.Instance.SetText(this.titleText, LanguageID.COMMON_MESSAGE);
        LanguageManager.Instance.SetText(this.messageText, msg);

        this.button1Object.SetActive(button1Text != null);
        LanguageManager.Instance.SetText(this.button1Text, button1Text);
        this.onClickButton1Callback = button1Callback;

        this.button2Object.SetActive(button2Text != null);
        LanguageManager.Instance.SetText(this.button2Text, button2Text);
        this.onClickButton2Callback = button2Callback;

        this.canBackgroundClose = canClose;
        this.clostBtnObj.SetActive(canClose);
    }

    public void Hide()
    {
        base.Hide();
    }

    public void OnClickButton1()
    {
        SoundManager.Instance.PlayButtonClick();
        if (this.onClickButton1Callback != null)
            this.onClickButton1Callback();

        this.Hide();
    }

    public void OnClickButton2()
    {
        SoundManager.Instance.PlayButtonClick();
        if (this.onClickButton2Callback != null)
            this.onClickButton2Callback();

        this.Hide();
    }

    /////////////////////////////////////////////////////////////
    /// private

    [SerializeField] private Text titleText;
    [SerializeField] private Text messageText;
    [SerializeField] private Text button1Text;
    [SerializeField] private Text button2Text;

    [SerializeField] private GameObject button1Object;
    [SerializeField] private GameObject button2Object;
    [SerializeField] private GameObject clostBtnObj;

    private DelOnClickButton onClickButton1Callback;
    private DelOnClickButton onClickButton2Callback;

}
