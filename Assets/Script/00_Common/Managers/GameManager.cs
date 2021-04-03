using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class GameManager : GameObjectSingleton<GameManager>
{

    /////////////////////////////////////////////////////////////
    // public

    public string unityDeviceId = "";


    /////////////////////////////////////////////////////////////
    // protected
    protected void Start()
    {
        NetworkManager.Instance.UpdateServerTime();

#if UNITY_ANDROID
        AntiCheatManager.Instance.Request((h) =>
        {
            this.CheckVersion(h);
        });
#else
        this.CheckVersion(null);
#endif

        // if (!Application.identifier.Equals(StringConst.PACKAGE))
        // {
        //     yield break;
        // }
    }




    // public void SendCSMail(int uid)
    // {
    //     string mailto = "cs_playgrounds@cookapps.com";
    //     string subject = this.EscapeURL("Random Royale Contact");
    //     string body = this.EscapeURL
    //         (
    //          "Please fill in the content here.\n\n\n\n" +
    //          "________" +
    //          "UID : " + uid + "\n\n" +
    //          "VER : " + Application.version + "\n\n" +
    //          "Device Model : " + SystemInfo.deviceModel + "\n\n" +
    //          "Device OS : " + SystemInfo.operatingSystem + "\n\n" +
    //          "________"
    //         );

    //     Application.OpenURL("mailto:" + mailto + "?subject=" + subject + "&body=" + body);
    // }

    // private string EscapeURL(string url)
    // {
    //     return UnityWebRequest.EscapeURL(url).Replace("+", "%20");
    // }

    public void ShowMoveToMain(string msg)
    {
        // if (LanguageManager.Instance != null)
        // {
        //     string msg = LanguageManager.Instance.GetText(LanguageID.MSG_CHECK_NETWORK);
        //     MessagePop.ShowPop(msg).SetOnCompleteHide(() =>
        //     {
        //         GameObject.Destroy(DontDestroyObject.Instance.gameObject);
        //         UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        //     });
        // }
        // else
        // {
        //     GameObject.Destroy(DontDestroyObject.Instance.gameObject);
        //     UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        // }
    }

    public void ShowQuit(string msg)
    {
        // string confirm = LanguageManager.Instance.GetConfirmText();
        // MessagePop.ShowPop(msg, confirm, () =>
        // {
        //     Application.Quit(0);
        // }, canClose: false);
    }

    public void ShowMoveToMarket(string msg)
    {
        // string confirm = LanguageManager.Instance.GetConfirmText();
        // MessagePop.ShowPop(msg, confirm, () =>
        //  {
        //      Application.OpenURL(StringConst.MARKET_URL);
        //  }, canClose: false);
    }

    private int getVersionTryCount = 0;

    private void CheckVersion(string key)
    {
        this.getVersionTryCount++;

        LoadingPop.HidePopup();
        NetworkManager.Instance.CheckVersion(key, (state, response) =>
        {
            if (state == NetworkState.NORMAL)
            {
                Debug.Log("Current Server Spec Verison " + response.spec_version);

                SpecDataManager.Instance.SpecVersion = response.spec_version;
                int savedVersion = Preference.LoadPreference(Pref.SPEC_VERSION, 0);
                string specJson = Preference.LoadPreference(Pref.SPEC_DATA, string.Empty);

                if (response.status == 1)
                {
                    LoadingPop.HidePopup();
                    string msg = LanguageManager.Instance.GetNeedUpdateText();
                    string confirm = LanguageManager.Instance.GetConfirmText();
                    MessagePop.ShowPop(msg, confirm, () =>
                     {
                         Application.OpenURL(StringConst.MARKET_URL);
                     }, canClose: false);
                }
                else if (response.status == 3)
                {
                    LoadingPop.HidePopup();
                    string msg = LanguageManager.Instance.GetServerCheckText();
                    string confirm = LanguageManager.Instance.GetConfirmText();
                    MessagePop.ShowPop(msg, confirm, () =>
                    {
                        Application.Quit(0);
                    }, canClose: false);
                }
                else if (response.status == 6)
                {
                    LoadingPop.HidePopup();
                    string msg = LanguageManager.Instance.GetNeedLoginText();
                    string confirm = LanguageManager.Instance.GetConfirmText();
                    MessagePop.ShowPop(msg, confirm, () =>
                    {
                        Application.Quit(0);
                    }, canClose: false);
                }
                else
                {
                    if (SpecDataManager.Instance.SpecVersion == savedVersion && !string.IsNullOrEmpty(specJson))
                    {
                        SpecDataManager.Instance.SetServerSpecData(JsonConvert.DeserializeObject<ServerSpecData>(specJson));
                        this.Login();
                    }
                    else
                    {
                        this.LoadSpecData();
                    }
                }
            }
            else
            {
                if (this.getVersionTryCount < 3)
                {
                    Run.After(1f, () => { this.CheckVersion(key); });
                }
                else
                {
                    LoadingPop.HidePopup();
                    MessagePop.ShowPop(LanguageManager.Instance.GetTryAgainText()).SetOnCompleteHide(() =>
                    {
                        GameObject.Destroy(DontDestroyObject.Instance.gameObject);
                        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                    });
                }
            }
        });
    }

    private void Login()
    {
        NetworkManager.Instance.GetConfig((state, data) =>
        {
            DataManager.Instance.SetConfig(data.config);
            string socialID = Preference.LoadPreference(Pref.SOCIAL_ID, string.Empty);
            if (!string.IsNullOrEmpty(socialID))
            {
                LoadingPop.HidePopup();
                this.SoicalLogin();
            }
            else
            {
                this.GuestLogin();
            }
        });
    }

    private void GuestLogin()
    {
        //LOGIN TO SERVER..
        NetworkManager.Instance.AuthLogin("guest", "guest", (state, response) =>
        {
            if (state == NetworkState.NORMAL)
            {
                if (response.isBanned == 1)
                {
                    LoadingPop.HidePopup();
                    this.ShowBanned(response.uid);
                }
                else
                {
                    Preference.SavePreference(Pref.SOCIAL_ID, string.Empty);
                    this.GetLobbyInfo();
                }
            }
            else
            {
                LoadingPop.HidePopup();
                MessagePop.ShowPop(LanguageManager.Instance.GetTryAgainText()).SetOnCompleteHide(() =>
                {
                    GameObject.Destroy(DontDestroyObject.Instance.gameObject);
                    UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                });
            }
        });
    }

    private void LoadSpecData()
    {
        //DOWNLOADING DATA..
        NetworkManager.Instance.GetSpecData(SpecDataManager.Instance.SpecVersion, (state, response) =>
        {
            if (state == NetworkState.NORMAL)
            {
                ServerSpecData data = JsonConvert.DeserializeObject<ServerSpecData>(response);
                SpecDataManager.Instance.SetServerSpecData(data);
                Preference.SavePreference(Pref.SPEC_VERSION, SpecDataManager.Instance.SpecVersion);
                Preference.SavePreference(Pref.SPEC_DATA, response);
                this.Login();
            }
            else
            {
                MessagePop.ShowPop(LanguageManager.Instance.GetTryAgainText()).SetOnCompleteHide(() =>
                {
                    GameObject.Destroy(DontDestroyObject.Instance.gameObject);
                    UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                });
            }
        });
    }

    private void GetLobbyInfo()
    {
        NetworkManager.Instance.LobbyInfo((state, response) =>
        {
            LoadingPop.HidePopup();
            if (state == NetworkState.NORMAL)
            {
                DataManager.Instance.SetServerData(response);
                // GameUI.Instance.Refresh();

                if (DataManager.Instance.UserData.isCheater)
                {
                    this.ShowBanned(DataManager.Instance.UserData.uid);
                }
                else
                {
                    // if (!DataManager.Instance.UserData.nVersion.Equals(Application.version))
                    // {
                    //     NoticePop.ShowPopup();
                    // }
                }
            }
            else
            {
                MessagePop.ShowPop(LanguageManager.Instance.GetTryAgainText()).SetOnCompleteHide(() =>
                {
                    GameObject.Destroy(DontDestroyObject.Instance.gameObject);
                    UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                });
            }
        });
    }

    private void ShowBanned(int uid)
    {
        string msg = LanguageManager.Instance.GetAccountLockedText();
        string confirm = LanguageManager.Instance.GetConfirmText();
        string sendMail = LanguageManager.Instance.GetContactUSText();
        MessagePop.ShowPop(msg, confirm, () =>
        {
            Application.Quit(0);
        }, sendMail, () =>
        {
            this.SendCSMail(uid);
        }, canClose: false);
    }

    private void SendCSMail(int uid)
    {
        string mailto = "cs_playgrounds@cookapps.com";
        string subject = this.EscapeURL("Epic Fantasy");
        string body = this.EscapeURL
            (
             "Please fill in the content here.\n\n\n\n" +
             "________" +
             "UID : " + uid + "\n\n" +
             "VER : " + Application.version + "\n\n" +
             "Device Model : " + SystemInfo.deviceModel + "\n\n" +
             "Device OS : " + SystemInfo.operatingSystem + "\n\n" +
             "________"
            );

        Application.OpenURL("mailto:" + mailto + "?subject=" + subject + "&body=" + body);
    }

    private string EscapeURL(string url)
    {
        return UnityWebRequest.EscapeURL(url).Replace("+", "%20");
    }

    private void SoicalLogin()
    {
        // GameSocialManager.Instance.LoginToSocial((uid, name, platform) =>
        // {
        //     if (string.IsNullOrEmpty(uid))
        //     {
        //         LoadingPop.HidePopup();
        //         ToastPop.ShowPop(LanguageID.MSG_FAIL_SOCIAL_LOGIN);
        //     }
        //     else
        //     {
        //         NetworkManager.Instance.AuthLogin(uid, platform, (state, response) =>
        //         {
        //             if (state == NetworkState.NORMAL)
        //             {
        //                 if (response.isBanned == 1)
        //                 {
        //                     LoadingPop.HidePopup();
        //                     this.ShowBanned(response.uid);
        //                 }
        //                 else
        //                 {
        //                     Preference.SavePreference(Pref.GUEST_ID, string.Empty);
        //                     Preference.SavePreference(Pref.SOCIAL_ID, uid);
        //                     this.guestLoginButtonObj.SetActive(false);
        //                     this.googleLoginButtonObj.SetActive(false);
        //                     this.appleLoginButtonObj.SetActive(false);
        //                     this.GetLobbyInfo();
        //                 }
        //             }
        //             else
        //             {
        //                 LoadingPop.HidePopup();
        //                 MessagePop.ShowPop(LanguageManager.Instance.GetTryAgainText()).SetOnCompleteHide(() =>
        //                 {
        //                     GameObject.Destroy(DontDestroyObject.Instance.gameObject);
        //                     UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        //                 });
        //             }
        //         });
        //     }
        // });
    }
}
