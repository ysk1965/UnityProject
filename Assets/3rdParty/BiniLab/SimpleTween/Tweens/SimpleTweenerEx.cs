using UnityEngine;
using System.Collections;

public class SimpleTweenerEx : SimpleTweener
{
    public enum EasingType
    {
        LinearEasing,
        LinearEasingInOut,
        QuadEasingIn,
        QuadEasingOut,
        QuadEasingInOut,
        CircEasingIn,
        CircEasingOut,
        CircEasingInOut,
        ExpoEasingIn,
        ExpoEasingOut,
        ExpoEasingInOut,
        ElasticEasginIn,
        ElasticEasginOut,
        ElasticEasginInOut,
        BackEasginIn,
        BackEasginOut,
        BackEasginInOut,
        BounceEasingIn,
        BounceEasingOut,
        BounceEasingInOut
    }

    public delegate void EasingComplete(object[] onCompleteParms);

    public SimpleTweenerEx() :
        base()
    {
        this.onComplete = null;
    }

    public override bool Completed
    {
        get
        {
            if ((this.duration > 0.0f) && base.Completed)
            {
                if (this.onComplete != null)
                    this.onComplete(this.onCompleteParms);
            }

            return base.Completed;
        }
    }

    public void Reset(float d, EasingType easingType, EasingComplete comp = null, params object[] onCompleteParms)
    {
        EasingObject.EasingPosition fn = null;

        switch (easingType)
        {
            case EasingType.BackEasginIn: fn = EasingObject.BackEasingIn; break;
            case EasingType.BackEasginOut: fn = EasingObject.BackEasingOut; break;
            case EasingType.BackEasginInOut: fn = EasingObject.BackEasingInOut; break;
            case EasingType.BounceEasingIn: fn = EasingObject.BounceEasingIn; break;
            case EasingType.BounceEasingOut: fn = EasingObject.BounceEasingOut; break;
            case EasingType.BounceEasingInOut: fn = EasingObject.BounceEasingInOut; break;
            case EasingType.CircEasingIn: fn = EasingObject.CircEasingIn; break;
            case EasingType.CircEasingOut: fn = EasingObject.CircEasingOut; break;
            case EasingType.CircEasingInOut: fn = EasingObject.CircEasingInOut; break;
            case EasingType.ElasticEasginIn: fn = EasingObject.ElasticEasingIn; break;
            case EasingType.ElasticEasginOut: fn = EasingObject.ElasticEasingOut; break;
            case EasingType.ElasticEasginInOut: fn = EasingObject.ElasticEasingInOut; break;
            case EasingType.ExpoEasingIn: fn = EasingObject.ExpoEasingIn; break;
            case EasingType.ExpoEasingOut: fn = EasingObject.ExpoEasingOut; break;
            case EasingType.ExpoEasingInOut: fn = EasingObject.ExpoEasingInOut; break;
            case EasingType.LinearEasing: fn = EasingObject.LinearEasing; break;
            case EasingType.LinearEasingInOut: fn = EasingObject.LinearEasingInOut; break;
            case EasingType.QuadEasingIn: fn = EasingObject.QuadEasingIn; break;
            case EasingType.QuadEasingOut: fn = EasingObject.QuadEasingOut; break;
            case EasingType.QuadEasingInOut: fn = EasingObject.QuadEasingInOut; break;
        }
        this.Reset(d, fn, comp, onCompleteParms);
    }

    public void Reset(float d, AnimationCurve animCurve, EasingComplete comp = null, params object[] onCompleteParms)
    {
        base.Reset(d, this.AnimCurveEasing);

        this.animCurve = animCurve;
        this.onComplete = comp;
        this.onCompleteParms = onCompleteParms;
    }

    public void Reset(float d, EasingObject.EasingPosition fn, float overshoot_amplitude, float period, EasingComplete comp = null, params object[] onCompleteParms)
    {
        base.Reset(d, fn, overshoot_amplitude, period);

        this.animCurve = null;
        this.onComplete = comp;
        this.onCompleteParms = onCompleteParms;
    }

    public void Reset(float d, EasingObject.EasingPosition fn = null, EasingComplete comp = null, params object[] onCompleteParms)
    {
        float[] val;

        if ((fn != null) && EasingObject.DEF_PARAM.TryGetValue(fn, out val))
            this.Reset(d, fn, val[0], val[1], comp, onCompleteParms);
        else
            this.Reset(d, fn, 0.0f, 0.0f, comp, onCompleteParms);
    }

    public void Shake(float d, float amplitude, float period, EasingComplete comp = null, params object[] onCompleteParms)
    {
        base.Shake(d, amplitude, period);

        this.animCurve = null;
        this.onComplete = comp;
        this.onCompleteParms = onCompleteParms;
    }

    public override void ForceComplete()
    {
        base.ForceComplete();

        if (this.onComplete != null)
            this.onComplete(this.onCompleteParms);
    }

    /////////////////////////////////////////////////////////////////
    // private
    private EasingComplete onComplete;
    private object[] onCompleteParms;

    private AnimationCurve animCurve;

    private float AnimCurveEasing(float s, float e, float deltaTime, float duration, float overshoot_amplitude, float period)
    {
        return this.animCurve.Evaluate(deltaTime / this.duration);
    }

    private float GetAnimCurveEvaluated(float t)
    {
        return this.animCurve.Evaluate(t);
    }
}
