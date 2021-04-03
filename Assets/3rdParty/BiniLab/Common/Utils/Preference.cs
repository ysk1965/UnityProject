using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Pref
{
    LANGUAGE,
    BGM_V,
    SFX_V,
    SAFE_MODE_T,
    AUTO_SKILL,
    FOV,
    SPEC_VERSION,
    SPEC_DATA,
    LAN_VERSION,
    LAN_SAVE,
    LAN_DATA,
    GUEST_ID,
    SOCIAL_ID,
    DATA_TIME,
    EXIST_POST,
    HERO_VIEW,
    SHOW_INTRO,
    AUTO_BOSS,
    CAM_NUM,
    VIBRATE_DUNGEON,
    VIBRATE_CHALLENGE,
    VIBRATE_PVP,
}

public class Preference
{
    public static bool LoadPreference(Pref pref, bool defaultValue)
    {
        return LoadPreference(pref, defaultValue ? 1 : 0) > 0;
    }

    public static void SavePreference(Pref pref, bool value)
    {
        SavePreference(pref, value ? 1 : 0);
    }

    public static int LoadPreference(Pref pref, int defaultValue)
    {
        return UnityEngine.PlayerPrefs.GetInt(pref.ToString(), defaultValue);
    }

    public static void SavePreference(Pref pref, int value)
    {
        UnityEngine.PlayerPrefs.SetInt(pref.ToString(), value);
        UnityEngine.PlayerPrefs.Save();
    }

    public static string LoadPreference(Pref pref, string defaultValue)
    {
        string returnStr = UnityEngine.PlayerPrefs.GetString(pref.ToString(), defaultValue);
        Debug.Log("Preference Loaded " + returnStr);
        return returnStr;
    }

    public static void SavePreference(Pref pref, string value)
    {
        UnityEngine.PlayerPrefs.SetString(pref.ToString(), value);
        UnityEngine.PlayerPrefs.Save();
        Debug.Log("Preference Saved " + value);
    }

    public static float LoadPreference(Pref pref, float defaultValue)
    {
        return UnityEngine.PlayerPrefs.GetFloat(pref.ToString(), defaultValue);
    }

    public static void SavePreference(Pref pref, float value)
    {
        UnityEngine.PlayerPrefs.SetFloat(pref.ToString(), value);
        UnityEngine.PlayerPrefs.Save();
    }

    public static void Clear()
    {
        UnityEngine.PlayerPrefs.DeleteAll();
        UnityEngine.PlayerPrefs.Save();
    }
}