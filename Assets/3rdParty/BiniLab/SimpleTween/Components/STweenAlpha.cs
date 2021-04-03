/*********************************************
 * NHN StarFish - Simple Tween
 * CHOI YOONBIN
 * 
 *********************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class STweenAlpha : STweenBase<float>
{

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // public

    public float Value
    {
        get { return tweenValue.Value; }
    }

    public override void Restore()
    {
        base.Restore();

        if (this.canvasGroup != null)
            this.canvasGroup.alpha = this.start;
        else
        {
            CanvasRenderer cr = this.GetComponent<CanvasRenderer>();
            if (cr != null) cr.SetAlpha(this.start);
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // overrides STweenBase 

    protected void Awake()
    {
        this.canvasGroup = this.GetComponent<CanvasGroup>();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void PlayTween()
    {
        base.PlayTween();
        this.SetValue(this.start);
        base.tweenValue = this.tweener.CreateTween(this.start, this.end);
    }

    protected override void UpdateValue(float value)
    {
        base.UpdateValue(value);
        this.SetValue(value);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // private 

    private CanvasGroup canvasGroup;

    private void SetValue(float alphaValue)
    {
        if (this.canvasGroup != null)
            this.canvasGroup.alpha = alphaValue;
        else
        {
            CanvasRenderer cr = this.GetComponent<CanvasRenderer>();
            if (cr != null) cr.SetAlpha(alphaValue);
        }
    }

}
