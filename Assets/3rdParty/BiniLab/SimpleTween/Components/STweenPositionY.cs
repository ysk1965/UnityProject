/*********************************************
 * NHN StarFish - Simple Tween
 * CHOI YOONBIN
 * 
 *********************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class STweenPositionY : STweenBase<float>
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
            rectT.anchoredPosition = new Vector2(cur.x, this.start);
        }
        else
        {
            Vector3 cur = this.transform.localPosition;
            this.transform.localPosition = new Vector3(cur.x, this.start, cur.z);
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
            rectT.anchoredPosition = new Vector2(cur.x, value);
        }
        else
        {
            Vector3 cur = this.transform.localPosition;
            this.transform.localPosition = new Vector3(cur.x, value, cur.z);
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
