using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Playables;

public class SplashUI : MonoBehaviour
{
    //////////////////////////////////////////////////////////////////////////////
    //public
    public void OnClickGuestLogin()
    {
        SoundManager.Instance.PlayButtonClick();
        MoveToMain();
    }

    //////////////////////////////////////////////////////////////////////////////
    //protected

    protected void Awake()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Application.targetFrameRate = 60;
    }

    protected IEnumerator Start()
    {
        SoundManager.Instance.SetBGMVolume(Preference.LoadPreference(Pref.BGM_V, 0.8f));
        SoundManager.Instance.SetSFXVolume(Preference.LoadPreference(Pref.SFX_V, 1f));

#if USE_SRDEBUG
        if (!Application.isEditor) SRDebug.Init();
#endif

        yield return new WaitForSeconds(2f);
        this.readyToLogin = true;
        this.loginObj.SetActive(true);
    }



    //////////////////////////////////////////////////////////////////////////////
    //private
    [SerializeField] private GameObject loginObj;

    private bool readyToLogin = false;

    private void MoveToMain()
    {
        Run.Wait(() => { return this.readyToLogin; }, () =>
        {
            Transition.LoadLevel("Main", 0.2f, Color.black);
        });
    }
}
