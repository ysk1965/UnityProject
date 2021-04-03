using System.Collections;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;

public class StringConst
{
#if UNITY_IOS
    public static ObscuredString MARKET_URL = "https://apps.apple.com/app/id1523797142";
#else

    public static ObscuredString MARKET_URL = "market://details?id=com.cookapps.epicfantacy";
#endif
    public static ObscuredString PACKAGE = "com.cookapps.epicfantacy";
    public static string UI_POPUP_PATH = "Prefabs/UI/Popups/";
}
