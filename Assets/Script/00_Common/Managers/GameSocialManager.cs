// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Events;
// using UnityEngine.SocialPlatforms;
// using GooglePlayGames;
// using GooglePlayGames.BasicApi;
// using UnityEngine.SignInWithApple;

// public class GameSocialManager : GameObjectSingleton<GameSocialManager>
// {
//     /////////////////////////////////////////////////////////////
//     // public

//     public string editorId = "a10001";

//     public void LoginToSocial(UnityAction<string, string, string> onComplete = null)
//     {
//         this.timeOut = Run.After(TIME_OUT_SEC, () =>
//         {
//             onComplete(null, null, null);
//         });

//         if (Application.isEditor)
//         {
//             if (onComplete != null) onComplete(this.editorId, string.Empty, "google");
//             this.timeOut.Abort();
//         }
//         else
//         {
// #if UNITY_IOS
//             if(this.siwa == null) this.siwa = gameObject.GetComponent<SignInWithApple>();
//             this.siwa.Login((SignInWithApple.CallbackArgs args) => 
//             {
//                 if(string.IsNullOrEmpty(args.error))
//                 {
//                     Debug.LogWarning("LoginToSocial authenticated");
//                     onComplete(args.userInfo.userId, args.userInfo.displayName, "apple");
//                     this.timeOut.Abort();
//                 }
//                 else
//                 {
//                     onComplete(null, null, null);
//                     this.timeOut.Abort();
//                 }
//             });
// #elif UNITY_ANDROID
//             if (Social.localUser.authenticated)
//             {
//                 Debug.Log("LoginToSocial authenticated");
//                 if (onComplete != null) onComplete(Social.localUser.id, Social.localUser.userName, "google");
//                 this.timeOut.Abort();
//             }
//             else
//             {
//                 Social.localUser.Authenticate((bool success) =>
//                 {
//                     Debug.Log("LoginToSocial Authenticate " + success);
//                     if (success)
//                     {
//                         if (onComplete != null) onComplete(Social.localUser.id, Social.localUser.userName, "google");
//                         this.timeOut.Abort();
//                     }
//                     else
//                     {
//                         if (onComplete != null) onComplete(null, null, null);
//                         this.timeOut.Abort();
//                     }
//                 });
//             }
// #endif
//         }
//     }

//     public void LogOut()
//     {
// #if UNITY_ANDROID
//         PlayGamesPlatform.Instance.SignOut();
// #endif
//     }

//     /////////////////////////////////////////////////////////////
//     // protected

//     protected void Start()
//     {
// #if UNITY_IOS

//         this.siwa = gameObject.GetComponent<SignInWithApple>();

// #elif UNITY_ANDROID

//         PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
//         PlayGamesPlatform.InitializeInstance(config);
//         PlayGamesPlatform.DebugLogEnabled = false;
//         PlayGamesPlatform.Activate();

// #endif
//     }

//     /////////////////////////////////////////////////////////////
//     // private
//     private readonly float TIME_OUT_SEC = 15f;
//     private SignInWithApple siwa;
//     private Run timeOut;
// }
