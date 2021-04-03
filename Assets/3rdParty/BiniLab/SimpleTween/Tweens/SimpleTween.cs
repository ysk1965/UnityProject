using UnityEngine;
using System.Collections;

/*
 SimpleTweener와 TweenLerp 한 개를 묶어서 관리
*/

public class SimpleTween<TYPE> : SimpleTweener
{
    public SimpleTween(TweenLerp<TYPE>.Lerp lerpfn)
    {
        this.lerpfn = lerpfn;
        this.onUpdate = null;
    }

    public void Tween(float delay, float duration, EasingObject.EasingPosition easingfn, TYPE start, TYPE end, System.Action<TYPE> onUpdate = null)
    {
        base.Reset(duration, easingfn);
        base.Delay(delay);
        this.tweenLerp = base.CreateTween(start, end, this.lerpfn);
        this.onUpdate = onUpdate;
    }

    public void Tween(float delay, float duration, EasingObject.EasingPosition easingfn, float overshoot_amplitude, float period, TYPE start, TYPE end, System.Action<TYPE> onUpdate = null)
    {
        base.Reset(duration, easingfn, overshoot_amplitude, period);
        base.Delay(delay);
        this.tweenLerp = base.CreateTween(start, end, this.lerpfn);
        this.onUpdate = onUpdate;
    }

    public void Shake(float delay, float duration, TYPE start, TYPE end, System.Action<TYPE> onUpdate = null)
    {
        base.Shake(duration);
        base.Delay(delay);
        this.tweenLerp = base.CreateTween(start, end, this.lerpfn);
        this.onUpdate = onUpdate;
    }

    public TYPE Start
    {
        get
        {
            return this.tweenLerp.Start;
        }
    }

    public TYPE End
    {
        get
        {
            return this.tweenLerp.End;
        }
    }

    public TYPE Value
    {
        get
        {
            return this.tweenLerp.Value;
        }
    }

    public override void Update(float deltaTime)
    {
        base.Update(deltaTime);

        if (this.onUpdate != null)
            this.onUpdate(this.Value);
    }

    /////////////////////////////////////////////////////////////////
    // private
    private TweenLerp<TYPE>.Lerp lerpfn;
    private TweenLerp<TYPE> tweenLerp;
    private System.Action<TYPE> onUpdate;
}
