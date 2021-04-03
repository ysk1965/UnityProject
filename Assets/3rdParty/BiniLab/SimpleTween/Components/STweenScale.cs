/*********************************************
 * NHN StarFish - Simple Tween
 * CHOI YOONBIN
 * 
 *********************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class STweenScale : STweenBase<Vector3> {

	////////////////////////////////////////////////////////////////////////////////////////////////////
	// public

	public override void Restore ()
	{
		base.Restore ();
		this.transform.localScale = this.start;
	}

	////////////////////////////////////////////////////////////////////////////////////////////////////
	// overrides STweenBase 

	protected override void PlayTween ()
	{
		base.PlayTween ();
		base.tweenValue = this.tweener.CreateTween (this.start, this.end);
	}

	protected override void UpdateValue (Vector3 value)
	{
		this.transform.localScale = value;
	}
		
	////////////////////////////////////////////////////////////////////////////////////////////////////
	// public 
    public void Swap()
    {
        Vector3 swapVector = this.start;
        this.start = this.end;
        this.end = swapVector;
    }
	
	////////////////////////////////////////////////////////////////////////////////////////////////////
	// private 

}
