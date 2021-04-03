/*********************************************
 * NHN StarFish - Simple Tween
 * CHOI YOONBIN
 * 
 *********************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class STweenPosition : STweenBase<Vector3>
{

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // public

    public Vector3 Value
    {
        get { return tweenValue.Value; }
    }

    public override void Restore()
    {
        base.Restore();

        if (this.rectT != null)
            rectT.anchoredPosition3D = this.start;
        else
            this.transform.localPosition = this.start;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // overrides STweenBase 

    protected override void PlayTween()
    {
        base.PlayTween();
        this.rectT = this.GetComponent<RectTransform>();
        base.tweenValue = this.tweener.CreateTween(this.start, this.end);
    }

    protected override void UpdateValue(Vector3 value)
    {
        base.UpdateValue(value);

        if (this.rectT != null)
            rectT.anchoredPosition3D = value;
        else
            this.transform.localPosition = value;
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
