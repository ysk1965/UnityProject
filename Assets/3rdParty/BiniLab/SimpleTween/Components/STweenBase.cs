/*********************************************
 * CHOI YOONBIN - Simple Tween
 *********************************************/

using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class STween : MonoBehaviour
{

    public virtual void Begin() { }
    public virtual void Restore() { }
}

public class STweenBase<T> : STween
{

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // public 

    public float Delay
    {
        get { return this.delay; }
        set { this.delay = value; }
    }

    public float Duration
    {
        get { return this.duration; }
        set { this.duration = value; }
    }

    public T End
    {
        get { return this.end; }
        set { this.end = value; }
    }

    public bool InActiveOnComplete { get => this.inActiveOnComplete; set => this.inActiveOnComplete = value; }

    public override void Begin()
    {
        this.PlayTween();
        if (this.beginWith != null)
            foreach (STween tween in this.beginWith)
                tween.Begin();
    }

    public override void Restore()
    {
        if (this.beginWith != null)
            foreach (STween tween in this.beginWith)
                tween.Restore();
    }

    public void Begin(T start, T end, SimpleTweenerEx.EasingType easingType = SimpleTweenerEx.EasingType.LinearEasing)
    {
        this.start = start;
        this.end = end;
        this.easingType = easingType;
        this.PlayTween();
        if (this.beginWith != null)
            foreach (STween tween in this.beginWith)
                tween.Begin();
    }

    public void AddOnComplete(UnityAction call)
    {
        if (this.onCompleteEvent == null)
            this.onCompleteEvent = new UnityEvent();
        this.onCompleteEvent.AddListener(call);
    }

    public void SetOnComplete(UnityAction call)
    {
        this.onCompleteEvent = new UnityEvent();
        this.onCompleteEvent.AddListener(call);
    }

    public void Stop() { this.tweener.ForceComplete(); }
    public void Pause() { this.pause = true; }
    public void Resume() { this.pause = false; }

    public bool IsPlaying() { return !this.tweener.Completed; }
    public bool isPause() { return this.pause; }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // protected

    [SerializeField] protected SimpleTweener.TweenLoopType loopType;
    [SerializeField] protected int loopCount;

    [SerializeField] protected float duration;
    [SerializeField] protected float delay;

    [SerializeField] protected T start;
    [SerializeField] protected T end;

    [SerializeField] protected SimpleTweenerEx.EasingType easingType = SimpleTweenerEx.EasingType.LinearEasing;

    [SerializeField] protected bool restoreBeforeStart = true;
    [SerializeField] protected bool unscaledTime = true;
    [SerializeField] protected bool playOnStart = false;
    [SerializeField] protected bool inActiveOnComplete = false;
    [SerializeField] protected bool useCurve = false;

    [SerializeField] protected AnimationCurve animCurve;
    [SerializeField] protected UnityEvent onCompleteEvent;

    [SerializeField] protected STween[] beginWith;

    protected SimpleTweenerEx tweener = new SimpleTweenerEx();
    protected TweenLerp<T> tweenValue;
    protected bool pause = false;

    protected virtual void UpdateValue(T value) { }

    protected virtual void PlayTween()
    {
        if (this.restoreBeforeStart)
            this.Restore();

        this.pause = false;

        if (this.useCurve)
            this.tweener.Reset(this.duration, this.animCurve, this.OnCompleteTween);
        else
            this.tweener.Reset(this.duration, this.easingType, this.OnCompleteTween);

        this.tweener.LoopType(this.loopType, this.loopCount);
        this.tweener.Delay(this.delay);
    }

    protected virtual void OnCompleteTween(object[] onCompleteParams)
    {
        if (this.onCompleteEvent != null)
            this.onCompleteEvent.Invoke();

        if (this.inActiveOnComplete)
            this.gameObject.SetActive(false);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // protected overrides MonoBehaviour 

    // Use this for initialization
    protected virtual void Start() { }

    // Update is called once per frame
    protected virtual void Update()
    {

        if (this.pause) return;
        if (!this.tweener.Completed)
        {
            this.tweener.Update(this.unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime);
            this.UpdateValue(this.tweenValue.Value);
        }
    }

    void OnEnable()
    {
        if (this.playOnStart)
            this.PlayTween();
    }
}