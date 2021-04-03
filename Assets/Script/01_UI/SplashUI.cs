using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Playables;

public class SplashUI : MonoBehaviour
{
//     //////////////////////////////////////////////////////////////////////////////
//     //public

//     public void OnClickSocialLogin()
//     {
//         SoundManager.Instance.PlayButtonClick();
//         this.SoicalLogin();
//     }

//     public void OnClickGuestLogin()
//     {
//         SoundManager.Instance.PlayButtonClick();
//         this.GuestLogin();
//     }

//     //////////////////////////////////////////////////////////////////////////////
//     //protected

//     protected void Awake()
//     {
//         Screen.sleepTimeout = SleepTimeout.NeverSleep;
//         Application.targetFrameRate = 60;
//         if (Application.platform == RuntimePlatform.IPhonePlayer)
//             Cookapps.Analytics.CAppEvent.Initialize("f9af632f4cb1252191356b0b245b3a08");
//         else
//             Cookapps.Analytics.CAppEvent.Initialize("e129b57d73177cc9e0c79eb1ef6e778f");
//     }

//     protected IEnumerator Start()
//     {
//         SoundManager.Instance.SetBGMVolume(Preference.LoadPreference(Pref.BGM_V, 0.8f));
//         SoundManager.Instance.SetSFXVolume(Preference.LoadPreference(Pref.SFX_V, 1f));

//         SoundManager.Instance.PlayBGM(SoundBGM.bgm_title);
//         Cookapps.Analytics.CAppEvent.ReportProgress(0);


// #if USE_SRDEBUG
//         if (!Application.isEditor) SRDebug.Init();
// #endif

// #if UNITY_ANDROID
//         AntiCheatManager.Instance.Request((h) =>
//         {
//             this.CheckVersion(h);
//         });
// #else
//         this.CheckVersion(null);
// #endif

//         yield return new WaitForSeconds(2f);
//         this.readyToLogin = true;
//         this.loginObj.SetActive(true);
//         this.logoTweenAlpha.Begin();
//     }



//     //////////////////////////////////////////////////////////////////////////////
//     //private

//     [SerializeField] private STweenAlpha logoTweenAlpha;
//     [SerializeField] private GameObject loginObj;
//     [SerializeField] private GameObject appleLoginButtonObj;
//     [SerializeField] private GameObject googleLoginButtonObj;
//     [SerializeField] private GameObject guestLoginButtonObj;
//     [SerializeField] private GameObject uiObj;

//     private int getVersionTryCount = 0;

//     private bool readyToLogin = false;

//     private AudioSource audioData;

//     private Run introRun = null;

//     private void MoveToMain()
//     {
//         Run.Wait(() => { return this.readyToLogin; }, () =>
//         {
//             SingularManager.Instance.LoginEvent();
//             Transition.LoadLevel("Main", 0.2f, Color.black);
//         });
//     }

//     private void CheckVersion(string key)
//     {
//         this.getVersionTryCount++;

//         LoadingPop.HidePop();
//         NetworkManager.Instance.CheckVersion(key, (state, result) =>
//         {
//             if (state == NetworkState.NORMAL)
//             {
//                 if (GameManager.Instance.HandleVersionData(result.status, result.spec_version))
//                 {
//                     SpecDataManager.Instance.SpecVersion = result.spec_version;
//                     SpecDataManager.Instance.LangaugeVersion = result.language_version;
//                     this.CheckAndLoadLanguageData();
//                 }
//             }
//             else
//             {
//                 if (this.getVersionTryCount < 3)
//                 {
//                     Run.After(1f, () => { this.CheckVersion(key); });
//                 }
//                 else
//                 {
//                     LoadingPop.HidePop();
//                     MessagePop.ShowPop(LanguageManager.Instance.GetTryAgainText()).SetOnCompleteHide(() =>
//                     {
//                         GameObject.Destroy(DontDestroyObject.Instance.gameObject);
//                         UnityEngine.SceneManagement.SceneManager.LoadScene(0);
//                     });
//                 }
//             }
//         });
//     }

//     private void Login()
//     {
//         NetworkManager.Instance.GetConfig((state, data) =>
//         {
//             DataManager.Instance.SetConfig(data.config);
//             string socialID = Preference.LoadPreference(Pref.SOCIAL_ID, string.Empty);
//             if (!string.IsNullOrEmpty(socialID))
//             {
//                 LoadingPop.HidePop();
//                 this.SoicalLogin();
//             }
//             else
//             {
//                 this.showIntroButtonObj.SetActive(true);
//                 this.guestLoginButtonObj.SetActive(true);
// #if UNITY_IOS
//                 this.appleLoginButtonObj.SetActive(true);
// #elif UNITY_ANDROID
//                 this.googleLoginButtonObj.SetActive(true);
// #endif
//             }
//         });
//     }

//     private void GuestLogin()
//     {
//         //LOGIN TO SERVER..
//         NetworkManager.Instance.AuthLogin("guest", "guest", (state, response) =>
//         {
//             if (state == NetworkState.NORMAL)
//             {
//                 if (response.isBanned == 1)
//                 {
//                     LoadingPop.HidePop();
//                     this.ShowBanned(response.uid);
//                 }
//                 else
//                 {
//                     Preference.SavePreference(Pref.SOCIAL_ID, string.Empty);
//                     this.guestLoginButtonObj.SetActive(false);
//                     this.googleLoginButtonObj.SetActive(false);
//                     this.showIntroButtonObj.SetActive(false);
//                     this.GetLobbyInfo();
//                 }
//             }
//             else
//             {
//                 LoadingPop.HidePop();
//                 MessagePop.ShowPop(LanguageManager.Instance.GetTryAgainText()).SetOnCompleteHide(() =>
//                 {
//                     GameObject.Destroy(DontDestroyObject.Instance.gameObject);
//                     UnityEngine.SceneManagement.SceneManager.LoadScene(0);
//                 });
//             }
//         });
//     }

//     private void CheckAndLoadLanguageData()
//     {
//         int savedVersion = Preference.LoadPreference(Pref.LAN_VERSION, 0);
//         string savedLan = Preference.LoadPreference(Pref.LAN_SAVE, string.Empty);

//         if (SpecDataManager.Instance.LangaugeVersion == savedVersion && savedLan.Equals(LanguageManager.Instance.Language.ToString()))
//         {

//             string savedJson = Preference.LoadPreference(Pref.LAN_DATA, string.Empty);
//             List<LanguageMetaData> data = JsonConvert.DeserializeObject<List<LanguageMetaData>>(savedJson);
//             SpecDataManager.Instance.SetServerLanguageData(data);

//             Debug.LogColor("Use exist language data VER :" + SpecDataManager.Instance.LangaugeVersion + " LAN : " + savedLan);
            
//             this.CheckAndLoadSpecData();
//         }
//         else
//         {
//             this.LoadLanguageData();
//         }
//     }

//     private void LoadLanguageData()
//     {
//         //DOWNLOADING DATA..
//         NetworkManager.Instance.GetLanguageData(SpecDataManager.Instance.LangaugeVersion, (state, response) =>
//         {
//             if (state == NetworkState.NORMAL)
//             {
//                 List<LanguageMetaData> data = JsonConvert.DeserializeObject<List<LanguageMetaData>>(response);
//                 SpecDataManager.Instance.SetServerLanguageData(data);

//                 int savedVersion = Preference.LoadPreference(Pref.LAN_VERSION, 0);
//                 string savedLan = Preference.LoadPreference(Pref.LAN_SAVE, string.Empty);

//                 Preference.SavePreference(Pref.LAN_VERSION, SpecDataManager.Instance.LangaugeVersion);
//                 Preference.SavePreference(Pref.LAN_SAVE, LanguageManager.Instance.Language.ToString());
//                 Preference.SavePreference(Pref.LAN_DATA, response);

//                 Debug.LogColor("Sucess load language data VER : " + SpecDataManager.Instance.LangaugeVersion + " LAN : " + LanguageManager.Instance.Language.ToString());
                
//                 this.CheckAndLoadSpecData();
//             }
//             else
//             {
//                 MessagePop.ShowPop(LanguageManager.Instance.GetTryAgainText()).SetOnCompleteHide(() =>
//                 {
//                     GameObject.Destroy(DontDestroyObject.Instance.gameObject);
//                     UnityEngine.SceneManagement.SceneManager.LoadScene(0);
//                 });
//             }
//         });
//     }

//     private void CheckAndLoadSpecData()
//     {
//         int savedVersion = Preference.LoadPreference(Pref.SPEC_VERSION, 0);
//         string specJson = Preference.LoadPreference(Pref.SPEC_DATA, string.Empty);

//         if (SpecDataManager.Instance.SpecVersion == savedVersion && !string.IsNullOrEmpty(specJson))
//         {
//             SpecDataManager.Instance.SetServerSpecData(JsonConvert.DeserializeObject<ServerSpecData>(specJson));
//             this.Login();
//         }
//         else
//         {
//             this.LoadSpecData();
//         }
//     }

//     private void LoadSpecData()
//     {
//         //DOWNLOADING DATA..
//         NetworkManager.Instance.GetSpecData(SpecDataManager.Instance.SpecVersion, (state, response) =>
//         {
//             if (state == NetworkState.NORMAL)
//             {
//                 ServerSpecData data = JsonConvert.DeserializeObject<ServerSpecData>(response);
//                 SpecDataManager.Instance.SetServerSpecData(data);
//                 Preference.SavePreference(Pref.SPEC_VERSION, SpecDataManager.Instance.SpecVersion);
//                 Preference.SavePreference(Pref.SPEC_DATA, response);
//                 this.Login();
//             }
//             else
//             {
//                 MessagePop.ShowPop(LanguageManager.Instance.GetTryAgainText()).SetOnCompleteHide(() =>
//                 {
//                     GameObject.Destroy(DontDestroyObject.Instance.gameObject);
//                     UnityEngine.SceneManagement.SceneManager.LoadScene(0);
//                 });
//             }
//         });
//     }

//     private void GetLobbyInfo()
//     {
//         NetworkManager.Instance.LobbyInfo((state, response) =>
//         {
//             LoadingPop.HidePop();
//             if (state == NetworkState.NORMAL)
//             {
//                 Cookapps.Analytics.CAppEvent.ReportProgress(1);
//                 GameManager.Instance.ExistsPost = (response.existsPost == 1);
//                 DataManager.Instance.SetServerData(response);
//                 if (DataManager.Instance.UserData.isCheater)
//                 {
//                     this.ShowBanned(DataManager.Instance.UserData.uid);
//                 }
//                 else
//                 {
//                     bool show = Preference.LoadPreference(Pref.SHOW_INTRO, false);
//                     if (show)
//                         this.MoveToMain();
//                     else
//                         this.ShowIntro();
//                 }
//             }
//             else
//             {
//                 MessagePop.ShowPop(LanguageManager.Instance.GetTryAgainText()).SetOnCompleteHide(() =>
//                 {
//                     GameObject.Destroy(DontDestroyObject.Instance.gameObject);
//                     UnityEngine.SceneManagement.SceneManager.LoadScene(0);
//                 });
//             }
//         });
//     }

//     private void ShowBanned(int uid)
//     {
//         string msg = LanguageManager.Instance.GetAccountLockedText();
//         string confirm = LanguageManager.Instance.GetConfirmText();
//         string sendMail = LanguageManager.Instance.GetContactUSText();
//         MessagePop.ShowPop(msg, confirm, () =>
//         {
//             Application.Quit(0);
//         }, sendMail, () =>
//         {
//             this.SendCSMail(uid);
//         }, canClose: false);
//     }

//     private void SendCSMail(int uid)
//     {
//         string mailto = "cs_playgrounds@cookapps.com";
//         string subject = this.EscapeURL("Epic Fantasy");
//         string body = this.EscapeURL
//             (
//              "Please fill in the content here.\n\n\n\n" +
//              "________" +
//              "UID : " + uid + "\n\n" +
//              "VER : " + Application.version + "\n\n" +
//              "Device Model : " + SystemInfo.deviceModel + "\n\n" +
//              "Device OS : " + SystemInfo.operatingSystem + "\n\n" +
//              "________"
//             );

//         Application.OpenURL("mailto:" + mailto + "?subject=" + subject + "&body=" + body);
//     }

//     private string EscapeURL(string url)
//     {
//         return UnityWebRequest.EscapeURL(url).Replace("+", "%20");
//     }

//     private void SoicalLogin()
//     {
//         LoadingPop.ShowPop();
//         GameSocialManager.Instance.LoginToSocial((uid, name, platform) =>
//         {
//             if (string.IsNullOrEmpty(uid))
//             {
//                 LoadingPop.HidePop();
//                 ToastPop.ShowPop(LanguageID.MSG_FAIL_TO_LOGIN);
//             }
//             else
//             {
//                 NetworkManager.Instance.AuthLogin(uid, platform, (state, response) =>
//                 {
//                     if (state == NetworkState.NORMAL)
//                     {
//                         if (response.isBanned == 1)
//                         {
//                             LoadingPop.HidePop();
//                             this.ShowBanned(response.uid);
//                         }
//                         else
//                         {
//                             Preference.SavePreference(Pref.GUEST_ID, string.Empty);
//                             Preference.SavePreference(Pref.SOCIAL_ID, uid);
//                             this.guestLoginButtonObj.SetActive(false);
//                             this.googleLoginButtonObj.SetActive(false);
//                             this.showIntroButtonObj.SetActive(false);
//                             this.appleLoginButtonObj.SetActive(false);
//                             this.GetLobbyInfo();
//                         }
//                     }
//                     else
//                     {
//                         LoadingPop.HidePop();
//                         MessagePop.ShowPop(LanguageManager.Instance.GetTryAgainText()).SetOnCompleteHide(() =>
//                         {
//                             GameObject.Destroy(DontDestroyObject.Instance.gameObject);
//                             UnityEngine.SceneManagement.SceneManager.LoadScene(0);
//                         });
//                     }
//                 });
//             }
//         });
//     }

//     private void ShowIntro()
//     {
//         Preference.SavePreference(Pref.SHOW_INTRO, true);
//         string url = "https://d19otl8pcfrvqp.cloudfront.net/images/afk-dungeon/intro.wav";
//         LoadingPop.ShowPop();
//         Run.Coroutine(NetworkManager.Instance.GetAudioClip(url, (clip) =>
//         {
//             LoadingPop.HidePop();
//             this.playableDirector.Play();
//             if (clip != null)
//             {
//                 GameObject gameObject = new GameObject();
//                 this.audioData = gameObject.AddComponent<AudioSource>();
//                 this.audioData.clip = clip;
//                 this.audioData.volume = Preference.LoadPreference(Pref.BGM_V, 1f);
//                 this.audioData.Play();
//                 SoundManager.Instance.PauseBGM();
//             }

//             this.uiObj.SetActive(false);
//             Run.After(2f, () => { this.skipUIObj.SetActive(true); });

//             Debug.LogColor("DURATION " + this.playableDirector.duration);

//             this.introRun = Run.After((float)this.playableDirector.duration, () => { this.FinishIntro(); });

//         }));
//     }

//     private void FinishIntro()
//     {
//         if (this.introRun != null)
//         {
//             this.introRun.Abort();
//             this.introRun = null;
//         }

//         if (DataManager.Instance.IsDataLoaded)
//         {
//             this.MoveToMain();
//         }
//         else
//         {
//             this.uiObj.SetActive(true);
//             this.skipUIObj.SetActive(false);
//             this.audioData.Stop();
//             SoundManager.Instance.UnPauseBGM();
//         }
//     }
}
