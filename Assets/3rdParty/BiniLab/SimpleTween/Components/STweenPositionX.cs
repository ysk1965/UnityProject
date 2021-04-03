/*********************************************
 * NHN StarFish - Simple Tween
 * CHOI YOONBIN
 * 
 *********************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class STweenPositionX : STweenBase<float>
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

        if (rectT != null)
        {
            Vector2 cur = rectT.anchoredPosition;
            rectT.anchoredPosition = new Vector2(this.start, cur.y);
        }
        else
        {
            Vector3 cur = this.transform.localPosition;
            this.transform.localPosition = new Vector3(this.start, cur.y, cur.z);
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // overrides STweenBase 

    protected override void PlayTween()
    {
        base.PlayTween();
        this.rectT = this.GetComponent<RectTransform>();
        base.tweenValue = this.tweener.CreateTween(this.start, this.end);
    }

    protected override void UpdateValue(float value)
    {
        base.UpdateValue(value);

        if (rectT != null)
        {
            Vector2 cur = rectT.anchoredPosition;
            rectT.anchoredPosition = new Vector2(value, cur.y);
        }
        else
        {
            Vector3 cur = this.transform.localPosition;
            this.transform.localPosition = new Vector3(value, cur.y, cur.z);
        }
    }

    protected override void Update()
    {
        if (base.pause) return;
        if (!this.tweener.Completed)
        {
            this.tweener.Update(this.unscaledTime ? Time.unscaledDeltaTime : Time.smoothDeltaTime);
            this.UpdateValue(this.tweenValue.Value);
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // protected 

    protected void Awake()
    {
        this.rectT = this.GetComponent<RectTransform>();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // private 

    private RectTransform rectT;

}
