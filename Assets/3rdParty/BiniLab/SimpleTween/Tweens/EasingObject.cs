using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EasingObject
{
    private const float TWO_PI = Mathf.PI * 2.0f;
    public static Dictionary<EasingPosition, float[]> DEF_PARAM = new Dictionary<EasingPosition, float[]>()
    {
        {EasingObject.ElasticEasingIn, new float[2] {0.1f, 0.12f}},
        {EasingObject.ElasticEasingOut, new float[2] {0.1f, 0.12f}},
        {EasingObject.ElasticEasingInOut, new float[2] {0.1f, 0.12f}},
        {EasingObject.BackEasingIn, new float[2] {1.70158f, 0.0f}},
        {EasingObject.BackEasingOut, new float[2] {1.70158f, 0.0f}},
        {EasingObject.BackEasingInOut, new float[2] {1.70158f, 0.0f}}
    };

    public delegate float EasingPosition(float s, float e, float deltaTime, float duration, float overshoot_amplitude, float period);


    public static float LinearEasing(float s, float e, float deltaTime, float duration, float unused1, float unused2)
    {
        return (e - s) * deltaTime / duration + s;
    }

    public static float LinearEasingInOut(float s, float e, float deltaTime, float duration, float unused1, float unused2)
    {
        deltaTime /= duration / 2.0f;

        if (deltaTime < 1.0f)
            return (e - s) * deltaTime / duration + s;

        return (e - s) * (2.0f - deltaTime) / duration + s;
    }

    public static float QuadEasingIn(float s, float e, float deltaTime, float duration, float unused1, float unused2)
    {
        deltaTime /= duration;
        return (e - s) * deltaTime * deltaTime + s;
    }

    public static float QuadEasingOut(float s, float e, float deltaTime, float duration, float unused1, float unused2)
    {
        deltaTime /= duration;
        return (s - e) * deltaTime * (deltaTime - 2.0f) + s;
    }

    public static float QuadEasingInOut(float s, float e, float deltaTime, float duration, float unused1, float unused2)
    {
        deltaTime /= duration / 2.0f;

        if (deltaTime < 1.0f)
            return (e - s) / 2.0f * deltaTime * deltaTime + s;

        deltaTime--;
        return (s - e) / 2.0f * (deltaTime * (deltaTime - 2.0f) - 1) + s;
    }

    public static float CircEasingIn(float s, float e, float deltaTime, float duration, float unused1, float unused2)
    {
        deltaTime /= duration;
        return (s - e) * (Mathf.Sqrt(1.0f - deltaTime * deltaTime) - 1.0f) + s;
    }

    public static float CircEasingOut(float s, float e, float deltaTime, float duration, float unused1, float unused2)
    {
        deltaTime /= duration;
        deltaTime--;
        return (e - s) * Mathf.Sqrt(1 - deltaTime * deltaTime) + s;
    }

    public static float CircEasingInOut(float s, float e, float deltaTime, float duration, float unused1, float unused2)
    {
        deltaTime /= duration / 2.0f;
        if (deltaTime < 1.0f)
            return (s - e) / 2.0f * (Mathf.Sqrt(1.0f - deltaTime * deltaTime) - 1.0f) + s;

        deltaTime -= 2.0f;
        return (e - s) / 2.0f * (Mathf.Sqrt(1.0f - deltaTime * deltaTime) + 1.0f) + s;
    }

    public static float ExpoEasingIn(float s, float e, float deltaTime, float duration, float unused1, float unused2)
    {
        return (e - s) * Mathf.Pow(2.0f, 10.0f * (deltaTime / duration - 1.0f)) + s;
    }

    public static float ExpoEasingOut(float s, float e, float deltaTime, float duration, float unused1, float unused2)
    {
        return (e - s) * (-Mathf.Pow(2.0f, -10.0f * deltaTime / duration) + 1.0f) + s;
    }

    public static float ExpoEasingInOut(float s, float e, float deltaTime, float duration, float unused1, float unused2)
    {
        deltaTime /= duration / 2.0f;
        if (deltaTime < 1.0f)
            return (e - s) / 2.0f * Mathf.Pow(2.0f, 10.0f * (deltaTime - 1.0f)) + s;

        deltaTime--;
        return (e - s) / 2.0f * (-Mathf.Pow(2.0f, -10.0f * deltaTime) + 2.0f) + s;
    }

    public static float ElasticEasingIn(float s, float e, float deltaTime, float duration, float amplitude, float period)
    {
        float ss, d;

        d = e - s;

        if (deltaTime == 0.0f)
            return s;

        if ((deltaTime /= duration) == 1.0f)
            return e;

        if (period == 0)
            period = duration * 0.3f;

        if (amplitude == 0 || (d> 0 && amplitude < d) || (d < 0 && amplitude < -d))
        {
            amplitude = d;
            ss = period / 4;
        }
        else
            ss = period / TWO_PI * Mathf.Asin(d / amplitude);

        return -(amplitude * Mathf.Pow(2, 10 * (deltaTime -= 1.0f)) * Mathf.Sin((deltaTime * duration - ss) * TWO_PI / period)) + s;
    }

    public static float ElasticEasingOut(float s, float e, float deltaTime, float duration, float amplitude, float period)
    {
        float ss, d;

        d = e - s;

        if (deltaTime == 0.0f)
            return s;

        if ((deltaTime /= duration) == 1.0f)
            return e;

        if (period == 0)
            period = duration * 0.3f;

        if (amplitude == 0 || (d > 0 && amplitude < d) || (d < 0 && amplitude < -d))
        {
            amplitude = d;
            ss = period / 4;
        }
        else
            ss = period / TWO_PI * Mathf.Asin(d / amplitude);

        return (amplitude * Mathf.Pow(2, -10 * deltaTime) * Mathf.Sin((deltaTime * duration - ss) * TWO_PI / period) + e);
    }

    public static float ElasticEasingInOut(float s, float e, float deltaTime, float duration, float amplitude, float period)
    {
        float ss, d;

        d = e - s;

        if (deltaTime == 0)
            return s;

        if ((deltaTime /= duration * 0.5f) == 2)
            return e;

        if (period == 0)
            period = duration * (0.3f * 1.5f);

        if (amplitude == 0 || (d > 0 && amplitude < d) || (d < 0 && amplitude < -d))
        {
            amplitude = d;
            ss = period / 4;
        }
        else
            ss = period / TWO_PI * Mathf.Asin(d / amplitude);

        if (deltaTime < 1.0f)
            return -0.5f * (amplitude * Mathf.Pow(2, 10 * (deltaTime -= 1)) * Mathf.Sin((deltaTime * duration - ss) * TWO_PI / period)) + s;

        return amplitude * Mathf.Pow(2, -10 * (deltaTime -= 1)) * Mathf.Sin((deltaTime * duration - ss) * TWO_PI / period) * 0.5f + e;
    }

    public static float BackEasingIn(float s, float e, float deltaTime, float duration, float overshoot, float unused2)
    {
        return (e - s) * (deltaTime /= duration) * deltaTime * ((overshoot + 1) * deltaTime - overshoot) + s;
    }

    public static float BackEasingOut(float s, float e, float deltaTime, float duration, float overshoot, float unused2)
    {
        return (e - s) * ((deltaTime = deltaTime / duration - 1) * deltaTime * ((overshoot + 1) * deltaTime + overshoot) + 1) + s;
    }

    public static float BackEasingInOut(float s, float e, float deltaTime, float duration, float overshoot, float unused2)
    {
        if ((deltaTime /= duration * 0.5f) < 1)
        {
            return (e - s) * 0.5f * (deltaTime * deltaTime * (((overshoot *= 1.525f) + 1) * deltaTime - overshoot)) + s;
        }
        return (e - s) / 2 * ((deltaTime -= 2) * deltaTime * (((overshoot *= 1.525f) + 1) * deltaTime + overshoot) + 2) + s;
    }

    public static float BounceEasingIn(float s, float e, float deltaTime, float duration, float unused1, float unused2)
    {
        return (e - s) - BounceEasingOut(0.0f, e - s, duration - deltaTime, duration, unused1, unused2) + s;
    }

    public static float BounceEasingOut(float s, float e, float deltaTime, float duration, float unused1, float unused2)
    {
        if ((deltaTime /= duration) < (1.0f / 2.75f))
            return (e - s) * (7.5625f * deltaTime * deltaTime) + s;

        if (deltaTime < (2.0f / 2.75f))
            return (e - s) * (7.5625f * (deltaTime -= (1.5f / 2.75f)) * deltaTime + 0.75f) + s;

        if (deltaTime < (2.5f / 2.75f))
            return (e - s) * (7.5625f * (deltaTime -= (2.25f / 2.75f)) * deltaTime + 0.9375f) + s;

        return (e - s) * (7.5625f * (deltaTime -= (2.625f / 2.75f)) * deltaTime + 0.984375f) + s;
    }

    public static float BounceEasingInOut(float s, float e, float deltaTime, float duration, float unused1, float unused2)
    {
        if (deltaTime < duration * 0.5f)
            return BounceEasingIn(0.0f, e - s, deltaTime * 2.0f, duration, unused1, unused2) * 0.5f + s;

        return BounceEasingOut(0.0f, e - s, deltaTime * 2.0f - duration, duration, unused1, unused2) * 0.5f + (e - s) * 0.5f + s;
    }
}
