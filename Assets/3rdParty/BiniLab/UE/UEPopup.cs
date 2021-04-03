/*********************************************
 * NHN StarFish - UI Extends
 * CHOI YOONBIN
 * 
 *********************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UEPopup : MonoBehaviour
{

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // public static

    public static T GetInstantiateComponent<T>() where T : MonoBehaviour
    {
        GameObject go = GameObject.Instantiate(Resources.Load(StringConst.UI_POPUP_PATH + typeof(T).Name) as GameObject) as GameObject;
        T instance = go.GetComponent<T>();
        UEPopup.instance = instance as UEPopup;
        return instance;
    }


    public static T GetInstantiateComponent<T>(string path) where T : MonoBehaviour
    {
        GameObject go = GameObject.Instantiate(Resources.Load(path) as GameObject) as GameObject;
        T instance = go.GetComponent<T>();
        UEPopup.instance = instance as UEPopup;
        return instance;
    }

    public static T GetInstantiateComponent<T>(GameObject prefab) where T : MonoBehaviour
    {
        GameObject go = GameObject.Instantiate(prefab) as GameObject;
        T instance = go.GetComponent<T>();
        UEPopup.instance = instance as UEPopup;
        return instance;
    }

    public static void HideAll()
    {
        foreach (UEPopup pop in FindObjectsOfType<UEPopup>())
        {
            pop.Hide();
        }
    }

    public static void HideAllForReset()
    {
        foreach (UEPopup pop in FindObjectsOfType<UEPopup>())
        {
            Destroy(pop.gameObject);
        }
    }

    public static bool HasPopup { get { return popupCount > 0; } }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // protected

    protected static UEPopup instance;

    protected virtual void Start() { }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!this.bodyScaleTweener.Completed)
        {
            this.bodyScaleTweener.Update(Time.unscaledDeltaTime);
            if (this.popBody) this.popBody.localScale = this.bodyScaleTweenValue.Value;
        }

        if (!this.alphaTweener.Completed)
        {
            this.alphaTweener.Update(Time.unscaledDeltaTime);
            if (this.bodyCanvasGroup) this.bodyCanvasGroup.alpha = this.alphaTweenValue.Value;
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                if (this.canBackgroundClose && this.completeShow && this.showing)
                    this.Hide();
            }
        }
    }

    protected virtual void OnEnable()
    {
        popupCount++;
    }

    protected virtual void OnDisable()
    {
        popupCount--;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // public

    private static int popupCount = 0;

    public delegate void DelOnCompleteHide();

    public virtual void Show(float tweenDuration = DEFAULT_TWEEN_DURATION, bool usingScaleAnim = true)
    {
        this.Initialized(usingScaleAnim);
        if (this.darkBackground != null) this.darkBackground.Show(tweenDuration);

        if (usingScaleAnim)
        {
            this.bodyScaleTweener.Reset(tweenDuration, EasingObject.BackEasingInOut);
            this.bodyScaleTweenValue = this.bodyScaleTweener.CreateTween(this.bodyScaleBeginValue, this.bodyScaleEndValue);
        }

        this.alphaTweener.Reset(tweenDuration, EasingObject.LinearEasing, this.OnCompleteShow);
        this.alphaTweenValue = this.alphaTweener.CreateTween(0f, 1f);

        this.showing = true;
    }

    public virtual void Hide(float tweenDuration = DEFAULT_TWEEN_DURATION, bool usingScaleAnim = true)
    {
        if (!this.showing)
            return;

        if (this.darkBackground != null) this.darkBackground.Hide(tweenDuration);

        if (usingScaleAnim)
        {
            this.bodyScaleTweener.Reset(tweenDuration / 2f, EasingObject.BackEasingIn);
            this.bodyScaleTweenValue = this.bodyScaleTweener.CreateTween(this.bodyScaleEndValue, this.bodyScaleBeginValue);
        }

        this.alphaTweener.Reset(tweenDuration / 2f, EasingObject.LinearEasing, this.OnCompleteHide);
        this.alphaTweenValue = this.alphaTweener.CreateTween(1f, 0f);

        this.showing = false;
        this.initialized = false;
    }

    public void SetOnCompleteHide(DelOnCompleteHide onCompleteHide)
    {
        this.onCompleteHide = onCompleteHide;
    }

    public bool Showing
    {
        get { return this.showing; }
    }

    public bool CompleteShow
    {
        get { return this.completeShow; }
    }

    public bool CompleteHide
    {
        get { return this.completeHide; }
    }

    public float GetCameraDepthMax()
    {
        float depth = -100f;
        foreach (var camera in this.cameras)
        {
            depth = Mathf.Max(depth, camera.depth);
        }
        return depth;
    }

    public void SetCameraDepth(float startDepth)
    {
        if (this.cameras == null)
            return;

        for (int i = 0; i < this.cameras.Length; i++)
        {
            this.cameras[i].depth = startDepth + (float)i;
        }
    }

    public virtual bool Closeable
    {
        get { return false; }
    }

    public virtual void OnClickClose()
    {
        SoundManager.Instance.PlayCancel();
        this.Hide();
    }

    public virtual void OnClickDarkBackground()
    {
        if (this.canBackgroundClose && this.completeShow && this.showing)
        {
            SoundManager.Instance.PlayCancel();
            this.Hide();
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // protected
    protected const float DEFAULT_TWEEN_DURATION = 0.2f;

    //UI Components
    [SerializeField] protected UEPopupBackground darkBackground;
    [SerializeField] protected RectTransform popBody;
    [SerializeField] protected Camera[] cameras;
    [SerializeField] protected bool canBackgroundClose = true;

    protected SimpleTweenerEx bodyScaleTweener = new SimpleTweenerEx();
    protected TweenLerp<Vector3> bodyScaleTweenValue;
    protected Vector3 bodyScaleBeginValue = Vector3.zero;
    protected Vector3 bodyScaleEndValue = Vector3.one;

    protected SimpleTweenerEx alphaTweener = new SimpleTweenerEx();
    protected TweenLerp<float> alphaTweenValue;

    protected DelOnCompleteHide onCompleteHide;

    protected bool showing = false;
    protected bool completeShow = false;
    protected bool completeHide = false;
    protected bool initialized = false;
    //out of spac
    //protected bool closeable = false;

    protected virtual void OnCompleteShow(object[] onCompleteParms = null)
    {
        this.completeShow = true;
    }

    protected virtual void OnCompleteHide(object[] onCompleteParms = null)
    {
        this.completeHide = true;
        this.gameObject.SetActive(false);
        if (this.onCompleteHide != null)
            this.onCompleteHide();
        Destroy(this.gameObject);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // private

    private CanvasGroup bodyCanvasGroup;

    private void Initialized(bool usingScaleAnim)
    {
        if (this.initialized)
            return;

        this.gameObject.SetActive(true);
        if (this.popBody)
        {
            this.bodyCanvasGroup = this.popBody.GetComponent<CanvasGroup>();
            this.bodyCanvasGroup.alpha = 0f;

            if (usingScaleAnim)
                this.popBody.transform.localScale = Vector3.zero;
        }

        this.initialized = true;
    }
}
