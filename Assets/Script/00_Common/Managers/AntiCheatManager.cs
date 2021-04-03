#if (UNITY_WINRT || UNITY_WINRT_10_0 || UNITY_WSA || UNITY_WSA_10_0) && !ENABLE_IL2CPP
#define ACTK_UWP_NO_IL2CPP
#endif
using UnityEngine;
using CodeStage.AntiCheat.Genuine.CodeHash;

#if UNITY_2018_1_OR_NEWER && !ACTK_UWP_NO_IL2CPP
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using CodeStage.AntiCheat.Utils;
using CodeStage.AntiCheat.ObscuredTypes;
#endif

// use this to check hash generated with CodeHashGeneratorListener.cs example file
// note: this is an example for the Windows Standalone platform only
public class AntiCheatManager : GameObjectSingleton<AntiCheatManager>
{
    public static readonly char[] HashKey = { '\x674', '\x345', '\x856', '\x968', '\x322' };
    public const string Base64 = "ADSKJFLKA33240SLKDFJA39F0303";
    public const string SHA1 = "DONTFUCKHACKMYAPPGOTOHELL";
    public const string Separator = "FU";

    public bool IsReady => this.isReady;

    public void Request(Action<string> callback)
    {
        this.callback = callback;
#if UNITY_EDITOR
        this.callback(null);
#elif UNITY_2018_1_OR_NEWER && !ACTK_UWP_NO_IL2CPP
        CodeHashGenerator.Generate();
#endif
    }

    protected override void Awake()
    {
        base.Awake();
#if UNITY_2018_1_OR_NEWER && !ACTK_UWP_NO_IL2CPP
        CodeHashGenerator.HashGenerated += OnGotHash;
#endif
        this.isReady = true;
    }

    private bool isReady = false;
    private Action<string> callback;

#if UNITY_2018_1_OR_NEWER && !ACTK_UWP_NO_IL2CPP
    private void OnGotHash(HashGeneratorResult result)
    {
        if (!result.Success)
        {
            this.callback(null);
        }
        else
        {
            this.callback(result.SummaryHash);
        }
    }
#endif
}

