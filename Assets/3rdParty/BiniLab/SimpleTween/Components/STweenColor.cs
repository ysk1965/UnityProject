/*********************************************
 * CHOI YOONBIN
 *********************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class STweenColor : STweenBase<Color>
{

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // public

    public Color Value
    {
        get { return tweenValue.Value; }
    }

    public override void Restore()
    {
        base.Restore();

        if (this.graphic != null)
            this.graphic.color = this.start;
        else if (this.renderer != null)
            this.renderer.color = this.start;
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // overrides STweenBase 

    protected override void Start()
    {
        this.graphic = this.GetComponent<Graphic>();
        this.renderer = this.GetComponent<SpriteRenderer>();
        base.Start();
    }

    protected override void PlayTween()
    {
        base.PlayTween();
        this.SetValue(this.start);
        base.tweenValue = this.tweener.CreateTween(this.start, this.end);
    }

    protected override void UpdateValue(Color value)
    {
        base.UpdateValue(value);
        this.SetValue(value);
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    // private 

    private Graphic graphic;

    private new SpriteRenderer renderer;

    private void SetValue(Color color)
    {
        if (this.graphic != null)
            this.graphic.color = color;
        else if (this.renderer != null)
            this.renderer.color = color;
    }

}
