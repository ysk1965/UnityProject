using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundBGM
{
    NONE = -1,
    bgm_01,
    bgm_boss,
    bgm_dungeon,
    bgm_title,
    bgm_pvp_end,
    bgm_pvp,
    bgm_challenge,
}

public enum SoundFX
{
    //Common 0~ 1000
    NONE = -1,

    sfx_click = 1,
    sfx_cancel,
    sfx_popup,
    sfx_equip,
    sfx_upgrade,
    sfx_skill_equip,
    sfx_tap,
    sfx_reward,
    sfx_item_upgrade_normal,
    sfx_item_upgrade_advanced,
    sfx_get_jewel,
    sfx_skill_levelup,
    sfx_box_open,
    sfx_box_open_legend,

    sfx_attack = 101,
    sfx_swing,
    sfx_get_gold,
    sfx_skill_use,
    sfx_died,
    sfx_levelup,
    sfx_hero_appear,
    sfx_item_fly,
    sfx_item_get,
    sfx_dungeon_enter,
    sfx_attack_critical,
    sfx_stun,

    sfx_monster_boss_appear = 201,
    sfx_monster_boss_atack,
    sfx_monster_boss_dead,
    sfx_monster_boss_dragon_breath,
    sfx_monster_boss_dragon_bomb,
    sfx_monster_boss_skull_slash,
    sfx_monster_boss_skill,

    sfx_Skill_10001 = 10001,
    sfx_Skill_10002,
    sfx_Skill_10003,
    sfx_Skill_10004,
    sfx_Skill_10101 = 10101,
    sfx_Skill_10102,
    sfx_Skill_10103,
    sfx_Skill_10104,
    sfx_Skill_10201 = 10201,
    sfx_Skill_10202,
    sfx_Skill_10203,
    sfx_Skill_10204,
    sfx_Skill_10301 = 10301,
    sfx_Skill_10302,
    sfx_Skill_10303,
    sfx_Skill_10304,
}


public class SoundManager : GameObjectSingleton<SoundManager>
{

    /////////////////////////////////////////////////////////////
    // public

    public ClockStone.AudioObject PlayBGM(SoundBGM bgm)
    {
        return this.PlayBGM(bgm.ToString());
    }

    public ClockStone.AudioObject PlaySFX(SoundFX sfx, bool forceInSilence = false)
    {
        if (forceInSilence)
            return this.PlaySFXWithoutSilence(sfx.ToString());
        else
            return this.PlaySFX(sfx.ToString());
    }

    public bool StopSFX(SoundFX sfx)
    {
        return this.StopSFX(sfx.ToString());
    }

    public bool StopBGM()
    {
        return AudioController.StopMusic();
    }

    public bool StopBGM(float fadeOut)
    {
        return AudioController.StopMusic(fadeOut);
    }

    public void Silence(bool isSilence)
    {
        this.isSilence = isSilence;
    }

    public void PauseSFX()
    {
        this.onSFX = false;
    }
    public void UnPauseSFX()
    {
        this.onSFX = Preference.LoadPreference(Pref.SFX_V, true);
    }

    public void PauseBGM()
    {
        if (Preference.LoadPreference(Pref.BGM_V, 0.8f) > 0f)
            AudioController.SetCategoryVolume("BGM", 0.01f);
    }

    public void UnPauseBGM()
    {
        AudioController.SetCategoryVolume("BGM", Preference.LoadPreference(Pref.BGM_V, 0.8f));
    }

    public void SetBGMVolume(float v)
    {
        AudioController.SetCategoryVolume("BGM", v);
    }

    public void SetSFXVolume(float v)
    {
        AudioController.SetCategoryVolume("SFX", v);
    }

    protected override void Awake()
    {
        base.Awake();
        this.UpdateOption();
    }

    /////////////////////////////////////////////////////////////
    // Common Use

    public void PlayButtonClick()
    {
        this.PlaySFX(SoundFX.sfx_click);
    }

    public void PlayCancel()
    {
        // this.PlaySFX(SoundFX.rd_sfx_cancel);
    }

    public void UpdateOption()
    {
        this.onBGM = Preference.LoadPreference(Pref.BGM_V, true);
        this.onSFX = Preference.LoadPreference(Pref.SFX_V, true);
    }

    /////////////////////////////////////////////////////////////
    // private

    private bool isSilence = false;

    private bool onBGM = true;
    private bool onSFX = true;

    private ClockStone.AudioObject PlayBGM(string audioID)
    {
        if (!this.onBGM)
            return null;

        ClockStone.AudioObject currentAudioObj = AudioController.GetCurrentMusic();
        if (currentAudioObj != null && currentAudioObj.audioID.Equals(audioID))
            return null;

        AudioController.StopMusic();

        return AudioController.PlayMusic(audioID);
    }

    private ClockStone.AudioObject PlaySFX(string audioID)
    {
        if (!this.onSFX)
            return null;

        if (this.isSilence)
            return AudioController.Play(audioID, 0.2f);
        else
            return AudioController.Play(audioID);
    }

    private ClockStone.AudioObject PlaySFXWithoutSilence(string audioID)
    {
        if (!this.onSFX)
            return null;

        Debug.Log("PlaySFXWithoutSilence " + audioID);
        return AudioController.Play(audioID);
    }

    private bool StopSFX(string audioID)
    {
        return AudioController.Stop(audioID);
    }
}