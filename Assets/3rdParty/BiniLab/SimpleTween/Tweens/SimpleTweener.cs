using UnityEngine;
using System.Collections;

public class SimpleTweener
{
    public enum TweenLoopType
    {
        Default,
        Yoyo   // Tweens to the end values then back to the original ones and so on (X to Y, Y to X, repeat).
    }

    public float duration { get; private set; }
    public float elapsed { get; private set; }

    public SimpleTweener()
    {
        this.duration = 0.0f;

        this.LastLerp = 0.0f;
        this.elapsed = 0.0f;
        this.delay = 0.0f;

        this.Reverse = false;
        this.SwapStartEnd = false;
        this.tweenCount = 1;
        this.loopType = TweenLoopType.Default;

        this.easing = null;
    }

    public void Reset(float d, EasingObject.EasingPosition easingfn = null, float overshoot_amplitude = 0.0f, float period = 0.0f)
    {
        this.duration = d;

        this.LastLerp = 0.0f;
        this.elapsed = 0.0f;
        this.delay = 0.0f;

        this.Reverse = false;
        this.SwapStartEnd = false;
        this.tweenCount = 1;
        this.loopType = TweenLoopType.Default;

        this.easing = (easingfn == null) ? EasingObject.LinearEasing : easingfn;
        this.easing_overshoot_amplitude = overshoot_amplitude;
        this.easing_period = period;
    }

    public void Shake(float d, float amplitude = 0.1f, float period = 0.12f)
    {
        this.Reset(d, EasingObject.ElasticEasingOut, amplitude, period);
        this.SwapStartEnd = true;
    }

    public void LoopType(TweenLoopType type, int count = 0)
    {
        this.loopType = type;
        if (count <= 0)
        {
//            if (this.loopType == TweenLoopType.Yoyo)
                count = int.MaxValue;
//            else
//                count = 1;
        }

        this.tweenCount = count;
    }

    public void Delay(float delay)
    {
        this.delay = delay;
    }

    public bool Reverse { get; set; }
    public bool SwapStartEnd { get; set; }

    public float Normalized
    {
        get
        {
            return (this.duration == 0.0f) ? 1.0f : this.elapsed / this.duration;
        }
    }

    public virtual bool Completed
    {
        get
        {
            if (this.duration > 0.0f)
            {
                if (this.elapsed > this.duration)
                    this.Kill();
                else
                    return false;
            }

            return this.duration == 0.0f;
        }
    }

	public bool Playing
	{
		get
		{
			return this.duration > 0.0f;
		}
	}

    public float LastLerp { get; private set; }

    public virtual void Update(float deltaTime)
    {
        if (this.Completed)
            return;

        if (this.delay > 0.0f)
        {
            if (this.delay > deltaTime)
            {
                this.delay -= deltaTime;
                return;
            }

            deltaTime -= this.delay;
            this.delay = 0.0f;
        }

        this.elapsed += deltaTime;
        if (this.elapsed > this.duration)
            this.elapsed = this.duration;

        this.LastLerp = this.easing(0.0f, 1.0f, this.Reverse ? (this.duration - this.elapsed) : this.elapsed, this.duration, this.easing_overshoot_amplitude, this.easing_period);

        if (this.elapsed >= this.duration)
        {
            this.tweenCount--;
            if (this.tweenCount==0)
                this.elapsed = float.MaxValue;
            else
            {
                this.elapsed = 0.0f;
                if (this.loopType == TweenLoopType.Yoyo)
                    this.Reverse = !this.Reverse;
            }
        }
    }

    public void Kill()
    {
        this.duration = 0.0f;
    }

	public virtual void ForceComplete()
	{
		this.duration = 0.0f;
	}

	public TweenLerp<int> CreateTween(int start, int end)
	{
		return new TweenLerp<int>(start, end, SimpleTweener.IntLerp, this);
	}

    public TweenLerp<float> CreateTween(float start, float end)
    {
        return new TweenLerp<float>(start, end, SimpleTweener.FloatLerp, this);
    }

    public TweenLerp<Vector3> CreateTween(Vector3 start, Vector3 end)
    {
        return new TweenLerp<Vector3>(start, end, SimpleTweener.Vector3Lerp, this);
    }

	public TweenLerp<Vector2> CreateTween(Vector2 start, Vector2 end)
	{
        return new TweenLerp<Vector2>(start, end, SimpleTweener.Vector2Lerp, this);
	}

    public TweenLerp<Quaternion> CreateTween(Quaternion start, Quaternion end)
    {
        return new TweenLerp<Quaternion>(start, end, Quaternion.Lerp, this);
    }

    public TweenLerp<Color> CreateTween(Color start, Color end)
    {
        return new TweenLerp<Color>(start, end, SimpleTweener.ColorLerp, this);
    }

    public TweenLerp<TYPE> CreateTween<TYPE>(TYPE start, TYPE end, TweenLerp<TYPE>.Lerp lerpfn)
    {
        return new TweenLerp<TYPE>(start, end, lerpfn, this);
    }

    public static float FloatLerp(float start, float end, float t)
    {
        return start + (end - start) * t;
    }

	public static int IntLerp(int start, int end, float t)
	{
		return start + (int) ((float)(end - start) * t);
	}

    public static Vector3 Vector3Lerp(Vector3 start, Vector3 end, float t)
    {
        return start + (end - start) * t;
    }

	public static Vector2 Vector2Lerp(Vector2 start, Vector2 end, float t)
	{
		return start + (end - start) * t;
	}

    public static Color ColorLerp(Color start, Color end, float t)
    {
        return start + (end - start) * t;
    }

    /////////////////////////////////////////////////////////////////
    // private
    private float delay;

    private int tweenCount;
    private TweenLoopType loopType;

    private EasingObject.EasingPosition easing;
    private float easing_overshoot_amplitude;
    private float easing_period;
}


public struct TweenLerp<T>
{
    public delegate T Lerp(T start, T end, float t);

    public bool enabled;
    public T start;
    public T end;

    private Lerp lerp;
    private SimpleTweener tweener;

    public TweenLerp(T s, T e, Lerp l, SimpleTweener tweener)
    {
        this.enabled = true;
        this.start = s;
        this.end = e;

        this.tweener = tweener;
        this.lerp = l;
    }

    public T Start
    {
        get
        {
            return this.start;
        }
    }

    public T End
    {
        get
        {
            return this.end;
        }
    }

    public T Value
    {
        get
        {
            return this.tweener.SwapStartEnd ? this.lerp(this.end, this.start, this.tweener.LastLerp) : this.lerp(this.start, this.end, this.tweener.LastLerp);
        }
    }

    public SimpleTweener Tweener
    {
        get
        {
            return this.tweener;
        }
    }
}
